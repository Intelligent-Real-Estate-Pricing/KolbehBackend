using Entities.Estates;
using Entities.UploadedFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntitiesConfiguration.Estates
{
    public class EstateConfiguration : IEntityTypeConfiguration<SmartRealEstatePricing>
    {
        public void Configure(EntityTypeBuilder<SmartRealEstatePricing> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(a => a.User).WithMany(a => a.Estates).HasForeignKey(a => a.UserId);
            builder.HasMany(a => a.Notifications).WithOne(a => a.RelatedEstate).HasForeignKey(a => a.RelatedEstateId);
            builder.HasQueryFilter(x => !x.IsDeleted);

        }
    }
}
