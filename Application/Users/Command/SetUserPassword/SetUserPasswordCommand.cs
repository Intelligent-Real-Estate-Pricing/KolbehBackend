using Application.Cqrs.Commands;
using Services;

public class SetUserPasswordCommand : ICommand<ServiceResult>
{
    public Guid UserId { get; set; }
    public string NewPassword { get; set; }
}
