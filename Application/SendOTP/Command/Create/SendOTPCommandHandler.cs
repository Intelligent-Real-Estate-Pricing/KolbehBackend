using Application.Cqrs.Commands;
using Data.Contracts;
using Entities.Otp  ;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services;
using Services.Services.SmsSender;
using System.Text.Json;
using static LoginWithOtpCommandHandler;
using Entities.Otp;
using Data.Repositories;
using Application.SendOTP.DTo;
using Common;

namespace Application.SendOTP.Command.Create;

/*public class SendOTPCommandHandler(IRepository<ValidationCode> repository, UserManager<User> userManager, ISmsSenderService smsSender) : ICommandHandler<SendOTPCommand, ServiceResult<string>>
{
    public async Task<ServiceResult> Handle(SendOTPCommand request, CancellationToken cancellationToken)
    {

        var user = await userManager.FindByNameAsync(request.PhoneNumber);
        if (user is not null)
            return ServiceResult.Fail("کاربری با این شماره موبایل ثبت نام کرده است");


        var code = await repository.Table.OrderByDescending(x => x.Id).Where(x => x.PhoneNumber == request.PhoneNumber).FirstOrDefaultAsync(cancellationToken);
        if (code is not null && code.IsValid)
        {
            var timeaLeft = TimeSpan.FromSeconds(120) - (DateTime.Now - code.CreatedAt);
            var secondsLeft = Math.Max(0, (int)timeaLeft.TotalSeconds);
            var result = secondsLeft.ToString();
               return ServiceResult.Ok( "کد با موفقیت ارسال شد");
   *//*         return ServiceResult.ExpiredCode(result);*//*
        }


        code = new ValidationCode
        {
            Code = Random.Shared.Next(10000, 99999).ToString(),
            PhoneNumber = request.PhoneNumber
        };
        await repository.AddAsync(code, cancellationToken, true);
        var Code = code.Code.ToString();

        await smsSender.SendOTP(request.PhoneNumber, code.Code);
        return ServiceResult.Ok<string>(Code, "کد با موفقیت ارسال شد");

    }
}
*/





public class SendOTPCommandHandler(
    
    ISmsSenderService smsSender,
    IRepository<User> UserRepository,
    ICacheService cache,
    IHttpContextAccessor httpContextAccessor
) : ICommandHandler<SendOTPCommand, ServiceResult<otpDto>>
{
    private const int OtpCooldownSeconds = 120;
    private const int MaxDailyLimit = 10;
    private const int OtpExpireSeconds = 180;

    public async Task<ServiceResult<otpDto>> Handle(SendOTPCommand request, CancellationToken cancellationToken)
    {
        var phone = request.PhoneNumber;
        var ip = GetClientIp();

        var cooldownKey = $"otp:cooldown:{phone}";
        var dailyLimitKey = $"otp:daily:{phone}:{DateTime.Now:yyyyMMdd}";
        var otpKey = $"otp:data:{phone}";

      var UserExsit=  UserRepository.TableNoTracking.Where(a => a.PhoneNumber == phone).Any();
        var Result = new otpDto()
        {
        UserExist=UserExsit
        };
        var cooldownTtl = await cache.GetTimeToLiveAsync(cooldownKey);
        if (cooldownTtl.HasValue && cooldownTtl.Value.TotalSeconds > 0)
        {
            var secondsLeft = (int)cooldownTtl.Value.TotalSeconds;
            return ServiceResult<otpDto>.Fail<otpDto>(new otpDto(),secondsLeft.ToString(), ApiResultStatusCode.ExpiredCode);

        }

        var sendCount = await cache.GetIntAsync(dailyLimitKey) ?? 0;
        if (sendCount >= MaxDailyLimit)
        return ServiceResult<otpDto>.Fail<otpDto>(new otpDto(), "سقف ارسال کد فعال‌سازی برای امروز به پایان رسیده", ApiResultStatusCode.ExpiredCode);


        var otpCode = Random.Shared.Next(10000, 99999).ToString();

        var otpInfo = new OtpData
        {
            Code = otpCode,
            CreatedAt = DateTime.Now.ToString("O"),
            ExpireAt = DateTime.Now.AddSeconds(OtpExpireSeconds).ToString("O"),
            TryCount = 0,
            IsValid = true,
            IP = ip
        };

        var otpJson = JsonSerializer.Serialize(otpInfo);

        await cache.SetAsync(otpKey, otpJson, TimeSpan.FromSeconds(OtpExpireSeconds));
        await cache.SetAsync(cooldownKey, "1", TimeSpan.FromSeconds(OtpCooldownSeconds));
        await cache.IncrementAsync(dailyLimitKey, 1, TimeSpan.FromHours(24));

      

        await smsSender.SendOTP(phone, otpCode);    

        return ServiceResult<otpDto>.Ok(Result, "کد فعال‌سازی با موفقیت ارسال شد");
    }

    private string GetClientIp()
    {
        return httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "unknown";
    }
}