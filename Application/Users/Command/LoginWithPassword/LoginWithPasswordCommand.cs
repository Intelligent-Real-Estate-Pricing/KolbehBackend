using Application.Cqrs.Commands;
using Application.Users.DTOs;
using Services;

public class LoginWithPasswordCommand : ICommand<ServiceResult<TokenResponseDTO>>
{
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
}
