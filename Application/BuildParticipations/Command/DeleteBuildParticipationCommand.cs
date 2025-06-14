using Application.Cqrs.Commands;
using Data.Contracts;
using Services;

public record DeleteBuildParticipationCommand(Guid Id) : ICommand<ServiceResult>;

public class DeleteBuildParticipationCommandHandler(IRepository<BuildParticipation> repo)
    : ICommandHandler<DeleteBuildParticipationCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(DeleteBuildParticipationCommand request, CancellationToken cancellationToken)
    {
        var entity = await repo.GetByIdAsync(cancellationToken, request.Id);
        if (entity == null) return ServiceResult.NotFound("یافت نشد");

        await repo.DeleteAsync(entity, cancellationToken);
        return ServiceResult.Ok();
    }
}
