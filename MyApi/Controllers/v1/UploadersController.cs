using Application.Cqrs.Commands;
using Application.Cqrs.Queris;
using Application.UploadedFiles.Command.UploadAsWebp;
using Application.UploadedFiles.Query.GetUploadedFiles;
using Asp.Versioning;
using Kolbeh.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Uploader.DTOs;
using WebFramework.Api;

namespace Kolbeh.Api.Controllers.v1;

/// <summary>
/// کنترلر آپلود
/// </summary>
/// <param name="commandDispatcher"></param>
/// <param name="queryDispatcher"></param>
[ApiVersion("1")]
public class UploadersController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher) : BaseController
{
    /// <summary>
    /// آپلود
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("[action]")]
    [Authorize]
    public async Task<ApiResult<UploadResultDTO>> Upload([FromForm] UploadV2DTO input) =>
     (await commandDispatcher.SendAsync(new UploadAsWebpCommand(input.File, input.FileType, input.Title, input.Alt, input.Description, input.OtherUsersId))).ToApiResult();
    /// <summary>
    /// اپلود برای استاتیک پیج
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("[action]")]
    [Authorize]
    [IgnoreApiResultFilter]
    /*   
        public async Task<string> UploaddForStaticPage([FromForm] UploadV2DTO input) =>
         (await commandDispatcher.SendAsync(new UploadAsWebpCommand(input.File, input.FileType, input.Title, input.Alt, input.Description, input.OtherUsersId))).Data.Url;
    */
    public async Task<ActionResult> UploaddForStaticPage([FromForm] UploadV2DTO input)
    {
        var result = await commandDispatcher.SendAsync(new UploadAsWebpCommand(
            input.File, input.FileType, input.Title, input.Alt, input.Description, input.OtherUsersId));
        return Ok(new { Link = result.Data.Url });

    }
    /// <summary>
    /// دریافت فایل
    /// </summary>
    /// <param name="guid"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("[action]/{guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(string guid, [FromQuery] Guid userId)
                => await queryDispatcher.SendAsync(new GetUploadedFileQuery(guid, userId));

}