namespace CityCode.MandateSystem.Application.Common.Behaviours;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using CityCode.MandateSystem.Application.Common.Interfaces;

public class LoggingBehaviour<TRequest>(ILogger<TRequest> logger, IUser user, IIdentityService identityService) : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger = logger;
    private readonly IUser _user = user;
    private readonly IIdentityService _identityService = identityService;

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = this._user.Id ?? string.Empty;
        var userName = string.Empty;

        if (!string.IsNullOrEmpty(userId))
        {
            userName = await this._identityService.GetUserNameAsync(userId);
        }

        this._logger.LogInformation("CityCode.MandateSystem Request: {Name} {@UserId} {@UserName} {@Request}",
            requestName, userId, userName, request);
    }
}
