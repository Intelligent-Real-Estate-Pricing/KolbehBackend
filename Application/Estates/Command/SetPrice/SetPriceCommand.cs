using Application.Cqrs.Commands;
using Application.Estates.DTOs;
using Data.Contracts;
using Entities.Estates;
using Microsoft.EntityFrameworkCore;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Estates.Command.SetPrice
{
    public record SetPriceCommand(SetPriceDto Dto) : ICommand<ServiceResult>;

    public class SetPriceCommandHandler(IRepository<SmartRealEstatePricing> repo)
    : ICommandHandler<SetPriceCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(SetPriceCommand request, CancellationToken cancellationToken)
        {
            var Estate =await repo.Table.Where(a => a.Id == request.Dto.EstatesId).FirstOrDefaultAsync();

            Estate.PriceingWithAi = request.Dto.Price;
            Estate.PricePerSquareMeter = request.Dto.PricePerMeter;
            repo.Update(Estate);
            return ServiceResult.Ok();

        }
    }
}
