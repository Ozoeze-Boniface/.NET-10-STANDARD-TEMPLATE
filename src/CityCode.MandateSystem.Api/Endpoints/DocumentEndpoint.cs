using Microsoft.AspNetCore.Mvc;

namespace CityCode.MandateSystem.Api.Endpoints
{
    public static class DocumentEndpoint
    {
        public static RouteGroupBuilder DocumentGroup(this RouteGroupBuilder group)
        {
            group.MapPost("/upload-document", async ([FromForm]UploadDocumentRequest request, ISender sender) =>
                {
                    var command = new UploadDocumentCommand
                    {
                        MandateReference = request.MandateReference,
                        Documents = request.Documents
                    };
                    var result = await sender.Send(command);
                    return result;
                })
                .WithDisplayName("Upload Document").Accepts<UploadDocumentRequest>("multipart/form-data").DisableAntiforgery();

            group.MapGet("/download-document/{documentId}", async (long documentId, ISender sender) =>
                {
                    var command = new DownloadDocumentCommand { DocumentId = documentId };
                    var result = await sender.Send(command);

                    if (result.Value == null)
                    {
                        return Results.NotFound(result);
                    }

                    return Results.File(
                        result.Value.FileData,
                        result.Value.ContentType,
                        result.Value.DocumentName
                    );
                })
                .WithDisplayName("Download Document");
            
            return group;
        }
    }
}