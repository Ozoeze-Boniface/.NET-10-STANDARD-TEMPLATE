
using Microsoft.AspNetCore.Http;

namespace CityCode.MandateSystem.Application.Commands
{
    public class UploadDocumentCommand : IRequest<Common.Models.View.Result<List<Document>>>
    {
        public string MandateReference { get; set; } = string.Empty;
        public List<IFormFile> Documents { get; set; } = new List<IFormFile>();
    }

    public class UploadDocumentRequest
    {
        public string MandateReference { get; set; } = string.Empty;
        public List<IFormFile> Documents { get; set; } = new List<IFormFile>();
    }

    public class UploadDocumentCommandValidator : AbstractValidator<UploadDocumentCommand>
    {
        public UploadDocumentCommandValidator()
        {
            RuleFor(x => x.MandateReference)
                .NotEmpty().WithMessage("MandateReference is required.")
                .MaximumLength(100);

            RuleFor(x => x.Documents)
                .NotEmpty().WithMessage("At least one document is required.");
        }
    }

    public class UploadDocumentCommandHandler(IApplicationDbContext context)
        : IRequestHandler<UploadDocumentCommand, Common.Models.View.Result<List<Document>>>
    {
        public async Task<Common.Models.View.Result<List<Document>>> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
        {
            var mandateExixts = await context.MandateRequests
                .AnyAsync(m => m.MandateReference == request.MandateReference, cancellationToken);
            if (!mandateExixts)
            {
                return Common.Models.View.Result<List<Document>>.Failure("MandateReference does not exist.");
            }
            List<Document> documents = [];
            foreach (var formFile in request.Documents)
            {
                if (formFile.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await formFile.CopyToAsync(memoryStream, cancellationToken);
                    documents.Add(new Document
                    {
                        MandateReference = request.MandateReference,
                        DocumentName = formFile.FileName,
                        ContentType = formFile.ContentType,
                        FileData = memoryStream.ToArray()
                    });
                }
            }
            context.Documents.AddRange(documents);

            await context.SaveChangesAsync(cancellationToken);
            return Common.Models.View.Result<List<Document>>.Success(DateTime.UtcNow, documents);
        }
    }
}