namespace KeyRails.BankingApi.Web.Endpoints;

public interface IEndpointDefinition
{
    void DefineServices(IServiceCollection services);
    void DefineEndPoints(WebApplication app);
}
