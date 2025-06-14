using Application.Cqrs.Commands;
using Data.Contracts;
using Services;

public record DeleteEstateDelegationCommand(Guid Id) : ICommand<ServiceResult>;

public class DeleteEstateDelegationCommandHandler(IRepository<EstateDelegation> repo)
    : ICommandHandler<DeleteEstateDelegationCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(DeleteEstateDelegationCommand request, CancellationToken cancellationToken)
    {
        var entity = await repo.GetByIdAsync(cancellationToken, request.Id);
        if (entity == null) return ServiceResult.NotFound("یافت نشد");

        await repo.DeleteAsync(entity, cancellationToken);
        return ServiceResult.Ok();
    }
}
