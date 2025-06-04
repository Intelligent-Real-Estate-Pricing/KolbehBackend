using Entities.Common;
using Entities.Users;

namespace Entities.UploadedFiles;

public class UploadedFile : BaseEntity<Guid>
{
    public UploadedFile()
    {

    }
    public UploadedFile(Guid userId, string imageUrl, string imagePath, UploadType fileType, string title, string alt, string description, string fileContentType, long length, string imageGuid, bool isSecure, bool isAccessForOtherPerson)
    {
        CreatedByUserId = userId;
        ImageUrl = imageUrl;
        ImagePath = imagePath;
        FileType = fileType;
        Title = title;
        Alt = alt;
        Description = description;
        FileContent = fileContentType;
        FileSize = length;
        Guid = imageGuid;
        CreateAt = DateTime.Now;
        IsSecure = isSecure;
        IsAccessForOtherPerson = isAccessForOtherPerson;
    }

    //FKs
    public Guid CreatedByUserId { get; set; }

    //Props
    public string Guid { get; set; }
    public string ImageUrl { get; set; }
    public string ImagePath { get; set; }
    public UploadType FileType { get; set; }
    public DateTime CreateAt { get; set; }
    public string Title { get; set; }
    public string Alt { get; set; }
    public string Description { get; set; }
    public string FileContent { get; set; }
    public long FileSize { get; set; }
    public bool IsSecure { get; set; }
    public bool IsAccessForOtherPerson { get; set; } = false;

    //Navigations
    public User CreatedByUser { get; set; }
    public List<OtherPeopleAccessUploadedFile> OtherPeopleAccessUploadedFiles { get; set; }


    public static UploadedFile Create(Guid userId, string imageUrl, string imagePath, UploadType fileType, string title, string alt, string description, string fileContentType, long length, string imageGuid, bool isSecure, bool isAccessForOtherPerson)
    {
        UploadedFile uploadedFile = new(userId, imageUrl, imagePath, fileType, title, alt, description, fileContentType, length, imageGuid, isSecure, isAccessForOtherPerson);
        return uploadedFile;
    }
}

public enum UploadType
{
    [UploadFileFormat(Formats = ".png,.jpg,.webp,.jpeg", Optimize = true, WithWatermark = false, SaveToFolder = "Shop", IsSecure = false)]
    estate = 1,
}

public enum SizeType
{
    Small,
    Medium,
    Large,
}
