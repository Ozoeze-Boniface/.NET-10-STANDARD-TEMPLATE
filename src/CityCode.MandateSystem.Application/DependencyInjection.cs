
using CityCode.MandateSystem.Application.Services.UtilityServices;
using CityCode.MandateSystem.Application.Services.UtilityServices.Implementations;
using CityCode.MandateSystem.Application.Services.UtilityServices.Interfaces;
using CityCode.MandateSystem.Application.Settings;
using StackExchange.Redis;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        // services.AddAutoMapper(typeof(Mapper).Assembly);

        services.AddHttpClient();

        services.AddTransient<IGenericServices, GenericServices>();
        services.AddTransient<IInfoBipService, InfoBipService>();  
        services.AddTransient<IMandateService, MandateService>();
        services.AddScoped<IEmailService, EmailService>();
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings").Bind);
        services.Configure<SystemSettings>(configuration.GetSection("SystemSettings").Bind);

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        });


        return services;
    }
}
