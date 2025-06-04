using Microsoft.AspNetCore.Mvc;
using Services.Uploader.DTOs;

namespace Services.Uploader;

public interface IUploaderService
{
    Task<ServiceResult<UploadResultDTO>> UploadAsWebpV2(UploadV2DTO input);
    Task<IActionResult> GetBy(string guid, Guid userId);
}
