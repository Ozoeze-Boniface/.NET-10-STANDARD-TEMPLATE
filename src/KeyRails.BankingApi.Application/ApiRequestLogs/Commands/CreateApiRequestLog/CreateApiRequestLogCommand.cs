namespace KeyRails.BankingApi.Application.Commands;

public record CreateApiRequestLogCommand : IRequest<Result<int>>
{
    public int ApiRequestLogId { get; set; }
    public string? ApiUserId { get; set; }
    public string? ClientUniqueRequestId { get; set; }
    public string? EndPointUrl { get; set; }
    public string? ApiRequestJson { get; set; }
    public DateTime ApiRequestDate { get; set; }
    public DateTime ApiRequestTime { get; set; }
    public string? ApiResponseJson { get; set; }
    public DateTime? ApiResponseDate { get; set; }
    public DateTime? ApiResponseTime { get; set; }
    public string? ApiCallStatus { get; set; }
    public ulong RowVersion { get; set; }
}

public class CreateApiRequestLogCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateApiRequestLogCommand, Result<int>>
{
    private readonly IApplicationDbContext context = context;

    public async Task<Result<int>> Handle(CreateApiRequestLogCommand request, CancellationToken cancellationToken)
    {
        var requestTime = DateTime.Now;
        var entity = new ApiRequestLog
        {
            ApiRequestLogId = request.ApiRequestLogId,
            ApiCallStatus = request.ApiCallStatus,
            ApiRequestDate = request.ApiRequestDate,
            ApiRequestJson = request.ApiRequestJson,
            ApiRequestTime = request.ApiRequestTime,
            ApiResponseDate = request.ApiResponseDate,
            ApiResponseJson = request.ApiResponseJson,
            ApiResponseTime = request.ApiResponseDate,
            ApiUserId = "system",
            ClientUniqueRequestId = request.ClientUniqueRequestId,
            EndPointUrl = request.EndPointUrl,
            RowVersion = request.RowVersion
        };

        entity.AddDomainEvent(new ApiRequestLogCreatedEvent(entity));

        this.context.ApiRequestLogs.Add(entity);

        var isSaved = await this.context.SaveChangesAsync(cancellationToken);

        if (isSaved > 0)
        {
            return new Result<int>
            {
                ResponseDescription = "Record created Successfully",
                IsSuccess = true,
                ResponseCode = "000",
                Content = isSaved,
                ResponseTime = DateTime.Now,
                RequestTime = requestTime
            };
        }

        return new Result<int>
        {
            ResponseDescription = "Record Creation failed: An error occurred",
            IsSuccess = false,
            ResponseCode = "999",
            Content = 0,
            ResponseTime = DateTime.Now,
            ErrorMessage = "Error Saving the record",
            RequestTime = requestTime
        };
    }
}
