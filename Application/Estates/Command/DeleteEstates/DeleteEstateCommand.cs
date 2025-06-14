using Application.Cqrs.Commands;
using Common.Utilities;
using Data.Contracts;
using Entities.Estates;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Estates.Command.DeleteEstates
{
    public record DeleteEstateCommand(Guid Id) : ICommand<ServiceResult>;
    public class DeleteEstateCommandHandler(IRepository<SmartRealEstatePricing> estateRepository,
        
    IHttpContextAccessor httpContextAccessor
        ) : ICommandHandler<DeleteEstateCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteEstateCommand request, CancellationToken cancellationToken)
        {
            var userIdString = httpContextAccessor.HttpContext?.User?.Identity?.GetUserId();
            if (string.IsNullOrWhiteSpace(userIdString))
                return ServiceResult.BadRequest("لطفا ابتدا لاگین کنید");

            var userId = Guid.Parse(userIdString);
            var estate = await estateRepository.Table
                .Where(x => x.Id == request.Id&&userId==x.UserId )
                .FirstOrDefaultAsync(cancellationToken);

            if (estate is null)
                return ServiceResult.NotFound("ملک مورد نظر یافت نشد");

            await estateRepository.SoftDeleteAsync(estate, cancellationToken);

            return ServiceResult.Ok();
        }
    }

}
