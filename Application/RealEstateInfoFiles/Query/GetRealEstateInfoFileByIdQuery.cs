using Application.Cqrs.Queris;
using Data.Contracts;
using Entities.RealEstateInfoFiles;
using Microsoft.EntityFrameworkCore;
using Services;

public record GetRealEstateInfoFileByIdQuery(Guid Id)
        : IQuery<ServiceResult<RealEstateInfoFileDTO>>;

public class GetRealEstateInfoFileByIdQueryHandler(IRepository<RealEstateInfoFile> repository)
    : IQueryHandler<GetRealEstateInfoFileByIdQuery, ServiceResult<RealEstateInfoFileDTO>>
{
    public async Task<ServiceResult<RealEstateInfoFileDTO>> Handle(
        GetRealEstateInfoFileByIdQuery request, CancellationToken cancellationToken)
    {
        var dto = await repository
            .Table
            .Where(x => x.Id == request.Id && x.IsDeleted == false)
            .Select(x => new RealEstateInfoFileDTO
            {
                Id = x.Id,
                Mantaghe = x.Mantaghe,
                TotalFloors = x.TotalFloors,
                UnitsPerFloor = x.UnitsPerFloor,
                TotalPrice = x.TotalPrice,
                PricePerSquareMeter = x.PricePerSquareMeter,
                SquareMeters = x.SquareMeters,
                Floors = x.Floors,
                BuiltArea = x.BuiltArea,
                Bedrooms = x.Bedrooms,
                Balconies = x.Balconies,
                HasTelephone = x.HasTelephone,
                KitchenType = x.KitchenType,
                IsKitchenOpen = x.IsKitchenOpen,
                BathroomType = x.BathroomType,
                FlooringType = x.FlooringType,
                HasElevator = x.HasElevator,
                FacadeType = x.FacadeType,
                DocumentType = x.DocumentType,
                HasParking = x.HasParking,
                HasStorage = x.HasStorage,
                BuildingAge = x.BuildingAge,
                IsFurnished = x.IsFurnished,
                HasGas = x.HasGas,
                HasVideoIntercom = x.HasVideoIntercom,
                HasCooler = x.HasCooler,
                HasRemoteControlDoor = x.HasRemoteControlDoor,
                HasRadiator = x.HasRadiator,
                HasPackageHeater = x.HasPackageHeater,
                IsRenovated = x.IsRenovated,
                HasJacuzzi = x.HasJacuzzi,
                HasSauna = x.HasSauna,
                HasPool = x.HasPool,
                HasLobby = x.HasLobby,
                HasDuctSplit = x.HasDuctSplit,
                HasChiller = x.HasChiller,
                HasRoofGarden = x.HasRoofGarden,
                HasMasterRoom = x.HasMasterRoom,
                HasNoElevator = x.HasNoElevator,
                HasSplitAC = x.HasSplitAC,
                HasJanitorService = x.HasJanitorService,
                HasMeetingHall = x.HasMeetingHall,
                HasFanCoil = x.HasFanCoil,
                HasGym = x.HasGym,
                HasCCTV = x.HasCCTV,
                HasEmergencyPower = x.HasEmergencyPower,
                IsFlat = x.IsFlat,
                HasUnderfloorHeating = x.HasUnderfloorHeating,
                HasFireAlarm = x.HasFireAlarm,
                HasFireExtinguishingSystem = x.HasFireExtinguishingSystem,
                HasCentralVacuum = x.HasCentralVacuum,
                HasSecurityDoor = x.HasSecurityDoor,
                HasLaundry = x.HasLaundry,
                IsSoldWithTenant = x.IsSoldWithTenant,
                HasCentralAntenna = x.HasCentralAntenna,
                HasBackyard = x.HasBackyard,
                HasServantService = x.HasServantService,
                HasBarbecue = x.HasBarbecue,
                HasElectricalPanel = x.HasElectricalPanel,
                HasConferenceHall = x.HasConferenceHall,
                HasGuardService = x.HasGuardService,
                HasAirHandler = x.HasAirHandler,
                HasCentralSatellite = x.HasCentralSatellite,
                HasGarbageChute = x.HasGarbageChute,
                HasLobbyManService = x.HasLobbyManService,
                HasGuardOrJanitorService = x.HasGuardOrJanitorService,
                CreatedDate = x.CreatedAt,
                UpdatedDate = x.UpdatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        return dto is null
            ? ServiceResult.NotFound<RealEstateInfoFileDTO>("فایل اطلاعات املاک مورد نظر یافت نشد.")
            : ServiceResult.Ok(dto);
    }
}