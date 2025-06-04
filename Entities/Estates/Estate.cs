using Entities.Common;
using Entities.Estates.Enums;
using Entities.Users;

namespace Entities.Estates
{
    public class Estate :BaseEntity<Guid>
    {
        public User  User{ get; set; }
        /// <summary>
        /// یوزر سازنده
        /// </summary>
        public Guid? UserId { get; set; }
        /// <summary>
        /// منطقه
        /// </summary>
        public string NameNeighborhood { get; set; }

        /// <summary>
        /// ادرس
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// لوکیشن
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// نوع اگهی
        /// </summary>
        public RealEstateOperationType RealEstateOperationType { get; set; }
        /// <summary>
        ///     عنوان ملک
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// سال ساخت
        /// </summary>
        public  int ConstructionYear { get; set; }
        /// <summary>
        /// تعداد طبقه
        /// </summary>
        public int TotalFloors { get; set; }  
        /// <summary>
        /// تعداد واحد هر طبقه 
        /// </summary>
        public int UnitsPerFloor { get; set; }


        /// <summary>
        /// متراژ نقشه
        /// </summary>
        public double Area { get; set; }

        /// <summary>
        /// تعداد حمام
        /// </summary>
        public int BathroomCount { get; set; }

        /// <summary>
        /// تعداد اتاق
        /// </summary>
        public int RoomCount { get; set; }

        /// <summary>
        /// نورگیر
        /// </summary>
        public NaturalLightType NaturalLight { get; set; }

        /// <summary>
        /// قیمت هر متر
        /// </summary>

        public decimal PricePerSquareMeter { get; set; }

        /// <summary>
        /// نوع سند
        /// </summary>
        public DocumentType DocumentType { get; set; }

        // امکانات پایه
        public bool HasStorage { get; set; }
        public bool HasTerrace { get; set; }
        public bool HasParking { get; set; }
        public bool HasElevator { get; set; }

        // امکانات لاکچری
        public bool HasSauna { get; set; }
        public bool HasJacuzzi { get; set; }
        public bool HasRoofGarden { get; set; }
    }
}
