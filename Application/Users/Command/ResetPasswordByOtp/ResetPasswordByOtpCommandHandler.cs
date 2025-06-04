using Application.Cqrs.Commands;
using Data.Contracts;
using Entities.Shared;
using Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services;

namespace Application.Users.Command.ResetPasswordByOtp;

public class ResetPasswordByOtpCommandHandler(
       IRepository<ValidationCode> repository,
       IRepository<User> userRepository,
       UserManager<User> userManager
       ) : ICommandHandler<ResetPasswordByOtpCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(ResetPasswordByOtpCommand request, CancellationToken cancellationToken)
    {
        var validationCode = await
           repository
           .Table
           .OrderByDescending(x => x.Id
           ).Where(x => x.PhoneNumber == request.PhoneNumber && x.Code == request.Otp)
           .FirstOrDefaultAsync(cancellationToken);

        if (validationCode is null || (validationCode is not null && !validationCode.IsValid))
            return ServiceResult.BadRequest<AccessToken>("کد وارد شده صحیح نیست یا منقضی شده است");

        var user = await userRepository.Table.Where(x => x.PhoneNumber == request.PhoneNumber).FirstOrDefaultAsync();
        if (user == null)
            return ServiceResult.BadRequest<AccessToken>("کاربر یافت نشد");

        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        var result = await userManager.ResetPasswordAsync(user, token, request.Password);

        if (!result.Succeeded)
        {
            var errorMessage = string.Join(" | ", result.Errors.Select(x => x.Description).ToList());
            return ServiceResult.BadRequest<AccessToken>(errorMessage);
        }

        return ServiceResult.Ok();
    }
}
