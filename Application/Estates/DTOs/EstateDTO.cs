using Entities.Estates.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Estates.DTOs
{
    public class EstateDTO
    {

        public string UserName { get; set; }
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
        public RealEstateOperationType? RealEstateOperationType { get; set; }
        /// <summary>
        ///     عنوان ملک
        /// </summary>
        public string Title { get; set; }

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
        /// نورگیر
        /// </summary>
        public List<NaturalLightType> NaturalLight { get; set; } = new();

        /// <summary>
        /// قیمت هر متر
        /// </summary>

        public decimal PricePerSquareMeter { get; set; }
        public decimal PriceingWithAi { get; set; }

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
        /// نوع ملک (مثلاً مسکونی، تجاری، ویلایی و ...)
        /// </summary>
        public RealEstateOperationType PropertyType { get; set; }

        /// <summary>
        /// محله
        /// </summary>
        public string Neighborhood { get; set; }
    }
}
