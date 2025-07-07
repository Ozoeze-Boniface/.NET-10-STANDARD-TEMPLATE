using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Npgsql;

namespace CityCode.MandateSystem.WorkerService;

public class SampleWorker : IHostedLifecycleService
{
    private readonly ILogger<SampleWorker> _logger;
    private readonly IServiceScopeFactory _factory;
    private readonly IDbConnection connection;
    public SampleWorker(ILogger<SampleWorker> logger, IServiceScopeFactory factory, IConfiguration configuration)
    {
        _factory = factory;
        _logger = logger;
        connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));

    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Worker starts at: {Time}", DateTimeOffset.Now);
        Console.WriteLine("Worker starts at: {0}", DateTimeOffset.Now);
        return Task.CompletedTask;
    }
    public Task StartingAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Worker starting at: {Time}", DateTimeOffset.Now);

        return Task.CompletedTask;

    }
    public async Task StartedAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Worker started at: {Time}", DateTimeOffset.Now);

        await Task.CompletedTask;

    }


    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Worker stops at: {Time}", DateTimeOffset.Now);

        return Task.CompletedTask;

    }
    public Task StoppingAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Worker stopping at: {Time}", DateTimeOffset.Now);

        return Task.CompletedTask;

    }

    public Task StoppedAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Worker stopped at: {Time}", DateTimeOffset.Now);

        return Task.CompletedTask;

    }
}
