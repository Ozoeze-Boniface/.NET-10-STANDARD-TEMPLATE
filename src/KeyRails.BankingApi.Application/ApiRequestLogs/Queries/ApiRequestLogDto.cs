namespace KeyRails.BankingApi.Application.Queries;

public class ApiRequestLogDto
{
    public int ApiRequestLogId { get; init; }

    private sealed class Mapping : Profile
    {
        public Mapping() => this.CreateMap<ApiRequestLog, ApiRequestLogDto>();
    }
}
