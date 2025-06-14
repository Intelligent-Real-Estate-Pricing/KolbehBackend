using Application.Cqrs.Commands;
using Data.Contracts;
using Entities.Estates;
using Microsoft.EntityFrameworkCore;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Estates.Command.PricingRequest
{
    public class PricingRequestCommand(Guid estateId) : ICommand<ServiceResult>
    {
        public Guid EstateId { get; } = estateId;
    }
    public class PricingRequestCommandHandler : ICommandHandler<PricingRequestCommand, ServiceResult>
    {
        private readonly IRepository<SmartRealEstatePricing> _repository;
/*        private readonly HttpClient _httpClient;*/

        public PricingRequestCommandHandler(
            IRepository<SmartRealEstatePricing> repository
            /*HttpClient httpClient*/)
        {
            _repository = repository;
  /*          _httpClient = httpClient;*/
        }

        public async Task<ServiceResult> Handle(PricingRequestCommand request, CancellationToken cancellationToken)
        {
            // جستجوی رکورد در جدول SmartRealEstatePricing
            var pricing = await _repository.Table
                .FirstOrDefaultAsync(x => x.Id == request.EstateId, cancellationToken);

            if (pricing == null)
            {
                return ServiceResult.Fail("ملک مورد نظر یافت نشد");
            }

            // بررسی PricingWithAi - اگر 0 یا null بود، API کال می‌کنیم
            if (pricing.PriceingWithAi == null || pricing.PriceingWithAi == 0)
            {
                try
                {
                    // var response = await _httpClient.GetAsync("https://api.example.com/pricing", cancellationToken);
                    // response.EnsureSuccessStatusCode();

                    return ServiceResult.Ok("درخواست قیمت‌گذاری با موفقیت ارسال شد");
                }
                catch (Exception ex)
                {
                    return ServiceResult.Fail($"خطا در ارسال درخواست قیمت‌گذاری: {ex.Message}");
                }
            }

            return ServiceResult.Ok("قیمت‌گذاری قبلاً انجام شده است");
        }
    }

}
