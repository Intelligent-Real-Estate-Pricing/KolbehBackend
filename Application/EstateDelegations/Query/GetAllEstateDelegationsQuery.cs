using Application.Cqrs.Queris;
using Application;
using Services;
using Data.Contracts;
using Microsoft.EntityFrameworkCore;

public record GetAllEstateDelegationsQuery(GetAllEstateDelegationDto Filters)
    : IQuery<ServiceResult<GlobalGridResult<EstateDelegationDto>>>;

public class GetAllEstateDelegationsQueryHandler(IRepository<EstateDelegation> repository)
    : IQueryHandler<GetAllEstateDelegationsQuery, ServiceResult<GlobalGridResult<EstateDelegationDto>>>
{
    public async Task<ServiceResult<GlobalGridResult<EstateDelegationDto>>> Handle(
        GetAllEstateDelegationsQuery request,
        CancellationToken cancellationToken)
    {
        var filters = request.Filters;

        var query = repository.TableNoTracking.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filters.Zone))
            query = query.Where(x => x.Zone.Contains(filters.Zone));

        if (!string.IsNullOrWhiteSpace(filters.FullName))
            query = query.Where(x => x.FullName.Contains(filters.FullName));

        if (filters.Type.HasValue)
            query = query.Where(x => x.Type == filters.Type.Value);

        var total = await query.CountAsync(cancellationToken);

        var data = await query
            .Skip((filters.PageNumber.Value - 1) * filters.Count.Value)
            .Take(filters.Count.Value)
            .Select(x => new EstateDelegationDto
            {
                Id = x.Id,
                FullName = x.FullName,
                PhoneNumber = x.PhoneNumber,
                Title = x.Title,
                Zone = x.Zone,
                Address = x.Address,
                Type = x.Type
            })
            .ToListAsync(cancellationToken);

        return ServiceResult.Ok(new GlobalGridResult<EstateDelegationDto>
        {
            TotalCount = total,
            Data = data
        });
    }
}
