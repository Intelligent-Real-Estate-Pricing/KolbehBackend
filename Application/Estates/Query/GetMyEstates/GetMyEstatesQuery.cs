using Application.Cqrs.Queris;
using Application.Estates.DTOs;
using Common.Utilities;
using Data.Contracts;
using Entities.Estates;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services;
using Services.Uploader.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Estates.Query.GetMyEstates
{
    public record GetMyEstatesQuery : IQuery<ServiceResult<List<EstateDTO>>>;
    public class GetMyEstatesQueryHandler(
        IRepository<SmartRealEstatePricing> estateRepository,
        IHttpContextAccessor httpContextAccessor)
        : IQueryHandler<GetMyEstatesQuery, ServiceResult<List<EstateDTO>>>
    {
        public async Task<ServiceResult<List<EstateDTO>>> Handle(GetMyEstatesQuery request, CancellationToken cancellationToken)
        {
           
            var userIdString = httpContextAccessor.HttpContext.User.Identity.GetUserId();
            if (userIdString == null)
                    return ServiceResult.BadRequest<List<EstateDTO>>("لطفا ابتدا لاگین کنید");
                
            var userId = Guid.Parse(userIdString);

            var estates = await estateRepository
                .Table
                .Where(x => x.UserId == userId && x.IsDeleted == false)
                .Select(x => new EstateDTO
                {
                    UserName = x.User.FullName,
                    NameNeighborhood = x.Zone,
                    PriceingWithAi=x.PriceingWithAi,
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
                    Neighborhood = x.Neighborhood
                })
                .ToListAsync(cancellationToken);

            return ServiceResult.Ok(estates);
        }
    }

}
