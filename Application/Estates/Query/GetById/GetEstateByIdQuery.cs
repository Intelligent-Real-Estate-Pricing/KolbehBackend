using Application.Cqrs.Queris;
using Application.Estates.DTOs;
using Data.Contracts;
using Entities.Estates;
using Microsoft.EntityFrameworkCore;
using Services;

namespace Application.Estates.Query.GetById
{
    public record GetEstateByIdQuery(Guid EstateId)
      : IQuery<ServiceResult<EstateDTO>>;
    public class GetEstateByIdQueryHandler(IRepository<Estate> estateRepository)
        : IQueryHandler<GetEstateByIdQuery, ServiceResult<EstateDTO>>
    {
        public async Task<ServiceResult<EstateDTO>> Handle(GetEstateByIdQuery request, CancellationToken cancellationToken)
        {
            var dto = await estateRepository
                .Table.
                Include(a=>a.User)
                .Where(x => x.Id == request.EstateId && x.IsDeleted == false)
                .Select(x => new EstateDTO
                {
                    UserName = x.User.FullName,
                    NameNeighborhood = x.NameNeighborhood,
                    Address = x.Address,
                    Location = x.Location,
                    RealEstateOperationType = x.RealEstateOperationType,
                    Title = x.Title,
                    ConstructionYear = x.ConstructionYear,
                    TotalFloors = x.TotalFloors,
                    UnitsPerFloor = x.UnitsPerFloor,
                    Area = x.Area,
                    BathroomCount = x.BathroomCount,
                    RoomCount = x.RoomCount,
                    NaturalLight = x.NaturalLight.ToList(),
                    PricePerSquareMeter = x.PricePerSquareMeter,
                    DocumentType = x.DocumentType,
                    HasStorage = x.HasStorage,
                    HasTerrace = x.HasTerrace,
                    HasParking = x.HasParking,
                    HasElevator = x.HasElevator,
                    HasSauna = x.HasSauna,
                    HasJacuzzi = x.HasJacuzzi,
                    HasRoofGarden = x.HasRoofGarden,
                    HasPool = x.HasPool,
                    HasLobby = x.HasLobby,
                    GreeneryLevel = x.GreeneryLevel,
                    PassageWidth = x.PassageWidth,
                    IsModernTexture = x.IsModernTexture,
                    PropertyType = x.PropertyType,
                    Neighborhood = x.Neighborhood,
                })
                .FirstOrDefaultAsync(cancellationToken);

            return dto is null
                ? ServiceResult.NotFound<EstateDTO>("ملک مورد نظر یافت نشد.")
                : ServiceResult.Ok(dto);
        }
    }

}
