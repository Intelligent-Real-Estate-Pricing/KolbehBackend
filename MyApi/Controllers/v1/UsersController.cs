using Application.Cqrs.Commands;
using Application.Cqrs.Queris;
using Application.SendOTP.Command.Create;
using Application.SendOTP.DTo;
using Application.Users.DTOs;
using Asp.Versioning;
using Kolbeh.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Security.Claims;
using WebFramework.Api;

/// <summary>
/// یوزر
/// </summary>
[ApiVersion("1")]
public class UsersController : BaseController
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ILogger<UsersController> _logger;

    public UsersController(
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher,
        ILogger<UsersController> logger)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
        _logger = logger;
    }

    /// <summary>
    /// ثبت نام کاربر با OTP
    /// </summary>
    [HttpPost("[action]")]
    [AllowAnonymous]
    public async Task<ApiResult<AccessToken>> RegisterUserWithOtp(
        [FromBody] RegisterUserWithOtpDTO input,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserWithOtpCommand
        {
            PhoneNumber = input.PhoneNumber,
            FullName = input.FullName,
            OtpCode = input.OtpCode
        };

        var result = await _commandDispatcher.SendAsync(command, cancellationToken);
        return result.ToApiResult();
    }

    /// <summary>
    /// ورود با OTP
    /// </summary>
    [HttpPost("[action]")]
    [AllowAnonymous]
    public async Task<ApiResult<TokenResponseDTO>> LoginWithOtp(
        [FromBody] LoginWithOtpDTO input,
        CancellationToken cancellationToken)
    {
        var command = new LoginWithOtpCommand
        {
            PhoneNumber = input.PhoneNumber,
            OtpCode = input.OtpCode
        };

        var result = await _commandDispatcher.SendAsync(command, cancellationToken);
        return result.ToApiResult();
    }

    /// <summary>
    /// ورود با رمز عبور (فقط برای کاربرانی که رمز عبور تنظیم کرده‌اند)
    /// </summary>
    [HttpPost("[action]")]
    [AllowAnonymous]
    public async Task<ApiResult<TokenResponseDTO>> LoginWithPassword(
        [FromBody] LoginWithPasswordDTO input,
        CancellationToken cancellationToken)
    {
        var command = new LoginWithPasswordCommand
        {
            PhoneNumber = input.PhoneNumber,
            Password = input.Password
        };

        var result = await _commandDispatcher.SendAsync(command, cancellationToken);
        return result.ToApiResult();
    }

    /// <summary>
    /// تنظیم رمز عبور برای کاربر (فعال‌سازی احراز هویت با رمز عبور)
    /// </summary>
    [HttpPost("[action]")]
    [Authorize]
    public async Task<ApiResult> SetPassword(
        [FromBody] SetPasswordDTO input,
        CancellationToken cancellationToken)
    {
        if (input.NewPassword != input.ConfirmPassword)
            return BadRequest("رمز عبور و تکرار آن مطابقت ندارند");

        var userId = GetCurrentUserId(); // Extension method to get current user ID
        var command = new SetUserPasswordCommand
        {
            UserId = userId,
            NewPassword = input.NewPassword
        };

        var result = await _commandDispatcher.SendAsync(command, cancellationToken);
        return result.ToApiResult();
    }

    /// <summary>
    /// به‌روزرسانی رمز عبور
    /// </summary>
    [HttpPost("[action]")]
    [Authorize]
    public async Task<ApiResult> UpdatePassword(
        [FromBody] SetPasswordDTO input,
        CancellationToken cancellationToken)
    {
        if (input.NewPassword != input.ConfirmPassword)
            return BadRequest("رمز عبور و تکرار آن مطابقت ندارند");

        var userId = GetCurrentUserId();
        var command = new SetUserPasswordCommand
        {
            UserId = userId,
            NewPassword = input.NewPassword
        };

        var result = await _commandDispatcher.SendAsync(command, cancellationToken);
        return result.ToApiResult();
    }
    /// <summary>
    /// ارسال OTP
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("[action]")]
    [AllowAnonymous]
    public virtual async Task<ApiResult<otpDto>> SendOtp(string phoneNumber, CancellationToken cancellationToken) =>
      (await _commandDispatcher.SendAsync(new SendOTPCommand(phoneNumber), cancellationToken)).ToApiResult();


    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim);
    }
}