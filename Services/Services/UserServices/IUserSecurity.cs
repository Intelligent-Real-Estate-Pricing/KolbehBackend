using Common;
using Common.Utilities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Services.Services.UserServices
{
    public interface IUserSecurity
    {
        Guid? UserId { get; }
        public bool HasRole(string roleName);
        public List<string> GetUserRoles();
    }
    public class UserSecurity : IUserSecurity, IScopedDependency
    {
        private readonly IHttpContextAccessor _accessor;
        public UserSecurity(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public Guid? UserId
        {
            get
            {


                var userIdString = _accessor?.HttpContext?.User.Identity.GetUserId();
                var identity = Guid.Parse(userIdString);
                if (identity != null)
                    return identity;

                return null;
            }
        }

        public bool HasRole(string roleName)
        {
            return _accessor.HttpContext?.User.IsInRole(roleName) ?? false;
        }
        public List<string> GetUserRoles()
        {
            var user = _accessor.HttpContext?.User;
            if (user == null)
                return new List<string>();

            return user.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();
        }
    }
}
