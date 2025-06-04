using MediatR;

namespace Application.Cqrs.Commands
{
    public interface ICommand<TCommandResult> : IRequest<TCommandResult>
    {

    }
}
