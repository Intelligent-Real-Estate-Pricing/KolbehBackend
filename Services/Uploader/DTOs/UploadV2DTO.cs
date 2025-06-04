using Entities.UploadedFiles;
using Microsoft.AspNetCore.Http;

namespace Services.Uploader.DTOs;

public class UploadV2DTO
{
    public IFormFile File { get; set; }
    public UploadType FileType { get; set; }
    public string Title { get; set; }
    public string Alt { get; set; }
    public string Description { get; set; }

    public List<Guid> OtherUsersId { get; set; }
}
