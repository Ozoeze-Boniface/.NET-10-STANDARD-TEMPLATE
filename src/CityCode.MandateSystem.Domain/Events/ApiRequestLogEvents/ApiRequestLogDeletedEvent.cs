namespace CityCode.MandateSystem.Domain.Events;

public class ApiRequestLogDeletedEvent(ApiRequestLog item) : BaseEvent
{
    public ApiRequestLog Item { get; } = item;
}
