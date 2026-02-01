namespace CityCode.MandateSystem.Application.Query
{
    public class GetDocumentsByMandateReferenceQuery : IRequest<Common.Models.View.Result<List<Document>>>
    {
        public string MandateReference { get; set; } = string.Empty;
    }

    public class GetDocumentsByMandateReferenceQueryHandler(IApplicationDbContext context)
        : IRequestHandler<GetDocumentsByMandateReferenceQuery, Common.Models.View.Result<List<Document>>>
    {
        private readonly IApplicationDbContext _context = context;

        public async Task<Common.Models.View.Result<List<Document>>> Handle(GetDocumentsByMandateReferenceQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.MandateReference))
            {
                return Common.Models.View.Result<List<Document>>.Failure("MandateReference is required.");
            }

            var documents = await _context.Documents
                .AsNoTracking()
                .Where(d => d.MandateReference == request.MandateReference)
                .ToListAsync(cancellationToken);

            return Common.Models.View.Result<List<Document>>.Success(DateTime.UtcNow, documents);
        }
    }
}
