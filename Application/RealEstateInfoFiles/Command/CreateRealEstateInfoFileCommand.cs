using Application.Cqrs.Commands;
using Data.Contracts;
using Entities.RealEstateInfoFiles;
using Services;

public record CreateRealEstateInfoFileCommand(CreateRealEstateInfoFileDTO Model) : ICommand<ServiceResult>;

public class CreateRealEstateInfoFileCommandHandler(IRepository<RealEstateInfoFile> repository)
    : ICommandHandler<CreateRealEstateInfoFileCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(CreateRealEstateInfoFileCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Model;

        var entity = new RealEstateInfoFile
        {
            Id = Guid.NewGuid(),
            Mantaghe = dto.Mantaghe,
            TotalFloors = dto.TotalFloors,
            UnitsPerFloor = dto.UnitsPerFloor,
            TotalPrice = dto.TotalPrice,
            PricePerSquareMeter = dto.PricePerSquareMeter,
            SquareMeters = dto.SquareMeters,
            Floors = dto.Floors,
            BuiltArea = dto.BuiltArea,
            Bedrooms = dto.Bedrooms,
            Balconies = dto.Balconies,
            HasTelephone = dto.HasTelephone,
            KitchenType = dto.KitchenType,
            IsKitchenOpen = dto.IsKitchenOpen,
            BathroomType = dto.BathroomType,
            FlooringType = dto.FlooringType,
            HasElevator = dto.HasElevator,
            FacadeType = dto.FacadeType,
            DocumentType = dto.DocumentType,
            HasParking = dto.HasParking,
            HasStorage = dto.HasStorage,
            BuildingAge = dto.BuildingAge,
            IsFurnished = dto.IsFurnished,
            HasGas = dto.HasGas,
            HasVideoIntercom = dto.HasVideoIntercom,
            HasCooler = dto.HasCooler,
            HasRemoteControlDoor = dto.HasRemoteControlDoor,
            HasRadiator = dto.HasRadiator,
            HasPackageHeater = dto.HasPackageHeater,
            IsRenovated = dto.IsRenovated,
            HasJacuzzi = dto.HasJacuzzi,
            HasSauna = dto.HasSauna,
            HasPool = dto.HasPool,
            HasLobby = dto.HasLobby,
            HasDuctSplit = dto.HasDuctSplit,
            HasChiller = dto.HasChiller,
            HasRoofGarden = dto.HasRoofGarden,
            HasMasterRoom = dto.HasMasterRoom,
            HasNoElevator = dto.HasNoElevator,
            HasSplitAC = dto.HasSplitAC,
            HasJanitorService = dto.HasJanitorService,
            HasMeetingHall = dto.HasMeetingHall,
            HasFanCoil = dto.HasFanCoil,
            HasGym = dto.HasGym,
            HasCCTV = dto.HasCCTV,
            HasEmergencyPower = dto.HasEmergencyPower,
            IsFlat = dto.IsFlat,
            HasUnderfloorHeating = dto.HasUnderfloorHeating,
            HasFireAlarm = dto.HasFireAlarm,
            HasFireExtinguishingSystem = dto.HasFireExtinguishingSystem,
            HasCentralVacuum = dto.HasCentralVacuum,
            HasSecurityDoor = dto.HasSecurityDoor,
            HasLaundry = dto.HasLaundry,
            IsSoldWithTenant = dto.IsSoldWithTenant,
            HasCentralAntenna = dto.HasCentralAntenna,
            HasBackyard = dto.HasBackyard,
            HasServantService = dto.HasServantService,
            HasBarbecue = dto.HasBarbecue,
            HasElectricalPanel = dto.HasElectricalPanel,
            HasConferenceHall = dto.HasConferenceHall,
            HasGuardService = dto.HasGuardService,
            HasAirHandler = dto.HasAirHandler,
            HasCentralSatellite = dto.HasCentralSatellite,
            HasGarbageChute = dto.HasGarbageChute,
            HasLobbyManService = dto.HasLobbyManService,
            HasGuardOrJanitorService = dto.HasGuardOrJanitorService,
            IsDeleted = false
        };

        await repository.AddAsync(entity, cancellationToken);

        return ServiceResult.Ok();
    }
}