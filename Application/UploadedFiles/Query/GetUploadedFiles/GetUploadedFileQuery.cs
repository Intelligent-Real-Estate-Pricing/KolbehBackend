using Application.Cqrs.Queris;
using Microsoft.AspNetCore.Mvc;
using Services.Uploader;

namespace Application.UploadedFiles.Query.GetUploadedFiles;

public class GetUploadedFileQuery : IQuery<IActionResult>
{
    public GetUploadedFileQuery(string guid, Guid userId)
    {
        Guid = guid;
        UserId = userId;
    }

    public string Guid { get; }
    public Guid UserId { get; }
}
public class GetUploadedFileHandler(IUploaderService uploaderService) : IQueryHandler<GetUploadedFileQuery, IActionResult>
{
    public async Task<IActionResult> Handle(GetUploadedFileQuery request, CancellationToken cancellationToken) =>
        await uploaderService.GetBy(request.Guid, request.UserId);
}