using Application.Cqrs.Commands;
using Services;

public class RegisterUserWithOtpCommand : ICommand<ServiceResult<AccessToken>>
{
    public string PhoneNumber { get; set; }
    public string FullName { get; set; }
    public string OtpCode { get; set; }
}
