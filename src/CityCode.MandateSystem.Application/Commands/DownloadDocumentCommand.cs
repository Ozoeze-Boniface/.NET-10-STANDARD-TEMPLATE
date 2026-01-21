namespace CityCode.MandateSystem.Application.Commands
{
    public class DownloadDocumentCommand : IRequest<Common.Models.View.Result<Document>>
    {
        public long DocumentId { get; set; }
    }

    public class DownloadDocumentCommandValidator : AbstractValidator<DownloadDocumentCommand>
    {
        public DownloadDocumentCommandValidator()
        {
            RuleFor(x => x.DocumentId)
                .NotEmpty().WithMessage("DocumentId is required.");
        }
    }

    public class DownloadDocumentCommandHandler(IApplicationDbContext context)
        : IRequestHandler<DownloadDocumentCommand, Common.Models.View.Result<Document>>
    {
        public async Task<Common.Models.View.Result<Document>> Handle(DownloadDocumentCommand request, CancellationToken cancellationToken)
        {
            var document = await context.Documents
                .FirstOrDefaultAsync(d => d.DocumentId == request.DocumentId, cancellationToken);

            if (document == null)
            {
                return Common.Models.View.Result<Document>.Failure("Document not found.");
            }

            return Common.Models.View.Result<Document>.Success(DateTime.UtcNow, document);
        }
    }
}