using System.ComponentModel.DataAnnotations;

namespace Entities.Estates.Enums
{
    public enum DocumentType
    {
        [Display(Name = "سند رسمی")]
        Official,
        [Display(Name = "سند عادی")]
        Normal,
        [Display(Name = "قرارداد ساخت")]
        ConstructionContract,
        [Display(Name = "سند وقفی")]
        Endowment
    }

}
