using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MediatR;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using CityCode.MandateSystem.Application.Common.Interfaces;
using CityCode.MandateSystem.Infrastructure.Data;
using ZymLabs.NSwag.FluentValidation;
using StackExchange.Redis;
using Serilog.Sinks.Elasticsearch;

public static class DependencyInjection
{
    public static IServiceCollection AddWorkerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();
        // services.AddScoped<IUser, CurrentUser>();
        // services.AddScoped<ServiceJobs>();
        // services.AddSingleton(typeof(INpgApplicationDbContext<>), typeof(NpgApplicationDbContext<>));
        // services.AddSingleton<LedgerBalance>();
        // services.AddSingleton(typeof(INoSqlContext<>), typeof(NoSqlContext<>));
        // services.AddSingleton<ICasaSweep, CasaSweep>();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        Ardalis.GuardClauses.Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

        services.AddScoped<IDapperContext, DapperContext>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());


        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddScoped(provider =>
        {
            var validationRules = provider.GetService<IEnumerable<FluentValidationRule>>();
            var loggerFactory = provider.GetService<ILoggerFactory>();

            return new FluentValidationSchemaProcessor(provider, validationRules, loggerFactory);
        });

        services.AddSingleton<IConnectionMultiplexer>(provider =>
        {
            string redisConnectionString = configuration.GetConnectionString("RedisUrl")!;
            return ConnectionMultiplexer.Connect(redisConnectionString);
        });

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var serilogConfiguration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile(
                   $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                   optional: true)
               .Build();

        Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(serilogConfiguration["ElasticConfiguration:Uri"]!))
        {
            AutoRegisterTemplate = true,
            IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name!.ToLower()}-{environment?.ToLower()}-{DateTime.UtcNow:yyyy-MM}",
            NumberOfReplicas = 1,
            NumberOfShards = 2,
        })
        .Enrich.WithProperty("Enviroment", environment)
        .ReadFrom.Configuration(configuration)
        .CreateLogger();

        return services;
    }


}
