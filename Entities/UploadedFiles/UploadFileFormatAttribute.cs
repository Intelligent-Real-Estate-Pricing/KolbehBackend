namespace Entities.UploadedFiles;

public class UploadFileFormatAttribute : Attribute
{
    public string Formats { get; set; }
    public bool Optimize { get; set; }
    public bool WithWatermark { get; set; }
    public string SaveToFolder { get; set; }
    public bool IsSecure { get; set; }
    public string SizeLimitation { get; set; }
    public bool HasDifferentSize { get; set; }
    public string ResizeFolder { get; set; }
    public bool IsAccessForOtherPerson { get; set; } = false;

}