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
public class UploadFileCommand : ICommand<ServiceResult<string>>
{
    public UploadFileCommand(IFormFile file, Guid objectId, MediaTypes type, bool withWatermark, UploadType uploadType)
    {
        File = file;
        ObjectId = objectId;
        Type = type;
        WithWatermark = withWatermark;
        UploadType = uploadType;
    }
    public IFormFile File { get; init; }
    public Guid ObjectId { get; init; }
    public MediaTypes Type { get; init; }
    public bool WithWatermark { get; set; }
    public UploadType UploadType { get; set; }

    public class UploadFileCommandHandler(IUploaderService uploadServices, IRepository<Media> mediaRepository, ILogger<UploadFileCommand> logger) : ICommandHandler<UploadFileCommand, ServiceResult<string>>
    {
        public async Task<ServiceResult<string>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
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
                //set file to media
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