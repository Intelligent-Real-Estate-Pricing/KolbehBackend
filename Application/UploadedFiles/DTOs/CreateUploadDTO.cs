using Entities.UploadedFiles;
using Microsoft.AspNetCore.Http;

namespace Application.UploadedFiles.DTOs;

public class CreateUploadDTO
{
    public IFormFile File { get; set; }
    public UploadType FileType { get; set; }
    public string Title { get; set; }
    public string Alt { get; set; }
    public string Description { get; set; }
}
