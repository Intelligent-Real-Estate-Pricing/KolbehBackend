using Application.Cqrs.Commands;
using Application.Estates.DTOs;
using Common.Utilities;
using Data.Contracts;
using Entities.Estates;
using Microsoft.AspNetCore.Http;
using Services;

namespace Application.Estates.Command.CreateEstates
{
    public record CreateEstateCommand(CreateEstatesDTO Model) : ICommand<ServiceResult>;
    public class CreateEstateCommandHandler(
    IHttpContextAccessor httpContextAccessor,
    IRepository<SmartRealEstatePricing> estateRepository
) : ICommandHandler<CreateEstateCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(CreateEstateCommand request, CancellationToken cancellationToken)
        {
            var userIdString = httpContextAccessor.HttpContext?.User?.Identity?.GetUserId();
            if (string.IsNullOrWhiteSpace(userIdString))
                return ServiceResult.BadRequest("لطفا ابتدا لاگین کنید");

            var userId = Guid.Parse(userIdString);

            var dto = request.Model;

            var estate = SmartRealEstatePricing.Create(
                dto.NameNeighborhood,
                dto.Address,
                dto.Location,
                dto.RealEstateOperationType,
                dto.Title,
                dto.ConstructionYear,
                dto.TotalFloors,
                dto.UnitsPerFloor,
                dto.Area,
                dto.BathroomCount,
                dto.RoomCount,
                dto.NaturalLight,
                dto.DocumentType,
                dto.HasStorage,
                dto.HasTerrace,
                dto.HasParking,
                dto.HasElevator,
                dto.HasSauna,
                dto.HasJacuzzi,
                dto.HasRoofGarden,
                dto.HasPool,
                dto.HasLobby,
                dto.GreeneryLevel,
                dto.PassageWidth,
                dto.IsModernTexture,
                dto.PropertyType,
                dto.Neighborhood
            );

            estate.SetUserId(userId);   

            await estateRepository.AddAsync(estate, cancellationToken);

            return ServiceResult.Ok();
        }
    }

}
