using Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntitiesConfiguration.ValidationCodes;
public class ValidationCodeConfiguration : IEntityTypeConfiguration<ValidationCode>
{
    public void Configure(EntityTypeBuilder<ValidationCode> builder)
    {
        //Name And Schem
        builder.ToTable(nameof(ValidationCode), nameof(ValidationCode));

        //Identity
        builder.HasKey(x => x.Id);
    }
}