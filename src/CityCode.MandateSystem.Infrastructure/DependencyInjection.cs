namespace Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;

using CityCode.MandateSystem.Application.Common.Interfaces;
using CityCode.MandateSystem.Domain.Constants;
using CityCode.MandateSystem.Infrastructure.Data;
using CityCode.MandateSystem.Infrastructure.Data.Interceptors;
using CityCode.MandateSystem.Infrastructure.Identity;
using StackExchange.Redis;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connection = configuration.GetConnectionString("DefaultConnection");
        
        var timeout = configuration["DatabaseSettings:Timeout"];
        var commandTimeout = configuration["DatabaseSettings:CommandTimeout"];
        var maxPoolSize = configuration["DatabaseSettings:MaxPoolSize"];
        var minPoolSize = configuration["DatabaseSettings:MinPoolSize"];

        var connectionString = $"{connection};Timeout={timeout};CommandTimeout={commandTimeout};MaxPoolSize={maxPoolSize};MinPoolSize={minPoolSize}";

        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        services.AddScoped<IDapperContext, DapperContext>();
        // services.AddScoped(typeof(INpgApplicationDbContext<>), typeof(NpgApplicationDbContext<>));

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitialiser>();

        services
            .AddDefaultIdentity<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddSingleton(TimeProvider.System);
        services.AddTransient<IIdentityService, IdentityService>();

        services.AddAuthorization(options =>
        options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));
        services.AddAuthorization(options => options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));

        services.AddSingleton<IConnectionMultiplexer>(provider =>
        {
            string redisConnectionString = configuration.GetConnectionString("RedisUrl")!;
            return ConnectionMultiplexer.Connect(redisConnectionString);
        });

        return services;
    }
}
