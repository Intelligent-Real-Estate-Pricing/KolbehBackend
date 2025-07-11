using Application.Cqrs.Commands;
using Data.Contracts;
using Data.Repositories;
using Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services;

namespace Application.Users.Command.ResetPasswordByOtp;


public class ResetPasswordByOtpCommandHandler(
       IRepository<User> _userRepository,
       UserManager<User> _userManager,
       ICacheService _cache
       ) : ICommandHandler<ResetPasswordByOtpCommand, ServiceResult>
{
 

    public async Task<ServiceResult> Handle(ResetPasswordByOtpCommand request, CancellationToken cancellationToken)
    {
        var otpKey = $"otp:code:{request.PhoneNumber}";
        var cachedOtp = await _cache.GetAsync(otpKey);

        if (string.IsNullOrWhiteSpace(cachedOtp) || cachedOtp != request.Otp)
            return ServiceResult.BadRequest<AccessToken>("کد وارد شده صحیح نیست یا منقضی شده است");

        var user = await _userRepository.Table
            .FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber, cancellationToken);

        if (user is null)
            return ServiceResult.BadRequest<AccessToken>("کاربر یافت نشد");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var result = await _userManager.ResetPasswordAsync(user, token, request.Password);

        if (!result.Succeeded)
        {
            var errorMessage = string.Join(" | ", result.Errors.Select(x => x.Description));
            return ServiceResult.BadRequest<AccessToken>(errorMessage);
        }

        // پاک کردن کد OTP از کش
        await _cache.RemoveAsync(otpKey);

        return ServiceResult.Ok();
    }
}
