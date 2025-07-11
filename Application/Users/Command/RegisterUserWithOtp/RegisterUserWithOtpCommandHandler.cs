using Application.Cqrs.Commands;
using Common.Roles;
using Data.Contracts;
using Data.Repositories;
using Entities.Otp;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Services;
using Services.Services;
using System.Text.Json;

public class RegisterUserWithOtpCommandHandler : ICommandHandler<RegisterUserWithOtpCommand, ServiceResult<AccessToken>>
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<User> _userManager;
    private readonly ICacheService _cache;
    private const int MaxTryCount = 5;

    private readonly IJwtService _jwtService;

    public RegisterUserWithOtpCommandHandler(
        IUserRepository userRepository,
        ICacheService _cache,
        UserManager<User> userManager,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public async Task<ServiceResult<AccessToken>> Handle(RegisterUserWithOtpCommand request, CancellationToken cancellationToken)
    {
        var otpKey = $"otp:data:{request.PhoneNumber}";
        var cachedOtpJson = await _cache.GetAsync(otpKey);

        if (string.IsNullOrWhiteSpace(cachedOtpJson))
            return ServiceResult.BadRequest<AccessToken>("کد OTP منقضی شده است");

        var otpData = JsonSerializer.Deserialize<OtpData>(cachedOtpJson);
        if (otpData == null || !otpData.IsValid)
            return ServiceResult.BadRequest<AccessToken>("کد OTP معتبر نیست");

        if (otpData.Code != request.OtpCode)
        {
            otpData.TryCount++;
            if (otpData.TryCount >= MaxTryCount)
                otpData.IsValid = false;

            // به‌روزرسانی کش با TTL فعلی
            var ttl = await _cache.GetTimeToLiveAsync(otpKey) ?? TimeSpan.FromSeconds(60);
            await _cache.SetAsync(otpKey, JsonSerializer.Serialize(otpData), ttl);

            return ServiceResult.BadRequest<AccessToken>("کد OTP وارد شده صحیح نیست");
        }

        // ✅ بررسی شماره موبایل تکراری
        var userExists = await _userRepository.TableNoTracking.AnyAsync(u =>
            u.PhoneNumber == request.PhoneNumber, cancellationToken);

        if (userExists)
            return ServiceResult.BadRequest<AccessToken>("شماره موبایل یا کدملی وارده تکراری می‌باشد");

        // ✅ ساخت کاربر جدید
        var newUser = User.RegisterUser(request.PhoneNumber, request.FullName);
        newUser.AuthenticationMethod = AuthenticationMethod.OTP;

        var result = await _userManager.CreateAsync(newUser);
        if (!result.Succeeded)
        {
            return ServiceResult.BadRequest<AccessToken>(
                string.Join(", ", result.Errors.Select(e => e.Description))
            );
        }

        await _userManager.AddToRoleAsync(newUser, UserRoleNames.User);

        // ✅ حذف OTP
        await _cache.RemoveAsync(otpKey);

        // ✅ تولید JWT
        var jwt = await _jwtService.GenerateAsync(newUser);

        return ServiceResult.Ok(jwt);
    }

}
