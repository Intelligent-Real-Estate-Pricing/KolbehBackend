using Application.Cqrs.Commands;
using Data.Contracts;
using Services;

public record UpdateEstateDelegationCommand(Guid Id, EstateDelegationDto Dto) : ICommand<ServiceResult>;

public class UpdateEstateDelegationCommandHandler(IRepository<EstateDelegation> repo)
    : ICommandHandler<UpdateEstateDelegationCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(UpdateEstateDelegationCommand request, CancellationToken cancellationToken)
    {
        var entity = await repo.GetByIdAsync(cancellationToken, request.Id);
        if (entity == null) return ServiceResult.NotFound("یافت نشد");

        entity.FullName = request.Dto.FullName;
        entity.PhoneNumber = request.Dto.PhoneNumber;
        entity.Title = request.Dto.Title;
        entity.Zone = request.Dto.Zone;
        entity.Address = request.Dto.Address;
        entity.Type = request.Dto.Type;

        await repo.UpdateAsync(entity, cancellationToken);
        return ServiceResult.Ok();
    }
}
