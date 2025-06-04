using Application.Cqrs.Commands;
using Data.Contracts;
using Entities.Medias;
using Entities.UploadedFiles;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Services;
using Services.Uploader;
using Services.Uploader.DTOs;

namespace Application.Uploaders.Command.UploadWithoutObjectId;
public class UploadWithoutObjectIdCommand(IFormFile file, MediaTypes type, bool withWatermark, UploadType uploadType) : ICommand<ServiceResult<string>>
{
    public IFormFile File { get; init; } = file;
    public MediaTypes Type { get; init; } = type;
    public bool WithWatermark { get; set; } = withWatermark;
    public UploadType UploadType { get; set; } = uploadType;

    public class UploadWithoutObjectIdCommandHandle(IUploaderService uploadServices, IRepository<Media> mediaRepository, ILogger<UploadWithoutObjectIdCommandHandle> logger) : ICommandHandler<UploadWithoutObjectIdCommand, ServiceResult<string>>
    {
        public async Task<ServiceResult<string>> Handle(UploadWithoutObjectIdCommand request, CancellationToken cancellationToken)
        {
            //set dynamic path based on Type in entry of method
            var dynamicPath = request.Type switch
            {
                MediaTypes.estatePhoto => "estatePhoto",
                _ => string.Empty,
            };

            if (!string.IsNullOrEmpty(dynamicPath))
            {
                // upload to server
                var result = await uploadServices.UploadAsWebpV2(new UploadV2DTO
                {
                    File = request.File,
                    FileType = request.UploadType,

                });

                return ServiceResult.Ok<string>(result.Data.Url);

            }
            else
                logger.LogError("upload has error not complete");

            return ServiceResult.Fail("can not detect type");

        }
    }
}