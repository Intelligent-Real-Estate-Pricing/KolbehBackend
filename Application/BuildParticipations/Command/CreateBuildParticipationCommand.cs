using Application.Cqrs.Commands;
using Data.Contracts;
using Services;

public record CreateBuildParticipationCommand(BuildParticipationDto Dto) : ICommand<ServiceResult>;

public class CreateBuildParticipationCommandHandler(IRepository<BuildParticipation> repo)
    : ICommandHandler<CreateBuildParticipationCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(CreateBuildParticipationCommand request, CancellationToken cancellationToken)
    {
        var entity = new BuildParticipation
        {
            Id = Guid.NewGuid(),
            PhoneNumber = request.Dto.PhoneNumber,
            Role = request.Dto.Role,
            Zone = request.Dto.Zone,
            Neighborhood = request.Dto.Neighborhood,
            PassageWidth = request.Dto.PassageWidth,
            Frontage = request.Dto.Frontage,
            Area = request.Dto.Area
        };

        await repo.AddAsync(entity, cancellationToken);
        return ServiceResult.Ok();
    }
}
