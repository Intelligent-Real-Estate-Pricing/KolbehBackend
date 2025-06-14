using Application.Cqrs.Commands;
using Application.Cqrs.Queris;
using Application.Estates.Command.CreateEstates;
using Application.Estates.Command.DeleteEstates;
using Application.Estates.DTOs;
using Application.Estates.Query.GetAll;
using Application.Estates.Query.GetById;
using Application.Estates.Query.GetMyEstates;
using Application;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebFramework.Api;
using Application.Estates.Command.PricingRequest;

namespace Kolbeh.Api.Controllers.v1
{
    /// <summary>
    /// قیمت گذازی با ai 
    /// </summary>
    /// <param name="commandDispatcher"></param>
    /// <param name="queryDispatcher"></param>
    [ApiVersion("1")]
    [ApiController]
    public class SmartRealEstatePricingController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
      : BaseController
    {
        /// <summary>
        /// ساخت ملک جدید
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<ApiResult> Create([FromBody] CreateEstatesDTO input)
            => (await commandDispatcher.SendAsync(new CreateEstateCommand(input))).ToApiResult();

        /// <summary>
        /// حذف ملک
        /// </summary>
        [HttpPost("[action]")]
        [Authorize]
        public async Task<ApiResult> Delete([FromQuery] Guid id)
            => (await commandDispatcher.SendAsync(new DeleteEstateCommand(id))).ToApiResult();

        /// <summary>
        /// گرفتن لیست املاک با فیلتر و صفحه‌بندی
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ApiResult<GlobalGridResult<GetEstateListDto>>> GetAll([FromQuery] GetAllEstateDTO filters)
            => (await queryDispatcher.SendAsync(new GetAllEstatesQuery(filters))).ToApiResult();

        /// <summary>
        /// گرفتن اطلاعات یک ملک با آیدی
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ApiResult<EstateDTO>> Get(Guid id)
            => (await queryDispatcher.SendAsync(new GetEstateByIdQuery(id))).ToApiResult();


        /// <summary>
        /// درخواست قیمت گذاری انلاین
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [Authorize]
        public async Task<ApiResult> PricingRequest([FromQuery] Guid id)
      => (await commandDispatcher.SendAsync(new PricingRequestCommand(id))).ToApiResult();
         
        /// <summary>
        /// درخواست قیمت گذاری انلاین
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [Authorize]
        public async Task<ApiResult> PricingRequestV2([FromQuery] Guid id,string ImageUrl)
      => (await commandDispatcher.SendAsync(new PricingRequestCommand(id))).ToApiResult();

        /// <summary>
        /// گرفتن لیست املاک متعلق به کاربر لاگین کرده
        /// </summary>
        [HttpGet("[action]")]
        [Authorize]
        public async Task<ApiResult<List<EstateDTO>>> GetMyEstates()
            => (await queryDispatcher.SendAsync(new GetMyEstatesQuery())).ToApiResult();
    }

}
