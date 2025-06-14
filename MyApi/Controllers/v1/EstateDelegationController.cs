using Application.Cqrs.Commands;
using Application.Cqrs.Queris;
using Application;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebFramework.Api;
using Kolbeh.Api;

/// <summary>
/// خرید و اجاره 
/// </summary>
/// <param name="commandDispatcher"></param>
/// <param name="queryDispatcher"></param>
[ApiVersion("1")]
[ApiController]
public class EstateDelegationController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    : BaseController
{
    [HttpPost]
    [Authorize]
    public async Task<ApiResult> Create([FromBody] EstateDelegationDto dto)
        => (await commandDispatcher.SendAsync(new CreateEstateDelegationCommand(dto))).ToApiResult();

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ApiResult> Update(Guid id, [FromBody] EstateDelegationDto dto)
        => (await commandDispatcher.SendAsync(new UpdateEstateDelegationCommand(id, dto))).ToApiResult();

    [HttpPost("Delete")]
    [Authorize]
    public async Task<ApiResult> Delete([FromQuery] Guid id)
        => (await commandDispatcher.SendAsync(new DeleteEstateDelegationCommand(id))).ToApiResult();
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ApiResult<EstateDelegationDto>> GetById(Guid id)
    {
        var result = await queryDispatcher.SendAsync(new GetEstateDelegationByIdQuery(id));
        return result.ToApiResult();
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ApiResult<GlobalGridResult<EstateDelegationDto>>> GetAll([FromQuery] GetAllEstateDelegationDto filters)
        => (await queryDispatcher.SendAsync(new GetAllEstateDelegationsQuery(filters))).ToApiResult();
}
