using Application.Cqrs.Commands;
using Application.Cqrs.Queris;
using Application;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WebFramework.Api;
using Application.RealEstateInfoFiles.DTOs;
using Kolbeh.Api;
using Application.RealEstateInfoFiles.Query;

/// <summary>
/// اطلاعات فایل املاک
/// </summary>
/// <param name="commandDispatcher"></param>
/// <param name="queryDispatcher"></param>
[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class RealEstateInfoFileController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    : BaseController
{
    /// <summary>
    /// ساخت فایل اطلاعات املاک جدید
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<ApiResult> Create([FromBody] CreateRealEstateInfoFileDTO input)
        => (await commandDispatcher.SendAsync(new CreateRealEstateInfoFileCommand(input))).ToApiResult();

    /// <summary>
    /// ویرایش فایل اطلاعات املاک
    /// </summary>
    [HttpPut("{id}")]
    [Authorize]
    public async Task<ApiResult> Update(Guid id, [FromBody] UpdateRealEstateInfoFileDTO input)
        => (await commandDispatcher.SendAsync(new UpdateRealEstateInfoFileCommand(id, input))).ToApiResult();

    /// <summary>
    /// حذف فایل اطلاعات املاک
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ApiResult> Delete(Guid id)
        => (await commandDispatcher.SendAsync(new DeleteRealEstateInfoFileCommand(id))).ToApiResult();

    /// <summary>
    /// گرفتن لیست فایل‌های اطلاعات املاک با فیلتر و صفحه‌بندی
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ApiResult<GlobalGridResult<GetRealEstateInfoFileListDto>>> GetAll([FromQuery] GetAllRealEstateInfoFileDTO filters)
        => (await queryDispatcher.SendAsync(new GetAllRealEstateInfoFilesQuery(filters))).ToApiResult();

    /// <summary>
    /// گرفتن اطلاعات یک فایل املاک با آیدی
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ApiResult<RealEstateInfoFileDTO>> Get(Guid id)
        => (await queryDispatcher.SendAsync(new GetRealEstateInfoFileByIdQuery(id))).ToApiResult();
}
