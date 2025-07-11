using Application.Cqrs.Commands;
using Application.SendOTP.DTo;
using Services;

namespace Application.SendOTP.Command.Create;

public class SendOTPCommand(string phoneNumber) : ICommand<ServiceResult<otpDto>> 
{
    public string PhoneNumber { get; } = phoneNumber;
}
