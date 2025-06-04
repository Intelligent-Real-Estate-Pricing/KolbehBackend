using System.ComponentModel.DataAnnotations;

namespace Entities.Estates.Enums
{
    public enum NaturalLightType
    {
        [Display(Name = "بدون نورگیر")]
        None,
        [Display(Name = "نورگیر شمالی")]
        North,
        [Display(Name = "نورگیر جنوبی")]
        South,
        [Display(Name = "نورگیر شرقی")]
        East,
        [Display(Name = "نورگیر غربی")]
        West
    }
}
