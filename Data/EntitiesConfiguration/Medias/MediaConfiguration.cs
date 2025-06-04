using Entities.Medias;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntitiesConfiguration.Medias;

public class MediaConfiguration : IEntityTypeConfiguration<Media>
{
    public void Configure(EntityTypeBuilder<Media> builder)
    {
        //Name And Schem
        builder.ToTable(nameof(Media), nameof(Media));

        //Identity
        builder.HasKey(x => x.Id);

        //Query Filter
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}