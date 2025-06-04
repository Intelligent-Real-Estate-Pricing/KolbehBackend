using System.ComponentModel.DataAnnotations;

namespace Common.Roles;

public enum UserType
{
    /// <summary>
    /// ادمین
    /// </summary>
    [Display(Name = "ادمین")]
    Admin,
    /// <summary>
    /// کاربر
    /// </summary>
    [Display(Name = "کاربر")]
    User,
    /// <summary>
    /// آزمایشگاه
    /// </summary>
    [Display(Name = "آزمایشگاه")]
    Laboratoroperator,
    /// <summary>
    /// آزمایشگاه
    /// </summary>
    [Display(Name = "یوزر دارای شرایط خاص")]
    UserSpecialConditions,
    /// <summary>
    /// دکتر
    /// </summary>
    [Display(Name = "دکتر")]
    Doctor
}
