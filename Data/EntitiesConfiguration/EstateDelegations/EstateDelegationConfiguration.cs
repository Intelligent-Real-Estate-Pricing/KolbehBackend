using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntitiesConfiguration.EstateDelegations
{
    public class EstateDelegationConfiguration : IEntityTypeConfiguration<EstateDelegation>
    {
        public void Configure(EntityTypeBuilder<EstateDelegation> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasQueryFilter(x => !x.IsDeleted);

        }
    }
}
