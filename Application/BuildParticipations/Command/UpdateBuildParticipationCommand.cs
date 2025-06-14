using Application.Cqrs.Commands;
using Data.Contracts;
using Services;

public record UpdateBuildParticipationCommand(Guid Id, BuildParticipationDto Dto) : ICommand<ServiceResult>;

public class UpdateBuildParticipationCommandHandler(IRepository<BuildParticipation> repo)
    : ICommandHandler<UpdateBuildParticipationCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(UpdateBuildParticipationCommand request, CancellationToken cancellationToken)
    {
        var entity = await repo.GetByIdAsync(cancellationToken, request.Id);
        if (entity == null) return ServiceResult.NotFound("رکورد یافت نشد");

        entity.PhoneNumber = request.Dto.PhoneNumber;
        entity.Role = request.Dto.Role;
        entity.Zone = request.Dto.Zone;
        entity.Neighborhood = request.Dto.Neighborhood;
        entity.PassageWidth = request.Dto.PassageWidth;
        entity.Frontage = request.Dto.Frontage;
        entity.Area = request.Dto.Area;

        await repo.UpdateAsync(entity, cancellationToken);
        return ServiceResult.Ok();
    }
}
