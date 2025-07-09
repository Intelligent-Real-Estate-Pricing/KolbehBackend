using Application.Cqrs.Commands;
using Data.Contracts;
using Entities.Shared;
using Entities.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services;
using Services.Services.SmsSender;

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





/*public class SendOTPCommandHandler(
    IRepository<ValidationCode> repository,
    UserManager<User> userManager,
    ISmsSenderService smsSender,
*//*    ICacheService cache,*//*
    IHttpContextAccessor httpContextAccessor
) : ICommandHandler<SendOTPCommand, ServiceResult>
{
    private const int OtpCooldownSeconds = 120;
    private const int MaxDailyLimit = 10;
    private const int OtpExpireSeconds = 180;

    public async Task<ServiceResult> Handle(SendOTPCommand request, CancellationToken cancellationToken)
    {
        var phone = request.PhoneNumber;
        var ip = GetClientIp();

        // ✅ اگر کاربر قبلاً ثبت‌نام کرده، اجازه ارسال کد ندارد
        var user = await userManager.FindByNameAsync(phone);
        if (user is not null)
            return ServiceResult.Fail("کاربری با این شماره موبایل ثبت‌نام کرده است");

        var cooldownKey = $"otp:cooldown:{phone}";
        var dailyLimitKey = $"otp:daily:{phone}:{DateTime.Now:yyyyMMdd}";

        var cooldownTtl = await cache.GetTimeToLiveAsync(cooldownKey);
        if (cooldownTtl.HasValue && cooldownTtl.Value.TotalSeconds > 0)
        {
            var secondsLeft = (int)cooldownTtl.Value.TotalSeconds;
            return ServiceResult.ExpiredCode(secondsLeft.ToString());
     
        }

        var sendCount = await cache.GetIntAsync(dailyLimitKey) ?? 0;
        if (sendCount >= MaxDailyLimit)
            return ServiceResult.Fail("سقف ارسال کد فعال‌سازی برای امروز به پایان رسیده");

        var otpCode = Random.Shared.Next(10000, 99999).ToString();

        var validationCode = new ValidationCode
        {
            Code = otpCode,
            PhoneNumber = phone,
            CreatedAt = DateTime.Now,
            ExpireAt = DateTime.Now.AddSeconds(OtpExpireSeconds),
            TryCount = 0,
            IsValid = true
        };

        await repository.AddAsync(validationCode, cancellationToken);

        await smsSender.SendOTP(phone, otpCode);

        await cache.SetAsync(cooldownKey, "1", TimeSpan.FromSeconds(OtpCooldownSeconds));

        await cache.IncrementAsync(dailyLimitKey, 1, TimeSpan.FromHours(24));

        return ServiceResult.Ok("کد فعال‌سازی با موفقیت ارسال شد");
    }

    private string GetClientIp()
    {
        return httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "unknown";
    }
}
*/