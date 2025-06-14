using Application.Cqrs.Queris;
using Data.Contracts;
using Microsoft.EntityFrameworkCore;
using Services;

public record GetEstateDelegationByIdQuery(Guid Id)
    : IQuery<ServiceResult<EstateDelegationDto>>;

public class GetEstateDelegationByIdQueryHandler(IRepository<EstateDelegation> repository)
    : IQueryHandler<GetEstateDelegationByIdQuery, ServiceResult<EstateDelegationDto>>
{
    public async Task<ServiceResult<EstateDelegationDto>> Handle(
        GetEstateDelegationByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await repository.TableNoTracking
            .Where(x => x.Id == request.Id)
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
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
            return ServiceResult.NotFound<EstateDelegationDto>("آیتم مورد نظر یافت نشد.");

        return ServiceResult.Ok(entity);
    }
}
