using Application.Cqrs.Queris;
using Application.Estates.DTOs;
using Data.Contracts;
using Entities.Estates;
using Microsoft.EntityFrameworkCore;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Estates.Query.GetAll
{
    public record GetAllEstatesQuery(GetAllEstateDTO Filters)
     : IQuery<ServiceResult<GlobalGridResult<GetEstateListDto>>>;

    public class GetAllEstatesQueryHandler(IRepository<SmartRealEstatePricing> estateRepository)
    : IQueryHandler<GetAllEstatesQuery, ServiceResult<GlobalGridResult<GetEstateListDto>>>
    {
        public async Task<ServiceResult<GlobalGridResult<GetEstateListDto>>> Handle(GetAllEstatesQuery request, CancellationToken cancellationToken)
        {
            var filters = request.Filters;
            var query = estateRepository.Table.Where(x => x.IsDeleted == false);

            if (filters.YearFrom.HasValue)
                query = query.Where(x => x.ConstructionYear >= filters.YearFrom.Value);

            if (filters.YearTo.HasValue)
                query = query.Where(x => x.ConstructionYear <= filters.YearTo.Value);

            if (filters.AreaInMeters.HasValue)
                query = query.Where(x => x.Area>= filters.AreaInMeters.Value);

            if (filters.FloorNumber.HasValue)
                query = query.Where(x => x.FloorNumber == filters.FloorNumber.Value);

            if (filters.RoomCount.HasValue)
                query = query.Where(x => x.RoomCount == filters.RoomCount.Value);

            if (filters.BathroomCount.HasValue)
                query = query.Where(x => x.BathroomCount == filters.BathroomCount.Value);

            if (filters.HasElevator.HasValue)
                query = query.Where(x => x.HasElevator == filters.HasElevator.Value);

            if (filters.HasWarehouse.HasValue)
                query = query.Where(x => x.HasStorage == filters.HasWarehouse.Value);

            if (filters.HasParking.HasValue)
                query = query.Where(x => x.HasParking == filters.HasParking.Value);

            if (filters.HasTerrace.HasValue)
                query = query.Where(x => x.HasTerrace == filters.HasTerrace.Value);

            if (filters.NaturalLights is { Count: > 0 })
            {
                query = query.Where(x => x.NaturalLight.Any(nl => filters.NaturalLights.Contains(nl)));
            }

            var result = await query
                .Skip((request.Filters.PageNumber.Value - 1) * request.Filters.Count.Value)
                .Take(request.Filters.Count.Value)
                .Select(x => new GetEstateListDto
                {
                    Id = x.Id,
                    Title = x.Title
                })
                .ToListAsync(cancellationToken);

            var total = await query.CountAsync(cancellationToken);

            var globalResult = new GlobalGridResult<GetEstateListDto>
            {
                Data = result,
                TotalCount = total
            };

            return ServiceResult.Ok(globalResult);
        }
    }

}
