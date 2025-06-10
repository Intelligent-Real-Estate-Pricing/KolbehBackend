using Entities.Estates.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Estates.DTOs
{
    public class GetAllEstateDTO : GlobalGrid
    {
        public int? YearFrom { get; set; }
        public int? YearTo { get; set; }

        public double? AreaInMeters { get; set; }

        public int? FloorNumber { get; set; }

        public int? RoomCount { get; set; }

        public int? BathroomCount { get; set; }

        public bool? HasElevator { get; set; }

        public bool? HasWarehouse { get; set; }

        public bool? HasParking { get; set; }

        public bool? HasTerrace { get; set; }

        public List<NaturalLightType> NaturalLights { get; set; }
    }
}
