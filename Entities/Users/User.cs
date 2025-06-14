using Entities.Common;
using Entities.Estates;
using Entities.Notifications;
using Entities.UploadedFiles;
using Entities.Users;
using Microsoft.AspNetCore.Identity;

public class User : IdentityUser<Guid>, IEntity<Guid>, ISoftDelete
{
    public User()
    {
        IsActive = true;
        AuthenticationMethod = AuthenticationMethod.OTP;
    }

    public User(string phoneNumber, string fullName)
    {
        UserName = phoneNumber;
        FullName = fullName;
        PhoneNumber = phoneNumber;
        IsActive = true;
        AuthenticationMethod = AuthenticationMethod.OTP;
    }

    public string FullName { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset? LastLoginDate { get; set; }
    public string ImageUrl { get; set; }
    public bool IsDeleted { get; set; }

    // New property for authentication method
    public AuthenticationMethod AuthenticationMethod { get; set; }

    // Navigation properties
    public ICollection<UserRole> UserRoles { get; set; }
    public List<OtherPeopleAccessUploadedFile> OtherPeopleAccessUploadedFiles { get; set; }
    public List<SmartRealEstatePricing> Estates { get; set; }
    public List<Notification> Notifications { get; set; }

    // Factory Methods
    public static User RegisterUser(string phoneNumber, string fullName)
        => new(phoneNumber, fullName);

    public void ActivityToggle() => IsActive = !IsActive;

    // Domain methods for authentication
    public void EnablePasswordAuthentication()
    {
        AuthenticationMethod = AuthenticationMethod.Both;
    }

    public void DisablePasswordAuthentication()
    {
        AuthenticationMethod = AuthenticationMethod.OTP;
    }

    public bool CanAuthenticateWithPassword()
        => AuthenticationMethod == AuthenticationMethod.Password ||
           AuthenticationMethod == AuthenticationMethod.Both;

    public bool CanAuthenticateWithOTP()
        => AuthenticationMethod == AuthenticationMethod.OTP ||
           AuthenticationMethod == AuthenticationMethod.Both;
}

// Authentication Method Enum
public enum AuthenticationMethod
{
    OTP = 1,
    Password = 2,
    Both = 3
}
