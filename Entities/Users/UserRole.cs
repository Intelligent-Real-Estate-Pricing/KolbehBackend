using Entities.Common;
using Microsoft.AspNetCore.Identity;

namespace Entities.Users
{
    public class UserRole : IdentityUserRole<Guid>, IEntity
    {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
