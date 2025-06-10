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
    public class EstateConfiguration : IEntityTypeConfiguration<Estate>
    {
        public void Configure(EntityTypeBuilder<Estate> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(a => a.User).WithMany(a => a.Estates).HasForeignKey(a => a.UserId);
            builder.HasQueryFilter(x => !x.IsDeleted);

        }
    }
}
