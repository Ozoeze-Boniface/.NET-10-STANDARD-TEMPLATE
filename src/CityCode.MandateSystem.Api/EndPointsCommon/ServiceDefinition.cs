// using CityCode.MandateSystem.Api.Endpoints;

using CityCode.MandateSystem.Api.Endpoints;

namespace CityCode.MandateSystem.Web.Endpoints;

public class ServiceDefinition : IEndpointDefinition
{
        public void DefineEndPoints(WebApplication app)
        {

                app.MapGroup("/auth")
                        .AuthGroup()
                        .RequireCors("corsapp")
                        .RequireRateLimiting("LimitPolicy")
                        .WithTags("AuthManager");

                app.MapGroup("/user")
                        .UserGroup()
                        .RequireCors("corsapp")
                        .RequireRateLimiting("LimitPolicy")
                        .WithTags("UserManager");

                app.MapGroup("/mandate")
                        .MandateGroup()
                        .RequireCors("corsapp")
                        .RequireRateLimiting("LimitPolicy")
                        .WithTags("Mandate Manager");

                app.MapGroup("/activity-logs")
                        .ActivityGroup()
                        .RequireCors("corsapp")
                        .RequireRateLimiting("LimitPolicy")
                        .WithTags("Activity Manager");

        }
        public void DefineServices(IServiceCollection services)
        {

        }
}
