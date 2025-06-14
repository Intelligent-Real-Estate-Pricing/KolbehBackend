using Entities.Common;
using Entities.Estates.Enums;
using Entities.Notifications;
using Entities.Users;

namespace Entities.Estates
{
    public class SmartRealEstatePricing : BaseEntity<Guid>
    {
        public User User { get; set; }
        /// <summary>
        /// یوزر سازنده
        /// </summary>
        public Guid? UserId { get; set; }
        /// <summary>
        ///     عنوان ملک
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// منطقه
        /// </summary>
        public string Zone { get; set; }

        /// <summary>
        /// ادرس
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// لوکیشن
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// نوع ملک (مثلاً مسکونی، تجاری، ویلایی و ...)
        /// </summary>
        public RealEstateOperationType PropertyType { get; set; }

        /// <summary>
        /// نوع معامله
        /// </summary>
        public RealEstateOperationType? RealEstateOperationType { get; set; }
        /// <summary>
        /// سال ساخت
        /// </summary>
        public int ConstructionYear { get; set; }
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
        /// طبقه ی واحد
        /// </summary>
        public int FloorNumber { get; set; }

        /// <summary>
        /// نورگیر
        /// </summary>
        public List<NaturalLightType> NaturalLight { get; set; } = new();
        public List<Notification> Notifications { get; set; }


        /// <summary>
        /// قیمت هر متر
        /// </summary>

        public decimal PricePerSquareMeter { get; set; }

        /// <summary>
        /// قیمت پیشبینی شده
        /// </summary>
        public decimal PriceingWithAi{ get; set; }
            
        /// <summary>
        /// نوع سند
        /// </summary>
        public DocumentType DocumentType { get; set; }


        /// <summary>
        /// انباری دارد
        /// </summary>
        public bool HasStorage { get; set; }

        /// <summary>
        /// تراس دارد
        /// </summary>
        public bool HasTerrace { get; set; }

        /// <summary>
        /// پارکینگ دارد
        /// </summary>
        public bool HasParking { get; set; }

        /// <summary>
        /// آسانسور دارد
        /// </summary>
        public bool HasElevator { get; set; }

        /// <summary>
        /// سونا
        /// </summary>
        public bool HasSauna { get; set; }
        /// <summary>
        /// جکوزی
        /// </summary>
        public bool HasJacuzzi { get; set; }
        /// <summary>
        /// روف گاردن 
        /// </summary>
        public bool HasRoofGarden { get; set; }
        /// <summary>
        /// استخر
        /// </summary>
        public bool HasPool { get; set; }

        /// <summary>
        /// لابی
        /// </summary>
        public bool HasLobby { get; set; }

        /// <summary>
        /// مشجر بودن (از -1 تا 1)
        /// </summary>
        public float GreeneryLevel { get; set; }

        /// <summary>
        /// عرض معبر (بر حسب متر)
        /// </summary>
        public float PassageWidth { get; set; }

        /// <summary>
        /// بافت (قدیمی/جدید) - true = جدید، false = قدیمی
        /// </summary>
        public bool IsModernTexture { get; set; }


        /// <summary>
        /// محله
        /// </summary>
        public string Neighborhood { get; set; }

        public static SmartRealEstatePricing Create(
        string nameNeighborhood,
        string address,
        string location,
        RealEstateOperationType operationType,
        string title,
        int constructionYear,
        int totalFloors,
        int unitsPerFloor,
        double area,
        int bathroomCount,
        int roomCount,
        List<NaturalLightType> naturalLight,
        DocumentType docType,
        bool hasStorage,
        bool hasTerrace,
        bool hasParking,
        bool hasElevator,
        bool hasSauna,
        bool hasJacuzzi,
        bool hasRoofGarden,
        bool hasPool,
        bool hasLobby,
        float greeneryLevel,
        float passageWidth,
        bool isModernTexture,
        RealEstateOperationType propertyType,
        string neighborhood
    )
        {
            return new SmartRealEstatePricing
            {
                Id = Guid.NewGuid(),
                Zone = nameNeighborhood,
                Address = address,
                Location = location,
                RealEstateOperationType = operationType,
                Title = title,
                ConstructionYear = constructionYear,
                TotalFloors = totalFloors,
                UnitsPerFloor = unitsPerFloor,
                Area = area,
                BathroomCount = bathroomCount,
                RoomCount = roomCount,
                NaturalLight = naturalLight,
                DocumentType = docType,
                HasStorage = hasStorage,
                HasTerrace = hasTerrace,
                HasParking = hasParking,
                HasElevator = hasElevator,
                HasSauna = hasSauna,
                HasJacuzzi = hasJacuzzi,
                HasRoofGarden = hasRoofGarden,
                HasPool = hasPool,
                HasLobby = hasLobby,
                GreeneryLevel = greeneryLevel,
                PassageWidth = passageWidth,
                IsModernTexture = isModernTexture,
                PropertyType = propertyType,
                Neighborhood = neighborhood
            };
        }

        public void SetUserId(Guid userId)
        {
            UserId = userId;
        }

    }
}
