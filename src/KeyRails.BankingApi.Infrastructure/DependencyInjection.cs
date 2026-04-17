namespace Microsoft.Extensions.DependencyInjection;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;

using KeyRails.BankingApi.Application.Common.Interfaces;
using KeyRails.BankingApi.Domain.Constants;
using KeyRails.BankingApi.Infrastructure.Data;
using KeyRails.BankingApi.Infrastructure.Data.Interceptors;
using KeyRails.BankingApi.Infrastructure.Identity;
using StackExchange.Redis;
using KeyRails.BankingApi.Infrastructure.Seeders.Interface;
using KeyRails.BankingApi.Infrastructure.Seeders;
using Microsoft.AspNetCore.Builder;

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
        services.AddScoped<IIdentityService, IdentityService>();

        services.AddScoped<ApplicationDbContextInitialiser>();

        services.AddSingleton(TimeProvider.System);

        services.AddAuthorization(options =>
        options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));
        services.AddAuthorization(options => options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));

        services.AddSingleton<IConnectionMultiplexer>(provider =>
        {
            string redisConnectionString = configuration.GetConnectionString("RedisUrl")!;
            return ConnectionMultiplexer.Connect(redisConnectionString);
        });

        services.AddScoped<ISeederService, SeederService>();
        services.AddScoped<IDataSeeder, UserSeeder>();

        return services;
    }

    public static async Task InitializeSeed(this IApplicationBuilder app, CancellationToken cancellationToken = default)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<ISeederService>();
        await initializer.SeedAllAsync();
    }
}
