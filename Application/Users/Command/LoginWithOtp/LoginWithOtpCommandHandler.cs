using Application.Cqrs.Commands;
using Application.Users.DTOs;
using Data.Contracts;
using Data.Repositories;
using Entities.Otp;
using Microsoft.EntityFrameworkCore;
using Services;
using Services.Services;
using System.Text.Json;

public partial class LoginWithOtpCommandHandler : ICommandHandler<LoginWithOtpCommand, ServiceResult<TokenResponseDTO>>
{
    private readonly IUserRepository _userRepository;
    private readonly ICacheService _cache;
    private readonly IJwtService _jwtService;
    private const int MaxTryCount = 5;
    public LoginWithOtpCommandHandler(
        ICacheService cache,
        IUserRepository userRepository,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _cache = cache;
        _jwtService = jwtService;
    }

    /*    public async Task<ServiceResult<TokenResponseDTO>> Handle(LoginWithOtpCommand request, CancellationToken cancellationToken)
        {
            var otpKey = $"otp:code:{request.PhoneNumber}";
            var cachedCode = await _cache.GetAsync(otpKey);
            if (string.IsNullOrWhiteSpace(cachedCode) || cachedCode != request.OtpCode)
                return ServiceResult.BadRequest<TokenResponseDTO>("کد OTP وارد شده صحیح نیست یا منقضی شده است");

            // Find user
            var user = await _userRepository.Table
                .Where(x => x.PhoneNumber == request.PhoneNumber)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
                return ServiceResult.BadRequest<TokenResponseDTO>("کاربر یافت نشد");

            if (!user.CanAuthenticateWithOTP())
                return ServiceResult.BadRequest<TokenResponseDTO>("احراز هویت با OTP برای این کاربر فعال نیست");

            // Update last login
            await _userRepository.UpdateLastLoginDateAsync(user, cancellationToken);

            // Mark OTP as use  d
            await _cache.RemoveAsync(otpKey);
            // Generate JWT
            var jwt = await _jwtService.GenerateAsync(user);
            var userDto = await GetUserDto(user);

            var tokenResponse = new TokenResponseDTO
            {
                AccessToken = jwt,
                User = userDto
            };

            return ServiceResult.Ok(tokenResponse);
        }
    */
    public async Task<ServiceResult<TokenResponseDTO>> Handle(LoginWithOtpCommand request, CancellationToken cancellationToken)
    {
        var otpKey = $"otp:data:{request.PhoneNumber}";
        var cachedOtpJson = await _cache.GetAsync(otpKey);

        if (string.IsNullOrWhiteSpace(cachedOtpJson))
            return ServiceResult.BadRequest<TokenResponseDTO>("کد فعال‌سازی منقضی شده است");

        var otpData = JsonSerializer.Deserialize<OtpData>(cachedOtpJson);

        if (otpData == null || !otpData.IsValid || otpData.Code != request.OtpCode)
            return ServiceResult.BadRequest<TokenResponseDTO>("کد OTP وارد شده صحیح نیست");

        if (otpData.Code != request.OtpCode)
        {
            otpData.TryCount++;


            if (otpData.TryCount >= MaxTryCount)
            {
                otpData.IsValid = false;
            }

            var ttl = await _cache.GetTimeToLiveAsync(otpKey) ?? TimeSpan.FromSeconds(60); 
            var updatedJson = JsonSerializer.Serialize(otpData);
            await _cache.SetAsync(otpKey, updatedJson, ttl);

            return ServiceResult.BadRequest<TokenResponseDTO>("کد OTP اشتباه است");
        }
        // ✅ پیدا کردن کاربر از دیتابیس
        var user = await _userRepository.Table
            .Where(x => x.PhoneNumber == request.PhoneNumber)
            .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
            return ServiceResult.BadRequest<TokenResponseDTO>("کاربر یافت نشد");

        if (!user.CanAuthenticateWithOTP())
            return ServiceResult.BadRequest<TokenResponseDTO>("احراز هویت با OTP برای این کاربر فعال نیست");

        // ✅ به‌روزرسانی تاریخ ورود
        await _userRepository.UpdateLastLoginDateAsync(user, cancellationToken);

        // ✅ حذف OTP از کش
        await _cache.RemoveAsync(otpKey);

        // ✅ تولید توکن
        var jwt = await _jwtService.GenerateAsync(user);
        var userDto = await GetUserDto(user);

        var tokenResponse = new TokenResponseDTO
        {
            AccessToken = jwt,
            User = userDto
        };

        return ServiceResult.Ok(tokenResponse);
    }
    private async Task<UserDTO> GetUserDto(User user)
    {
        return await _userRepository.Table
            .Where(x => x.Id == user.Id)
            .Select(u => new UserDTO
            {
                FullName = u.FullName,
                Id = u.Id,
                IsActive = u.IsActive,
                PhoneNumber = u.PhoneNumber,
                Email = u.Email,
                EmailConfirmed = u.EmailConfirmed,
                AuthenticationMethod = u.AuthenticationMethod,
                Roles = u.UserRoles.Select(x => new RoleDTO
                {
                    Description = x.Role.Description,
                    Name = x.Role.Name
                }).ToArray()
            }).FirstOrDefaultAsync();
    }
}


