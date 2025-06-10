using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Estates.Enums
{
    public enum RealEstateType
    {
        [Display(Name = "مسکونی")]
        Residential,

        [Display(Name = "تجاری")]
        Commercial,

        [Display(Name = "ویلایی")]
        Villa,

        [Display(Name = "اداری")]
        Office,

        [Display(Name = "زمین")]
        Land
    }

}
