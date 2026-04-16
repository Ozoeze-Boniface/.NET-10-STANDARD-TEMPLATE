// using KeyRails.BankingApi.Api.Endpoints;

namespace KeyRails.BankingApi.Web.Endpoints;

public class ServiceDefinition : IEndpointDefinition
{
        public void DefineEndPoints(WebApplication app)
        {
                app.MapGroup("/todo")
                        .TodoItemGroup()
                        .RequireCors("corsapp")
                        .RequireRateLimiting("LimitPolicy")
                        .WithTags("TodoItemManager");
        }

        public void DefineServices(IServiceCollection services)
        {
        }
}
