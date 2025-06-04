namespace Common.Utilities;

public static class FileExtentions
{
    public static string GetUploadFileDirectory(this string folder)
    {
        string result = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadImages");
        if (!Directory.Exists(result))
        {
            Directory.CreateDirectory(result);
        }
        if (!string.IsNullOrEmpty(folder))
            result = Path.Combine(result, folder);

        return result;
    }
}
