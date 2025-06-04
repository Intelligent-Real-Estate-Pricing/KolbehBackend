

using Entities.UploadedFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntitiesConfiguration.OtherPeopleAccessUploadedFiles
{
    public class OtherPeopleAccessUploadedFileConfiguration : IEntityTypeConfiguration<OtherPeopleAccessUploadedFile>
    {
        public void Configure(EntityTypeBuilder<OtherPeopleAccessUploadedFile> builder)
        {
            //Name And Schem
            builder.ToTable(nameof(OtherPeopleAccessUploadedFile), nameof(UploadedFile));

            //Identity
            builder.HasKey(x => x.Id);


            builder.HasOne(x => x.User).WithMany(x => x.OtherPeopleAccessUploadedFiles).HasForeignKey(x => x.UserId);
            builder.HasOne(x => x.UploadedFile).WithMany(x => x.OtherPeopleAccessUploadedFiles).HasForeignKey(x => x.UploadedFileId);
        }
    }
}
