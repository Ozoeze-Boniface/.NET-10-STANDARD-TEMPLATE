// using CityCode.MandateSystem.Api.Endpoints;

using CityCode.MandateSystem.Api.Endpoints;

namespace CityCode.MandateSystem.Web.Endpoints;
public class ServiceDefinition : IEndpointDefinition
{
    public void DefineEndPoints(WebApplication app)
    {

        app.MapGroup("/CasaSweep")
                .EocCasaGroup()
                .RequireCors("corsapp")
                .RequireRateLimiting("LimitPolicy")
                .WithTags("EocCasaManager");

    }
    public void DefineServices(IServiceCollection services)
    {

    }
}
