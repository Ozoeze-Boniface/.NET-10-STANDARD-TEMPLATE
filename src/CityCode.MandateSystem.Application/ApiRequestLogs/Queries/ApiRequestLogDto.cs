namespace CityCode.MandateSystem.Application.Queries;

public class ApiRequestLogDto
{
    public int ApiRequestLogId { get; init; }

    private sealed class Mapping : Profile
    {
        public Mapping() => this.CreateMap<ApiRequestLog, ApiRequestLogDto>();
    }
}
