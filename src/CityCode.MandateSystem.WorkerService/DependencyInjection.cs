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
using CityCode.MandateSystem.Application.Services.UtilityServices.Interfaces;
using CityCode.MandateSystem.Application.ExternalServices;
using CityCode.MandateSystem.Application.Settings;

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

        services.AddSingleton<IDapperContext, DapperContext>();
        services.AddSingleton<IMandateService, MandateService>();
        services.AddSingleton<IGenericServices, GenericServices>();
        services.Configure<SystemSettings>(configuration.GetSection("SystemSettings").Bind);

        services.AddHttpClient("NibssClient", client =>
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            client.Timeout = TimeSpan.FromSeconds(30);
        })
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
            var handler = new HttpClientHandler
            {
                // Accept all certs (DEV ONLY, remove in prod)
                // ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            return handler;
        });

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());


        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddSingleton(provider =>
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
