using Application.Cqrs.Commands;
using Data.Contracts;
using Entities.RealEstateInfoFiles;
using Microsoft.EntityFrameworkCore;
using Services;

public record DeleteRealEstateInfoFileCommand(Guid Id) : ICommand<ServiceResult>;

public class DeleteRealEstateInfoFileCommandHandler(IRepository<RealEstateInfoFile> repository)
    : ICommandHandler<DeleteRealEstateInfoFileCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(DeleteRealEstateInfoFileCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.Table
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.IsDeleted == false, cancellationToken);

        if (entity == null)
            return ServiceResult.NotFound("فایل اطلاعات املاک مورد نظر یافت نشد.");

      await  repository.SoftDeleteAsync(entity, cancellationToken);


        return ServiceResult.Ok();
    }
}