using Entities.Common;
using Entities.Estates.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RealEstateInfoFiles
{
    public enum DocumentType
    {
        [Display(Name = "سند شخصی")]
        Personal,

        [Display(Name = "سند مسکونی")]
        Residential,

        [Display(Name = "سند وقفی")]
        Endowment,

        [Display(Name = "سند تعهدی")]
        Promissory,

        [Display(Name = "سند اداری")]
        Administrative,

        [Display(Name = "سند تجاری")]
        Commercial,

        [Display(Name = "سند راهنما و وقف")]
        GuidanceEndowment,

        [Display(Name = "سند تعاونی")]
        Cooperative,

        [Display(Name = "سند نامشخص")]
        Unknown
    }


    public enum FlooringType
    {
        [Display(Name = "سنگ")]
        Stone,

        [Display(Name = "سرامیک")]
        Ceramic,

        [Display(Name = "پارکت")]
        Parquet,

        [Display(Name = "لمینت")]
        Laminate,

        [Display(Name = "سرامیک پارکت")]
        CeramicParquet,

        [Display(Name = "سنگ پارکت")]
        StoneParquet,

        [Display(Name = "فرش")]
        Carpet,

        [Display(Name = "موزاییک")]
        Mosaic,

        [Display(Name = "موزاییک سنگی")]
        MosaicStone,

        [Display(Name = "چوبی")]
        Wooden,

        [Display(Name = "پی وی سی")]
        PVC,

        [Display(Name = "نامشخص")]
        Unknown
    }


    public enum KitchenType
    {
        [Display(Name = "ام دی اف")]
        MDF,

        [Display(Name = "پنتری")]
        Pantry,

        [Display(Name = "ممبران")]
        Membrane,

        [Display(Name = "های گلاس")]
        HighGloss,

        [Display(Name = "های کلاس")]
        HighClass,

        [Display(Name = "کاستوم")]
        Custom,

        [Display(Name = "فارنیشد")]
        Furnished,

        [Display(Name = "نئو کلاسیک")]
        NeoClassic,

        [Display(Name = "چوبی")]
        Wooden,

        [Display(Name = "کلاسیک")]
        Classic,

        [Display(Name = "نامشخص")]
        Unknown,

        [Display(Name = "PVC آلمانی")]
        GermanPVC,

        [Display(Name = "بدون مشخصات")]
        None,

        [Display(Name = "پولیش شده")]
        Polished,

        [Display(Name = "متالیک")]
        Metallic,

        [Display(Name = "چوب و فلز")]
        WoodMetal,

        [Display(Name = "مدرن")]
        Modern
    }


    public enum BathroomType
    {
        [Display(Name = "سرویس بهداشتی فرنگی")]
        Western,

        [Display(Name = "سرویس بهداشتی ایرانی")]
        Persian,

        [Display(Name = "سرویس بهداشتی فرنگی و ایرانی")]
        PersianWestern,

        [Display(Name = "سرویس بهداشتی عمومی")]
        Public,

        [Display(Name = "سرویس بهداشتی اختصاصی")]
        Private
    }


    public enum FacadeType
    {
        [Display(Name = "نامشخص")]
        Unknown,

        [Display(Name = "سنگ")]
        Stone,

        [Display(Name = "سنگ مدرن")]
        ModernStone,

        [Display(Name = "سنگ رومی")]
        RomanStone,

        [Display(Name = "رومی")]
        Roman,

        [Display(Name = "کلاسیک")]
        Classic,

        [Display(Name = "آجر")]
        Brick,

        [Display(Name = "آجر ۳ سانتی")]
        ///معادل 3cm Brick در اکسل
        ThreeCmBrick,

        [Display(Name = "سنگ و آجر")]
        StoneAndBrick,

        [Display(Name = "سنگ و شیشه")]
        StoneAndGlass,

        [Display(Name = "سیمان")]
        Cement,

        [Display(Name = "سیمان سنگ")]
        CementStone,

        [Display(Name = "شیشه")]
        Glass,

        [Display(Name = "سرامیک")]
        Ceramic,

        [Display(Name = "چوب ترمو")]
        ThermoWood,

        [Display(Name = "گرانیت")]
        Granite,

        [Display(Name = "کامپوزیت")]
        Composite,

        [Display(Name = "پنل کامپوزیت")]
        CompositePanel
    }

    public class RealEstateInfoFile :BaseEntity<Guid>
    {
        /// <summary>
        /// منطقه
        /// </summary>
        public string Mantaghe { get; set; }

        /// <summary>
        /// تعداد کل طبقات واحد
        /// </summary>
        public int TotalFloors { get; set; }

        /// <summary>
        /// تعداد واحد در طبقه
        /// </summary>
        public int UnitsPerFloor { get; set; }

        /// <summary>
        /// مبلغ کل (ریال)
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// قیمت هر متر
        /// </summary>
        public decimal PricePerSquareMeter { get; set; }

        /// <summary>
        /// متراژ (متر مربع)
        /// </summary>
        public int SquareMeters { get; set; }

        /// <summary>
        /// تعداد طبقات
        /// </summary>
        public int Floors { get; set; }

        /// <summary>
        /// زیر بنا (متر مربع)
        /// </summary>
        public int BuiltArea { get; set; }

        /// <summary>
        /// تعداد خواب‌ها
        /// </summary>
        public int Bedrooms { get; set; }

        /// <summary>
        /// تعداد بالکن‌ها
        /// </summary>
        public int Balconies { get; set; }

        /// <summary>
        /// تلفن دارد
        /// </summary>
        public bool HasTelephone { get; set; }

        /// <summary>
        /// نوع آشپزخانه
        /// </summary>
        public KitchenType KitchenType { get; set; }

        /// <summary>
        /// باز یا بسته بودن آشپزخانه
        /// </summary>
        public bool IsKitchenOpen { get; set; }

        /// <summary>
        /// نوع بهداشتی
        /// </summary>
        public BathroomType BathroomType { get; set; }

        /// <summary>
        /// نوع کف‌پوش
        /// </summary>
        public FlooringType FlooringType { get; set; }

        /// <summary>
        /// آسانسور دارد
        /// </summary>
        public bool HasElevator { get; set; }

        /// <summary>
        /// نوع نما
        /// </summary>
        public FacadeType FacadeType { get; set; }

        /// <summary>
        /// نوع سند
        /// </summary>
        public DocumentType DocumentType { get; set; }

        /// <summary>
        /// پارکینگ دارد
        /// </summary>
        public bool HasParking { get; set; }

        /// <summary>
        /// انباری دارد
        /// </summary>
        public bool HasStorage { get; set; }

        /// <summary>
        /// عمر بنا
        /// </summary>
        public string BuildingAge { get; set; }

        /// <summary>
        /// مبله است
        /// </summary>
        public bool IsFurnished { get; set; }

        /// <summary>
        /// گاز دارد
        /// </summary>
        public bool HasGas { get; set; }

        /// <summary>
        /// آیفون تصویری دارد
        /// </summary>
        public bool HasVideoIntercom { get; set; }

        /// <summary>
        /// کولر دارد
        /// </summary>
        public bool HasCooler { get; set; }

        /// <summary>
        /// درب ریموت کنترل دارد
        /// </summary>
        public bool HasRemoteControlDoor { get; set; }

        /// <summary>
        /// شوفاژ دارد
        /// </summary>
        public bool HasRadiator { get; set; }

        /// <summary>
        /// پکیج دارد
        /// </summary>
        public bool HasPackageHeater { get; set; }

        /// <summary>
        /// بازسازی شده است
        /// </summary>
        public bool IsRenovated { get; set; }

        /// <summary>
        /// جکوزی دارد
        /// </summary>
        public bool HasJacuzzi { get; set; }

        /// <summary>
        /// سونا دارد
        /// </summary>
        public bool HasSauna { get; set; }

        /// <summary>
        /// استخر دارد
        /// </summary>
        public bool HasPool { get; set; }

        /// <summary>
        /// لابی دارد
        /// </summary>
        public bool HasLobby { get; set; }

        /// <summary>
        /// داکت اسپلیت دارد
        /// </summary>
        public bool HasDuctSplit { get; set; }

        /// <summary>
        /// چیلر دارد
        /// </summary>
        public bool HasChiller { get; set; }

        /// <summary>
        /// روف گاردن دارد
        /// </summary>
        public bool HasRoofGarden { get; set; }

        /// <summary>
        /// مستر روم دارد
        /// </summary>
        public bool HasMasterRoom { get; set; }

        /// <summary>
        /// فاقد آسانسور است
        /// </summary>
        public bool HasNoElevator { get; set; }

        /// <summary>
        /// اسپلیت دارد
        /// </summary>
        public bool HasSplitAC { get; set; }

        /// <summary>
        /// سرویس مستخدم - سرایدار دارد
        /// </summary>
        public bool HasJanitorService { get; set; }

        /// <summary>
        /// سالن اجتماعات دارد
        /// </summary>
        public bool HasMeetingHall { get; set; }

        /// <summary>
        /// فن کوئل دارد
        /// </summary>
        public bool HasFanCoil { get; set; }

        /// <summary>
        /// سالن بدنسازی دارد
        /// </summary>
        public bool HasGym { get; set; }

        /// <summary>
        /// دوربین مداربسته دارد
        /// </summary>
        public bool HasCCTV { get; set; }

        /// <summary>
        /// برق اضطراری دارد
        /// </summary>
        public bool HasEmergencyPower { get; set; }

        /// <summary>
        /// فلت است
        /// </summary>
        public bool IsFlat { get; set; }

        /// <summary>
        /// گرمایشی از کف دارد
        /// </summary>
        public bool HasUnderfloorHeating { get; set; }

        /// <summary>
        /// اعلام حریق دارد
        /// </summary>
        public bool HasFireAlarm { get; set; }

        /// <summary>
        /// اطفاء حریق دارد
        /// </summary>
        public bool HasFireExtinguishingSystem { get; set; }

        /// <summary>
        /// جاروبرقی مرکزی دارد
        /// </summary>
        public bool HasCentralVacuum { get; set; }

        /// <summary>
        /// درب ضد سرقت دارد
        /// </summary>
        public bool HasSecurityDoor { get; set; }

        /// <summary>
        /// لندری دارد
        /// </summary>
        public bool HasLaundry { get; set; }

        /// <summary>
        /// فروش با مستاجر است
        /// </summary>
        public bool IsSoldWithTenant { get; set; }

        /// <summary>
        /// آنتن مرکزی دارد
        /// </summary>
        public bool HasCentralAntenna { get; set; }

        /// <summary>
        /// حیاط خلوت دارد
        /// </summary>
        public bool HasBackyard { get; set; }

        /// <summary>
        /// سرویس مستخدم دارد
        /// </summary>
        public bool HasServantService { get; set; }

        /// <summary>
        /// باربیکیو دارد
        /// </summary>
        public bool HasBarbecue { get; set; }

        /// <summary>
        /// تابلو خور دارد
        /// </summary>
        public bool HasElectricalPanel { get; set; }

        /// <summary>
        /// سالن کنفرانس دارد
        /// </summary>
        public bool HasConferenceHall { get; set; }

        /// <summary>
        /// سرویس مستخدم - نگهبان دارد
        /// </summary>
        public bool HasGuardService { get; set; }

        /// <summary>
        /// هواساز دارد
        /// </summary>
        public bool HasAirHandler { get; set; }

        /// <summary>
        /// ماهواره مرکزی دارد
        /// </summary>
        public bool HasCentralSatellite { get; set; }

        /// <summary>
        /// شوتینگ زباله دارد
        /// </summary>
        public bool HasGarbageChute { get; set; }

        /// <summary>
        /// سرویس مستخدم - لابی من دارد
        /// </summary>
        public bool HasLobbyManService { get; set; }

        /// <summary>
        /// سرویس مستخدم - نگهبان/سرایدار دارد
        /// </summary>
        public bool HasGuardOrJanitorService { get; set; }
    }

}

