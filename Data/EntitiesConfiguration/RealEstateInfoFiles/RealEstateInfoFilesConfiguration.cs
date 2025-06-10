using Entities.RealEstateInfoFiles;
using Entities.UploadedFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntitiesConfiguration.RealEstateInfoFiles
{
    internal class RealEstateInfoFilesConfiguration : IEntityTypeConfiguration<RealEstateInfoFile>
    {
        public void Configure(EntityTypeBuilder<RealEstateInfoFile> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasQueryFilter(x => !x.IsDeleted);

        }
    }
}
