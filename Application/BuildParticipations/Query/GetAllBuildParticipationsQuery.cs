using Application;
using Application.Cqrs.Queris;
using Data.Contracts;
using Microsoft.EntityFrameworkCore;
using Services;

public record GetAllBuildParticipationsQuery(GetAllBuildParticipationDto Filters)
    : IQuery<ServiceResult<GlobalGridResult<BuildParticipationDto>>>;


public class GetAllBuildParticipationsQueryHandler(IRepository<BuildParticipation> repository)
    : IQueryHandler<GetAllBuildParticipationsQuery, ServiceResult<GlobalGridResult<BuildParticipationDto>>>
{
    public async Task<ServiceResult<GlobalGridResult<BuildParticipationDto>>> Handle(
        GetAllBuildParticipationsQuery request,
        CancellationToken cancellationToken)
    {
        var filters = request.Filters;

        var query = repository.TableNoTracking.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filters.Zone))
            query = query.Where(x => x.Zone.Contains(filters.Zone));

        if (!string.IsNullOrWhiteSpace(filters.Neighborhood))
            query = query.Where(x => x.Neighborhood.Contains(filters.Neighborhood));

        if (filters.Role.HasValue)
            query = query.Where(x => x.Role == filters.Role);

        var total = await query.CountAsync(cancellationToken);

        var data = await query
            .Skip((filters.PageNumber.Value - 1) * filters.Count.Value)
            .Take(filters.Count.Value)
            .Select(x => new BuildParticipationDto
            {
                Id = x.Id,
                PhoneNumber = x.PhoneNumber,
                Role = x.Role   ,
                Zone = x.Zone,
                Neighborhood = x.Neighborhood,
                PassageWidth = x.PassageWidth,
                Frontage = x.Frontage,
                Area = x.Area
            })
            .ToListAsync(cancellationToken);

        var result = new GlobalGridResult<BuildParticipationDto>
        {
            Data = data,
            TotalCount = total
        };

        return ServiceResult.Ok(result);
    }
}

