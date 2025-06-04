using Ardalis.SmartEnum;


namespace Common.Roles
{
    public class UserRoleNames
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }

    public class UserRoles(string name, string value) : SmartEnum<UserRoles, string>(name, value)
    {
        public static UserRoles Admin = new(UserRoleNames.Admin, UserRoleNames.Admin) { PersianName = "مدیر کل" };
        public static UserRoles User = new(UserRoleNames.User, UserRoleNames.User) { PersianName = "کاربر عادی" };

        public string PersianName { get; set; }
    }
}
