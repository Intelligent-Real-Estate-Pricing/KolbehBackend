using Application.Cqrs.Commands;
using Entities.UploadedFiles;
using Microsoft.AspNetCore.Http;
using Services;
using Services.Uploader;
using Services.Uploader.DTOs;

namespace Application.Uploaders.Command.UploadFileWithMainUploader;

public class UploadFileWithMainUpLoaderCommand(IFormFile file, UploadType fileType, string title, string alt, string description) : ICommand<ServiceResult<UploadResultDTO>>
{
    public IFormFile File { get; } = file;
    public UploadType FileType { get; } = fileType;
    public string Title { get; } = title;
    public string Alt { get; } = alt;
    public string Description { get; } = description;
}

public class UploadFileWithMainUpLoaderHandler(IUploaderService uploaderService) : ICommandHandler<UploadFileWithMainUpLoaderCommand, ServiceResult<UploadResultDTO>>
{
    public async Task<ServiceResult<UploadResultDTO>> Handle(UploadFileWithMainUpLoaderCommand request, CancellationToken cancellationToken) =>
        await uploaderService.UploadAsWebpV2(new UploadV2DTO
        {
            File = request.File,
            FileType = request.FileType,
            Title = request.Title,
            Alt = request.Alt,
            Description = request.Description
        });

}