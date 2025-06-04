using Entities.Users;
using System.Linq.Expressions;

namespace Kolbeh.Api.Models.Users.Extentions
{
    internal static class UserRepositoryExtentions
    {

        public static IQueryable<UserDTO> SelectUsers(
     this IQueryable<User> input,
     bool includeSignUpInfo = false)
        {
            return includeSignUpInfo
            ? input.Select(SelectUsersEx())
            : input.Select(x => new UserDTO());
        }
        public static Expression<Func<User, UserDTO>> SelectUsersEx() =>
           t => UserDTO.Create(
               t.Id,
               t.FullName,
               t.IsActive,
               t.PhoneNumber,
               t.Email,
               t.EmailConfirmed,
               t.UserRoles.Select(tt => RoleDTO.Create(tt.Role.Name, tt.Role.Description)).ToArray(),
               t.UserName
           );




    }
}
