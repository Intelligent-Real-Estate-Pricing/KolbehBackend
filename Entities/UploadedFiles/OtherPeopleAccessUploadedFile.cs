
using Entities.Common;
using Entities.Users;

namespace Entities.UploadedFiles
{
    public class OtherPeopleAccessUploadedFile : BaseEntity<Guid>
    {

        public Guid UploadedFileId { get; set; }
        public UploadedFile UploadedFile { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

    }
}
