using Entities.Estates;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntitiesConfiguration.BuildParticipations
{
    public class BuildParticipationConfiguration : IEntityTypeConfiguration<BuildParticipation>
    {
        public void Configure(EntityTypeBuilder<BuildParticipation> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasQueryFilter(x => !x.IsDeleted);

        }
    }
    }
