using Application.Cqrs.Commands;
using Data.Contracts;
using Entities.Medias;
using Entities.UploadedFiles;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Services;
using Services.Uploader;
using Services.Uploader.DTOs;

namespace Application.Uploaders.Command;

public class UploadCommand(IFormFile file, Guid objectId, MediaTypes type, bool withWatermark, UploadType uploadType) : ICommand<ServiceResult<string>>
{
    public IFormFile File { get; init; } = file;
    public Guid ObjectId { get; init; } = objectId;
    public MediaTypes Type { get; init; } = type;
    public bool WithWatermark { get; set; } = withWatermark;
    public UploadType UploadType { get; set; } = uploadType;

    public class UploadCommandHandle(IUploaderService uploadServices, IRepository<Media> mediaRepository, ILogger<UploadCommandHandle> logger) : ICommandHandler<UploadCommand, ServiceResult<string>>
    {
        public async Task<ServiceResult<string>> Handle(UploadCommand request, CancellationToken cancellationToken)
        {
            //set dynamic path based on Type in entry of method
            var dynamicPath = request.Type switch
            {
                MediaTypes.estatePhoto => "estatePhoto",
                _ => string.Empty,
            };
            logger.LogInformation($"upload request start with {request.ObjectId} ========> Type: {request.Type},Directory: {dynamicPath}");

            if (!string.IsNullOrEmpty(dynamicPath))
            {
                // upload to server
                var result = await uploadServices.UploadAsWebpV2(new UploadV2DTO
                {
                    File = request.File,
                    FileType = request.UploadType,

                });

                logger.LogInformation($"upload Finished by id {request.ObjectId} ========> Type: {request.Type},Directory: {dynamicPath}");
                //set pic to media
                var Media = new Media(result.Data.Url, request.ObjectId, request.Type);
                await mediaRepository.AddAsync(Media, cancellationToken);

                logger.LogInformation($"insert uploaded file by {request.ObjectId} ========> Type: {request.Type},Directory: {dynamicPath}");
                return ServiceResult.Ok<string>(result.Data.Url);

            }
            else
                logger.LogError("upload has error not complete");

            return ServiceResult.Fail("can not detect type");

        }
    }
}