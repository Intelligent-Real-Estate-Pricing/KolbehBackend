using Application.Cqrs.Commands;
using Services;

namespace Application.Users.Command.ResetPasswordByOtp;

public class ResetPasswordByOtpCommand(string phoneNumber, string otp, string password) : ICommand<ServiceResult>
{
    public string PhoneNumber { get; } = phoneNumber;
    public string Otp { get; set; } = otp;
    public string Password { get; set; } = password;
}