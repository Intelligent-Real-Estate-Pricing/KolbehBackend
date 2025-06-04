using Common.Roles;
using Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace Services.DataInitializer
{
    public class RoleDataInitializer(RoleManager<Role> roleManager) : IDataInitializer
    {
        private readonly RoleManager<Role> _roleManager = roleManager;

        public void InitializeData()
        {
            if (_roleManager.Roles.Count() == UserRoles.List.Count)
                return;

            foreach (var item in UserRoles.List)
            {

                if (!_roleManager.RoleExistsAsync(item.Name).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new Role
                    {
                        Name = item.Name,
                        NormalizedName = item.Name.ToUpper(),
                        Description = item.PersianName,
                    }).GetAwaiter().GetResult();
                }
            }

        }
    }
}
