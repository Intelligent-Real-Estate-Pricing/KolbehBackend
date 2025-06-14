using Application.Cqrs.Queris;
using Data.Contracts;
using Entities.RealEstateInfoFiles;
using Microsoft.EntityFrameworkCore;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RealEstateInfoFiles.Query
{
    public record GetAllRealEstateInfoFilesQuery(GetAllRealEstateInfoFileDTO Filters)
         : IQuery<ServiceResult<GlobalGridResult<GetRealEstateInfoFileListDto>>>;

    public class GetAllRealEstateInfoFilesQueryHandler(IRepository<RealEstateInfoFile> repository)
        : IQueryHandler<GetAllRealEstateInfoFilesQuery, ServiceResult<GlobalGridResult<GetRealEstateInfoFileListDto>>>
    {
        public async Task<ServiceResult<GlobalGridResult<GetRealEstateInfoFileListDto>>> Handle(
            GetAllRealEstateInfoFilesQuery request, CancellationToken cancellationToken)
        {
            var filters = request.Filters;
            var query = repository.Table.Where(x => x.IsDeleted == false);

            // Apply filters
            if (!string.IsNullOrWhiteSpace(filters.Mantaghe))
                query = query.Where(x => x.Mantaghe.Contains(filters.Mantaghe));

            if (filters.MinPrice.HasValue)
                query = query.Where(x => x.TotalPrice >= filters.MinPrice.Value);

            if (filters.MaxPrice.HasValue)
                query = query.Where(x => x.TotalPrice <= filters.MaxPrice.Value);

            if (filters.MinSquareMeters.HasValue)
                query = query.Where(x => x.SquareMeters >= filters.MinSquareMeters.Value);

            if (filters.MaxSquareMeters.HasValue)
                query = query.Where(x => x.SquareMeters <= filters.MaxSquareMeters.Value);

            if (filters.Bedrooms.HasValue)
                query = query.Where(x => x.Bedrooms == filters.Bedrooms.Value);

            if (filters.HasParking.HasValue)
                query = query.Where(x => x.HasParking == filters.HasParking.Value);

            if (filters.HasStorage.HasValue)
                query = query.Where(x => x.HasStorage == filters.HasStorage.Value);

            if (filters.HasElevator.HasValue)
                query = query.Where(x => x.HasElevator == filters.HasElevator.Value);

            if (filters.KitchenType.HasValue)
                query = query.Where(x => x.KitchenType == filters.KitchenType.Value);

            if (filters.BathroomType.HasValue)
                query = query.Where(x => x.BathroomType == filters.BathroomType.Value);

            if (filters.FlooringType.HasValue)
                query = query.Where(x => x.FlooringType == filters.FlooringType.Value);

            if (filters.FacadeType.HasValue)
                query = query.Where(x => x.FacadeType == filters.FacadeType.Value);

            if (filters.DocumentType.HasValue)
                query = query.Where(x => x.DocumentType == filters.DocumentType.Value);

            // Pagination
            var result = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((filters.PageNumber.Value - 1) * filters.Count.Value)
                .Take(filters.Count.Value)
                .Select(x => new GetRealEstateInfoFileListDto
                {
                    Id = x.Id,
                    Mantaghe = x.Mantaghe,
                    TotalPrice = x.TotalPrice,
                    SquareMeters = x.SquareMeters,
                    Bedrooms = x.Bedrooms,
                    HasParking = x.HasParking,
                    HasStorage = x.HasStorage,
                    HasElevator = x.HasElevator,
                    BuildingAge = x.BuildingAge,
                })
                .ToListAsync(cancellationToken);

            var total = await query.CountAsync(cancellationToken);

            var globalResult = new GlobalGridResult<GetRealEstateInfoFileListDto>
            {
                Data = result,
                TotalCount = total
            };

            return ServiceResult.Ok(globalResult);
        }
    }
}
