using Common;
using Common.Utilities;
using Data.Contracts;
using Entities.UploadedFiles;
using Entities.Users;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Services.Uploader.DTOs;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace Services.Uploader;

/// <summary>
/// سرویس آپلود فایل
/// </summary>
/// <param name="uploadedFileRepository"></param>
/// <param name="userRepository"></param>
/// <param name="userManager"></param>
/// <param name="webHost"></param>
/// <param name="configuration"></param>
/// <param name="httpContextAccessor"></param>
/// <param name="logger"></param>
public class UploaderService(IRepository<UploadedFile> uploadedFileRepository, IRepository<OtherPeopleAccessUploadedFile> OtherPeopleAccessUploadedFileRepository, IRepository<User> userRepository, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, ILogger<UploaderService> logger) : IScopedDependency, IUploaderService
{


    public async Task<ServiceResult<UploadResultDTO>> UploadAsWebpV2(UploadV2DTO input)
    {
        try
        {
            var userId = httpContextAccessor.HttpContext.User.Identity.GetUserId<string>();
            if (userId == null)
                return ServiceResult.BadRequest<UploadResultDTO>("لطفا ابتدا لاگین کنید");

            var configAttr = input.FileType.GetAttribute<UploadFileFormatAttribute>();
            var validFormats = configAttr.Formats.Split(',').Select(f => f.ToLower()).ToList();
            var sizeLimitation = configAttr.SizeLimitation;
            var needOptimize = configAttr.Optimize;
            var withWatermark = configAttr.WithWatermark;
            var saveToFolder = configAttr.SaveToFolder;
            var resizeFolder = configAttr.ResizeFolder;
            var isSecure = configAttr.IsSecure;
            var hasDifferentSize = configAttr.HasDifferentSize;
            var isAccessForOtherPerson = configAttr.IsAccessForOtherPerson;

            if (isAccessForOtherPerson && (input.OtherUsersId == null || input.OtherUsersId.Count == 0))
                return ServiceResult.BadRequest<UploadResultDTO>("لیست افراد دیگر خالی است");

            if (input.File.Length > 20 * 1024 * 1024)
                return ServiceResult.BadRequest<UploadResultDTO>("حجم فایل بیش از حد مجاز است");

            var fileExt = Path.GetExtension(input.File.FileName).ToLower();
            if (!validFormats.Contains(fileExt))
                return ServiceResult.BadRequest<UploadResultDTO>($"فرمت فایل مجاز نیست. فرمت‌های مجاز: {configAttr.Formats}");

            // بررسی نوع واقعی فایل با ImageSharp
            Image image;
            try
            {
                var decoderOptions = new DecoderOptions { SkipMetadata = true };
                image = Image.Load(decoderOptions, input.File.OpenReadStream());
            }
            catch (UnknownImageFormatException)
            {
                return ServiceResult.BadRequest<UploadResultDTO>("نوع فایل نامعتبر است");
            }

            var rootPath = Directory.GetCurrentDirectory();
            var saveDirectory = Path.Combine(rootPath, "wwwroot", saveToFolder.GetUploadFileDirectory());

            // بررسی امنیت مسیر
            if (!Path.GetFullPath(saveDirectory).StartsWith(Path.Combine(rootPath, "wwwroot")))
                return ServiceResult.Fail<UploadResultDTO>(null, "مسیر ذخیره‌سازی امن نیست", ApiResultStatusCode.LogicError);

            Directory.CreateDirectory(saveDirectory);

            var imageGuid = Guid.NewGuid().ToString();
            var outputFileName = $"{imageGuid}.webp";
            var outputPath = Path.Combine(saveDirectory, outputFileName);

            // پردازش تصویر
            image.Metadata.ExifProfile = null;

            if (withWatermark)
            {
                var watermarkPath = Path.Combine(rootPath, "Media", "Gallery", "Watermarks", "default.png"); 
                if (File.Exists(watermarkPath))
                {
                    using var watermark = Image.Load(watermarkPath);
                    image.Mutate(x => x.DrawImage(watermark, new Point(image.Width - watermark.Width - 10, image.Height - watermark.Height - 10), 0.5f));
                }
            }

            await using var fs = new FileStream(outputPath, FileMode.Create);
            image.Save(fs, new WebpEncoder
            {
                Quality = 70,
                FileFormat = WebpFileFormatType.Lossy
            });

            // تغییر اندازه در صورت نیاز
            if (hasDifferentSize && fileExt == ".gif")
            {
                var resizeDir = Path.Combine(saveDirectory, resizeFolder);
                Directory.CreateDirectory(resizeDir);
                var sizes = new (int Width, int Height)[] { (1272, 128), (636, 128), (250, 80) };
                await ResizeImageAsync(image, resizeDir, imageGuid, ".webp", sizes);
            }

            // نوع فایل
            new FileExtensionContentTypeProvider().TryGetContentType(outputPath, out string fileContentType);
            fileContentType ??= "application/octet-stream";

            // ساخت مدل فایل
            var fileUrl = $"{(httpContextAccessor.HttpContext.Request.IsHttps ? "https" : "http")}://{httpContextAccessor.HttpContext.Request.Host}/api/v1/Uploaders/Get/{imageGuid}";

            var fileModel = UploadedFile.Create(
                Guid.Parse(userId),
                fileUrl,
                outputPath,
                input.FileType,
                input.Title,
                input.Alt,
                input.Description,
                fileContentType,
                new FileInfo(outputPath).Length,
                imageGuid,
                isSecure,
                isAccessForOtherPerson
            );

            await uploadedFileRepository.AddAsync(fileModel, CancellationToken.None);

            if (isAccessForOtherPerson)
            {
                await OtherPeopleAccessUploadedFileRepository.AddRangeAsync(
                    input.OtherUsersId.Select(id => new OtherPeopleAccessUploadedFile
                    {
                        UploadedFileId = fileModel.Id,
                        UserId = id
                    }).ToList(), CancellationToken.None
                );
            }

            return ServiceResult.Ok(new UploadResultDTO { Url = fileUrl });
        }
        catch (Exception ex)
        {
            return ServiceResult.Fail<UploadResultDTO>(null, "خطا در آپلود فایل: " + ex.Message, ApiResultStatusCode.ServerError);
        }
    }

    public async Task ResizeImageAsync(Image image, string imagePath, string imageGuid, string fileExtention, params (int Width, int Height)[] sizes)
    {
        List<string> outPuts = new();
        foreach (var size in sizes)
        {
            using (Image resizedImage = image.Clone(ctx => ctx.Resize(size.Width, size.Height)))
            {
                string imageResult = imageGuid + "_" + $"size_{size.Width}x{size.Height}{fileExtention}";
                string outputImagePath = Path.Combine(imagePath, imageResult);
                await resizedImage.SaveAsync(outputImagePath, new GifEncoder());
            }
        }
    }


    public async Task<IActionResult> GetBy(string guid, Guid userId)
    {
        var uploadedFileInDb = await uploadedFileRepository.Table.Include(x => x.OtherPeopleAccessUploadedFiles).Where(t => t.Guid == guid).FirstOrDefaultAsync();

        var IsInOtherPeopleAccessUploadedFiles = true;
        if (uploadedFileInDb is null)
            throw new FileNotFoundException("FileNotFound");
        if (uploadedFileInDb.IsSecure)
        {
            var user = await userRepository.Table.FirstOrDefaultAsync(t => t.Id == userId);
            if (uploadedFileInDb.IsAccessForOtherPerson)
            {

                IsInOtherPeopleAccessUploadedFiles = uploadedFileInDb.OtherPeopleAccessUploadedFiles.Any(x => x.UserId == userId);
            }
            bool isAdmin = await userManager.IsInRoleAsync(user, "Admin");
            if (!isAdmin)
            {
                if (uploadedFileInDb.CreatedByUserId != userId && !IsInOtherPeopleAccessUploadedFiles)
                    throw new FileNotFoundException("FileNotFound");
            }
        }
        return new FileContentResult(await File.ReadAllBytesAsync(uploadedFileInDb.ImagePath), uploadedFileInDb.FileContent);
    }
}

