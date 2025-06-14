using Application.Cqrs.Commands;
using Data.Contracts;
using Services;

public record CreateEstateDelegationCommand(EstateDelegationDto Dto) : ICommand<ServiceResult>;

public class CreateEstateDelegationCommandHandler(IRepository<EstateDelegation> repo)
    : ICommandHandler<CreateEstateDelegationCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(CreateEstateDelegationCommand request, CancellationToken cancellationToken)
    {
        var entity = new EstateDelegation
        {
            Id = Guid.NewGuid(),
            FullName = request.Dto.FullName,
            PhoneNumber = request.Dto.PhoneNumber,
            Title = request.Dto.Title,
            Zone = request.Dto.Zone,
            Address = request.Dto.Address,
            Type = request.Dto.Type
        };

        await repo.AddAsync(entity, cancellationToken);
        return ServiceResult.Ok();
    }
}
