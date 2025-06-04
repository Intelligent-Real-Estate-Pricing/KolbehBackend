using Entities.Medias;
using Microsoft.AspNetCore.Http;

namespace Application.Uploaders.DTOs;
public class UploadWithoutObjectIdDTO
{
    public IFormFile File { get; set; }
    public MediaTypes Type { get; set; }
    public bool WithWatermark { get; set; }
}
public class UploadCommandDTO
{
    public IFormFile File { get; set; }
    public long ObjectId { get; set; }
    public MediaTypes Type { get; set; }
    public bool WithWatermark { get; set; }
}
public class UploadCkEditorResult
{
    public string Link { get; set; }
}