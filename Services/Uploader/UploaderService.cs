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
public class UploaderService(IRepository<UploadedFile> uploadedFileRepository, IRepository<OtherPeopleAccessUploadedFile> OtherPeopleAccessUploadedFileRepository, IRepository<User> userRepository, UserManager<User> userManager, IWebHostEnvironment webHost, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<UploaderService> logger) : IScopedDependency, IUploaderService
{


    public async Task<ServiceResult<UploadResultDTO>> UploadAsWebpV2(UploadV2DTO input)
    {
        try
        {
            var userId = httpContextAccessor.HttpContext.User.Identity.GetUserId<string>();
            if (userId == null)
                return ServiceResult.BadRequest<UploadResultDTO>("لطفا ابتدا لاگین کنید");

            var wwwrootPath = Directory.GetCurrentDirectory();

            var configAttr = input.FileType.GetAttribute<UploadFileFormatAttribute>();
            var validFormats = configAttr.Formats.Split(',').ToList();
            var needOptimize = configAttr.Optimize;
            var withWatermark = configAttr.WithWatermark;
            var saveToFolder = configAttr.SaveToFolder;
            var resizeFolder = configAttr.ResizeFolder;
            var isSecure = configAttr.IsSecure;
            var hasDiffrentSize = configAttr.HasDifferentSize;
            var sizeLimitation = configAttr.SizeLimitation;
            var isAccessForOtherPerson = configAttr.IsAccessForOtherPerson;

            if (isAccessForOtherPerson && (input.OtherUsersId == null || input.OtherUsersId.Count == 0))
            {
                return ServiceResult.BadRequest<UploadResultDTO>($"لیست افراد دیگری که باید دسترسی داشته باشند رو وارد کن");

            }

            var fileName = Path.GetFileName(input.File.FileName);
            var fileExtention = Path.GetExtension(input.File.FileName);

            var saveDirectory = saveToFolder.GetUploadFileDirectory();

            if (!Directory.Exists(saveDirectory))
                Directory.CreateDirectory(saveDirectory);
            if (!validFormats.Contains(fileExtention.ToLower()))
                return ServiceResult.BadRequest<UploadResultDTO>($"فرمت ارسالی اشتبا میباشد لطفا از این فرمت ها استفاده نمایید: {configAttr.Formats}");

            string imageGuid = Guid.NewGuid().ToString();


            string imageResultWithExtentionDefault = imageGuid + "_" + Path.GetFileNameWithoutExtension(input.File.FileName) + fileExtention;
            string imageResultDefaultWithEnCoder = imageGuid + "_" + Path.GetFileNameWithoutExtension(input.File.FileName) + ".webp";

            string imagePath = Path.Combine(saveDirectory, imageResultWithExtentionDefault);
            if (needOptimize)
                imagePath = Path.Combine(saveDirectory, imageResultDefaultWithEnCoder);

            var imageUrl = (httpContextAccessor.HttpContext.Request.IsHttps ? "https://" : "http://") + httpContextAccessor.HttpContext.Request.Host + "/api/v1/Uploaders/Get/" + imageGuid;


            using (FileStream fs = new(imagePath, FileMode.Create))
            {
                //var image = SixLabors.ImageSharp.Image.Load(input.File.OpenReadStream());
                var decoderOptions = new SixLabors.ImageSharp.Formats.DecoderOptions
                {
                    SkipMetadata = true,
                };
                // Load the image with decoder options to ignore metadata (for any format)
                var image = Image.Load(decoderOptions, input.File.OpenReadStream());



                if (withWatermark)
                {
                    var waterMarkLoc = Path.Combine(wwwrootPath, "Media", "Gallery", "Watermarks", "");
                    if (File.Exists(waterMarkLoc))
                    {
                        var waterMark = Image.Load(waterMarkLoc);

                        image.Mutate(o =>
                        {
                            o.DrawImage(waterMark, new Point(image.Width - waterMark.Width - 10, image.Height - waterMark.Height - 10), 0.5f);
                        });
                    }
                }
                if (needOptimize)
                {

                    image.Save(fs, new WebpEncoder()
                    {
                        Quality = 70,
                        FileFormat = WebpFileFormatType.Lossy,
                    });

                }
                if (hasDiffrentSize && fileExtention == ".gif")
                {
                    var saveResizeDirectory = Path.Combine(saveDirectory, resizeFolder);
                    if (!Directory.Exists(saveResizeDirectory))
                        Directory.CreateDirectory(saveResizeDirectory);

                    var sizes = new (int Width, int Height)[]
                        {
                                (1272, 128),
                                (636, 128),
                                (250, 80)
                        };
                    image.Save(fs, new GifEncoder());

                    await ResizeImageAsync(image, saveResizeDirectory, imageGuid.ToString(), fileExtention, sizes);

                }


                new FileExtensionContentTypeProvider().TryGetContentType(imagePath, out string FileContentType);

                var FileModel = UploadedFile.Create(Guid.Parse(userId), imageUrl, imagePath, input.FileType, input.Title, input.Alt, input.Description, FileContentType, new FileInfo(imagePath).Length, imageGuid, isSecure, isAccessForOtherPerson);
                await uploadedFileRepository.AddAsync(FileModel, CancellationToken.None);
                if (isAccessForOtherPerson)
                {
                    await OtherPeopleAccessUploadedFileRepository.AddRangeAsync(
                          input.OtherUsersId.Select(id => new OtherPeopleAccessUploadedFile
                          {
                              UploadedFileId = FileModel.Id,
                              UserId = id
                          }).ToList(), CancellationToken.None
                            );

                }


            }

            return ServiceResult.Ok(new UploadResultDTO
            {
                Url = imageUrl
            });
        }
        catch (Exception ex)
        {
            return ServiceResult.Fail<UploadResultDTO>(null, ex.Message, ApiResultStatusCode.LogicError);
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

