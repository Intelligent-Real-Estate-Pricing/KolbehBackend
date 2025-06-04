using Entities.UploadedFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntitiesConfiguration.UploadedFiles;

public class UploadedFileConfiguration : IEntityTypeConfiguration<UploadedFile>
{
    public void Configure(EntityTypeBuilder<UploadedFile> builder)
    {
        //Name And Schem
        builder.ToTable(nameof(UploadedFile), nameof(UploadedFile));

        //Identity
        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.OtherPeopleAccessUploadedFiles).WithOne(x => x.UploadedFile).HasForeignKey(x => x.UploadedFileId);

    }
}
