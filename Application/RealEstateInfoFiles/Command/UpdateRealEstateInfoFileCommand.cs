using Application.Cqrs.Commands;
using Application.RealEstateInfoFiles.DTOs;
using Data.Contracts;
using Entities.RealEstateInfoFiles;
using Microsoft.EntityFrameworkCore;
using Services;

public record UpdateRealEstateInfoFileCommand(Guid Id, UpdateRealEstateInfoFileDTO Model) : ICommand<ServiceResult>;

public class UpdateRealEstateInfoFileCommandHandler(IRepository<RealEstateInfoFile> repository)
    : ICommandHandler<UpdateRealEstateInfoFileCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(UpdateRealEstateInfoFileCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.Table
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.IsDeleted == false, cancellationToken);

        if (entity == null)
            return ServiceResult.NotFound("فایل اطلاعات املاک مورد نظر یافت نشد.");

        var dto = request.Model;

        // Update properties
        entity.Mantaghe = dto.Mantaghe;
        entity.TotalFloors = dto.TotalFloors;
        entity.UnitsPerFloor = dto.UnitsPerFloor;
        entity.TotalPrice = dto.TotalPrice;
        entity.PricePerSquareMeter = dto.PricePerSquareMeter;
        entity.SquareMeters = dto.SquareMeters;
        entity.Floors = dto.Floors;
        entity.BuiltArea = dto.BuiltArea;
        entity.Bedrooms = dto.Bedrooms;
        entity.Balconies = dto.Balconies;
        entity.HasTelephone = dto.HasTelephone;
        entity.KitchenType = dto.KitchenType;
        entity.IsKitchenOpen = dto.IsKitchenOpen;
        entity.BathroomType = dto.BathroomType;
        entity.FlooringType = dto.FlooringType;
        entity.HasElevator = dto.HasElevator;
        entity.FacadeType = dto.FacadeType;
        entity.DocumentType = dto.DocumentType;
        entity.HasParking = dto.HasParking;
        entity.HasStorage = dto.HasStorage;
        entity.BuildingAge = dto.BuildingAge;
        entity.IsFurnished = dto.IsFurnished;
        entity.HasGas = dto.HasGas;
        entity.HasVideoIntercom = dto.HasVideoIntercom;
        entity.HasCooler = dto.HasCooler;
        entity.HasRemoteControlDoor = dto.HasRemoteControlDoor;
        entity.HasRadiator = dto.HasRadiator;
        entity.HasPackageHeater = dto.HasPackageHeater;
        entity.IsRenovated = dto.IsRenovated;
        entity.HasJacuzzi = dto.HasJacuzzi;
        entity.HasSauna = dto.HasSauna;
        entity.HasPool = dto.HasPool;
        entity.HasLobby = dto.HasLobby;
        entity.HasDuctSplit = dto.HasDuctSplit;
        entity.HasChiller = dto.HasChiller;
        entity.HasRoofGarden = dto.HasRoofGarden;
        entity.HasMasterRoom = dto.HasMasterRoom;
        entity.HasNoElevator = dto.HasNoElevator;
        entity.HasSplitAC = dto.HasSplitAC;
        entity.HasJanitorService = dto.HasJanitorService;
        entity.HasMeetingHall = dto.HasMeetingHall;
        entity.HasFanCoil = dto.HasFanCoil;
        entity.HasGym = dto.HasGym;
        entity.HasCCTV = dto.HasCCTV;
        entity.HasEmergencyPower = dto.HasEmergencyPower;
        entity.IsFlat = dto.IsFlat;
        entity.HasUnderfloorHeating = dto.HasUnderfloorHeating;
        entity.HasFireAlarm = dto.HasFireAlarm;
        entity.HasFireExtinguishingSystem = dto.HasFireExtinguishingSystem;
        entity.HasCentralVacuum = dto.HasCentralVacuum;
        entity.HasSecurityDoor = dto.HasSecurityDoor;
        entity.HasLaundry = dto.HasLaundry;
        entity.IsSoldWithTenant = dto.IsSoldWithTenant;
        entity.HasCentralAntenna = dto.HasCentralAntenna;
        entity.HasBackyard = dto.HasBackyard;
        entity.HasServantService = dto.HasServantService;
        entity.HasBarbecue = dto.HasBarbecue;
        entity.HasElectricalPanel = dto.HasElectricalPanel;
        entity.HasConferenceHall = dto.HasConferenceHall;
        entity.HasGuardService = dto.HasGuardService;
        entity.HasAirHandler = dto.HasAirHandler;
        entity.HasCentralSatellite = dto.HasCentralSatellite;
        entity.HasGarbageChute = dto.HasGarbageChute;
        entity.HasLobbyManService = dto.HasLobbyManService;
        entity.HasGuardOrJanitorService = dto.HasGuardOrJanitorService;
        entity.UpdatedAt = DateTime.UtcNow;

        await repository.UpdateAsync(entity, cancellationToken);

        return ServiceResult.Ok();
    }
}