using Entities.Common;
using Entities.UploadedFiles;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Entities.Users
{
    public class User : IdentityUser<Guid>, IEntity<Guid>, ISoftDelete
    {
        public User()
        {
            IsActive = true;
        }
        public User(string phoneNumber, string email,string fullName) 
        {
            UserName = phoneNumber;
            FullName = fullName;
            PhoneNumber = phoneNumber;
            Email = email;
            IsActive = true;
        }


        /// <summary>
        /// نام و نام خانوادگی
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// کدملی
        /// </summary>
        public string NationalCode { get; set; }

        public bool IsActive { get; set; }
        /// <summary>
        /// آخرین ورود
        /// </summary>
        public DateTimeOffset? LastLoginDate { get; set; }

        /// <summary>
        /// عکس کاربر
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// حذف کاربر
        /// </summary>
        public bool IsDeleted { get; set; }
  

        /// <summary>
        /// نقش های کاربر
        /// </summary>
        public ICollection<UserRole> UserRoles { get; set; }
        public List<OtherPeopleAccessUploadedFile> OtherPeopleAccessUploadedFiles { get; set; }



        //Factory Methods

        public static User RegisterUser(string phoneNumber, string nationalCode, string fullName)=>new (phoneNumber, nationalCode, fullName);   
        public void ActivityToggle() => IsActive = !IsActive;
    }


    public enum GenderType
    {
        [Display(Name = "مرد")]
        Male = 1,

        [Display(Name = "زن")]
        Female = 2
    }
}
