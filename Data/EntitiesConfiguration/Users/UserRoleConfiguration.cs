using Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntitiesConfiguration.Users
{
    internal class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasOne(t => t.User).WithMany(t => t.UserRoles).HasForeignKey(t => t.UserId);
            builder.HasOne(t => t.Role).WithMany(t => t.UserRoles).HasForeignKey(t => t.RoleId);
        }
    }
}
