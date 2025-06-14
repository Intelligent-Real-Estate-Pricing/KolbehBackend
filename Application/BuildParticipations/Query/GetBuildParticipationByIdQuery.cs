using Application.Cqrs.Queris;
using Data.Contracts;
using Microsoft.EntityFrameworkCore;
using Services;

public record GetBuildParticipationByIdQuery(Guid Id) : IQuery<ServiceResult<BuildParticipationDto>>;

public class GetBuildParticipationByIdQueryHandler(IRepository<BuildParticipation> repo)
    : IQueryHandler<GetBuildParticipationByIdQuery, ServiceResult<BuildParticipationDto>>
{
    public async Task<ServiceResult<BuildParticipationDto>> Handle(GetBuildParticipationByIdQuery request, CancellationToken cancellationToken)
    {
        var dto = await repo.TableNoTracking
            .Where(x => x.Id == request.Id)
            .Select(x => new BuildParticipationDto
            {
                Id = x.Id,
                PhoneNumber = x.PhoneNumber,
                Role = x.Role,
                Zone = x.Zone,
                Neighborhood = x.Neighborhood,
                PassageWidth = x.PassageWidth,
                Frontage = x.Frontage,
                Area = x.Area
            })
            .FirstOrDefaultAsync(cancellationToken);

        return dto == null
            ? ServiceResult.NotFound<BuildParticipationDto>("رکورد یافت نشد")
            : ServiceResult.Ok(dto);
    }
}
