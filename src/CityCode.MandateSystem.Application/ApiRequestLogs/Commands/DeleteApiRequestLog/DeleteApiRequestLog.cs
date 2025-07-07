namespace CityCode.MandateSystem.Application.Commands;
public record DeleteApiRequestLogCommand(int ApiRequestLogId) : IRequest<Result<int>>;

public class DeleteApiRequestLogCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteApiRequestLogCommand, Result<int>>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result<int>> Handle(DeleteApiRequestLogCommand request, CancellationToken cancellationToken)
    {
        var requestTime = DateTime.Now;

        var entity = await this._context.ApiRequestLogs
            .FindAsync([request.ApiRequestLogId], cancellationToken);

        Guard.Against.NotFound(request.ApiRequestLogId, entity);

        this._context.ApiRequestLogs.Remove(entity);

        entity.AddDomainEvent(new ApiRequestLogDeletedEvent(entity));

        var isDeleted = await this._context.SaveChangesAsync(cancellationToken);

        if (isDeleted > 0)
        {
            return new Result<int>
            {
                ResponseDescription = "Record deleted Successfully",
                IsSuccess = true,
                ResponseCode = "000",
                Content = isDeleted,
                ResponseTime = DateTime.Now,
                RequestTime = requestTime
            };
        }

        return new Result<int>
        {
            ResponseDescription = "Record deletion failed: An error occurred",
            IsSuccess = false,
            ResponseCode = "999",
            Content = 0,
            ResponseTime = DateTime.Now,
            ErrorMessage = "Error deleting the record",
            RequestTime = requestTime
        };

    }

}
