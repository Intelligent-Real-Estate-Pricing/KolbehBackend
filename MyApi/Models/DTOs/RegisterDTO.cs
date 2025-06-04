using Common.Roles;
using Common.Utilities;
using Entities.Users;
using Kolbeh.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace Kolbeh.Api.Models.DTOs;

public class RegisterDTO
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public UserType Type { get; set; }
}

public class RegisterUserDTO : IValidatableObject
{
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string NationalCode { get; set; }
    public string FullName { get; set; }
    public string validateCode { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {

        if (string.IsNullOrEmpty(FullName))
            yield return new ValidationResult("وارد کردن نام اجباریست. ", new[] { nameof(FullName) });


        if (FullName?.Length >= 30)
            yield return new ValidationResult("نام نباید بیشتر از 30 کارکتر باشد", new[] { nameof(FullName) });



        if (string.IsNullOrEmpty(PhoneNumber))
            yield return new ValidationResult("شماره تلفن اجباریست. ", new[] { nameof(PhoneNumber) });


        if (!PhoneNumber.IsMobile())
            yield return new ValidationResult("ساختار شماره تلفن وارده اشتباه است", new[] { nameof(PhoneNumber) });


        if (string.IsNullOrEmpty(Password))
            yield return new ValidationResult("رمز عبور اجباریست. ", new[] { nameof(Password) });


        if (string.IsNullOrEmpty(NationalCode))
            yield return new ValidationResult("کدملی اجباریست. ", new[] { nameof(NationalCode) });


        if (!NationalCode.IsValidNationalId())
            yield return new ValidationResult("ساختار کدملی وارده اشتباه است", new[] { nameof(NationalCode) });



    }
}
public class GetUserInfoDTO
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
    public string NationalCode { get; set; }
    public bool IsActive { get; set; }
    public string FullName { get; set; }
    public string ImageUrl { get; internal set; }
    public RoleDTO[] Roles { get; internal set; }
}
