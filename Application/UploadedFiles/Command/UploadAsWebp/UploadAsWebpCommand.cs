using Application.Cqrs.Commands;
using Entities.UploadedFiles;
using Microsoft.AspNetCore.Http;
using Services;
using Services.Uploader.DTOs;

namespace Application.UploadedFiles.Command.UploadAsWebp;
public class UploadAsWebpCommand(IFormFile file, UploadType fileType, string title, string alt, string description, List<Guid> otherUsersId) : ICommand<ServiceResult<UploadResultDTO>>
{
    public IFormFile File { get; set; } = file;
    public UploadType FileType { get; set; } = fileType;
    public string Title { get; set; } = title;
    public string Alt { get; set; } = alt;
    public string Description { get; set; } = description;
    public List<Guid> OtherUsersId { get; set; } = otherUsersId;

}