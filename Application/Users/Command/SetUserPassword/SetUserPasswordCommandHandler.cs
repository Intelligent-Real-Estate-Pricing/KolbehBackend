using Application.Cqrs.Commands;
using Data.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services;

public class SetUserPasswordCommandHandler : ICommandHandler<SetUserPasswordCommand, ServiceResult>
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<User> _userManager;

    public SetUserPasswordCommandHandler(
        IUserRepository userRepository,
        UserManager<User> userManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
    }

    public async Task<ServiceResult> Handle(SetUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.Table.FirstOrDefaultAsync(t => t.Id == request.UserId, cancellationToken);
        if (user == null)
            return ServiceResult.BadRequest("کاربر یافت نشد");

        // If user doesn't have a password, add one
        if (string.IsNullOrEmpty(user.PasswordHash))
        {
            var addPasswordResult = await _userManager.AddPasswordAsync(user, request.NewPassword);
            if (!addPasswordResult.Succeeded)
                return ServiceResult.BadRequest(string.Join(", ", addPasswordResult.Errors.Select(e => e.Description)));
        }
        else
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetResult = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            if (!resetResult.Succeeded)
                return ServiceResult.BadRequest(string.Join(", ", resetResult.Errors.Select(e => e.Description)));
        }

        // Enable password authentication
        user.EnablePasswordAuthentication();
        await _userRepository.UpdateAsync(user, cancellationToken);
        await _userManager.UpdateSecurityStampAsync(user);

        return ServiceResult.Ok();
    }
}
