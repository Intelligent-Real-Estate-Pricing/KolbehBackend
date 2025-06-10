using Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntitiesConfiguration.Users
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //prop
            builder.Property(p => p.UserName).IsRequired().HasMaxLength(100);
            //builder.Property(x => x.FullName).IsRequired().HasMaxLength(100);

            //Navigations
            builder.HasMany(x => x.OtherPeopleAccessUploadedFiles).WithOne(x => x.User).HasForeignKey(x => x.UserId);
            builder.HasMany(x => x.Estates).WithOne(x => x.User).HasForeignKey(x => x.UserId);
            builder.HasMany(a => a.Notifications).WithOne(a => a.RecipientUser).HasForeignKey(a => a.RecipientUserId);

            //QueryFilter
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
