using Application;
using Application.Cqrs.Commands;
using Application.Cqrs.Queris;
using Asp.Versioning;
using Kolbeh.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebFramework.Api;

/// <summary>
/// مشارکت در ساخت
/// </summary>
/// <param name="commandDispatcher"></param>
/// <param name="queryDispatcher"></param>
[ApiVersion("1")]
[ApiController]
public class BuildParticipationController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    : BaseController
{
    [HttpPost]
    [Authorize]
    public async Task<ApiResult> Create([FromBody] BuildParticipationDto dto)
        => (await commandDispatcher.SendAsync(new CreateBuildParticipationCommand(dto))).ToApiResult();

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ApiResult> Update(Guid id, [FromBody] BuildParticipationDto dto)
        => (await commandDispatcher.SendAsync(new UpdateBuildParticipationCommand(id, dto))).ToApiResult();

    [HttpPost("Delete")]
    [Authorize]
    public async Task<ApiResult> Delete([FromQuery] Guid id)
        => (await commandDispatcher.SendAsync(new DeleteBuildParticipationCommand(id))).ToApiResult();

    [HttpGet]
    [AllowAnonymous]
    public async Task<ApiResult<GlobalGridResult<BuildParticipationDto>>> GetAll(GetAllBuildParticipationDto Filters)
        => (await queryDispatcher.SendAsync(new GetAllBuildParticipationsQuery(Filters))).ToApiResult();

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ApiResult<BuildParticipationDto>> Get(Guid id)
        => (await queryDispatcher.SendAsync(new GetBuildParticipationByIdQuery(id))).ToApiResult();
}
