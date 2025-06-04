using System.ComponentModel.DataAnnotations;

namespace Entities.Estates.Enums
{
    public enum RealEstateOperationType
    {
        [Display(Name = "خرید")]
        Purchase,

        [Display(Name = "فروش")]
        Sale,

        [Display(Name = "رهن و اجاره دادن")]
        Leasehold,

        [Display(Name = "رهن و اجاره گرفتن")]
        RentToOwn
    }
}
