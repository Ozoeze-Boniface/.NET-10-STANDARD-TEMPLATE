namespace KeyRails.BankingApi.Application.Commands;
public record UpdateApiRequestLogCommand : IRequest
{
    public int ApiRequestLogId { get; init; }
    public string? ApiCallStatus { get; set; }
    public string? ApiResponseJson { get; set; }
    public ulong RowVersion { get; set; }

}

public class UpdateApiRequestLogCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateApiRequestLogCommand>
{
    private readonly IApplicationDbContext _context = context;

    public async Task Handle(UpdateApiRequestLogCommand request, CancellationToken cancellationToken)
    {
        var entity = await this._context.ApiRequestLogs
            .FindAsync([request.ApiRequestLogId], cancellationToken);

        Guard.Against.NotFound(request.ApiRequestLogId, entity);
        //Update Fields you want to update
        //entity.Title = request.Title;
        entity.ApiCallStatus = request.ApiCallStatus;
        entity.ApiResponseJson = request.ApiResponseJson;
        entity.RowVersion++;
        await this._context.SaveChangesAsync(cancellationToken);

    }
}
