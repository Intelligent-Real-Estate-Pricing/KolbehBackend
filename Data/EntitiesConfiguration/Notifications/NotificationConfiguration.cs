using Entities.Notifications;
using Entities.UploadedFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntitiesConfiguration.Notifications
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(a => a.RecipientUser).WithMany(a => a.Notifications).HasForeignKey(a => a.RecipientUserId);
            builder.HasOne(a => a.RelatedEstate).WithMany(a => a.Notifications).HasForeignKey(a => a.RelatedEstateId);
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
