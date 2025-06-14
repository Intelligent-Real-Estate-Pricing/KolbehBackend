using Application.Cqrs.Commands;
using Services;

namespace Application.SendOTP.Command.Create;

public class SendOTPCommand(string phoneNumber) : ICommand<ServiceResult<string>>
{
    public string PhoneNumber { get; } = phoneNumber;
}
