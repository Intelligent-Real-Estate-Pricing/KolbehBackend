namespace Application.Users.DTOs;

public class AddRoleToUserDTO
{
    /// <summary>
    /// َناسه کاربر
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// نقش ها
    /// لیستی از رشته ها که نقش کاربر می باشد
    /// </summary>
    public List<string> Roles { get; set; }
}