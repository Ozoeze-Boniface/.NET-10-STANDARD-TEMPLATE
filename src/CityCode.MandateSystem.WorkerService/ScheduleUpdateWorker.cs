using System.Data;
using CityCode.MandateSystem.Application.Common.Interfaces;
using CityCode.MandateSystem.Application.Services.UtilityServices.Interfaces;
using CityCode.MandateSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace CityCode.MandateSystem.WorkerService;

public class ScheduleUpdateWorker : BackgroundService
{
    private readonly ILogger<ScheduleUpdateWorker> _logger;
    private readonly IServiceScopeFactory _factory;
    private readonly IMandateService _mandateService;
    private readonly IDbConnection _connection;

    public ScheduleUpdateWorker(
        ILogger<ScheduleUpdateWorker> logger,
        IServiceScopeFactory factory,
        IConfiguration configuration,
        IMandateService mandateService)
    {
        _factory = factory;
        _mandateService = mandateService; // singleton, safe to inject directly
        _logger = logger;
        _connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker started at: {Time}", DateTimeOffset.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _factory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

                _logger.LogInformation("SCHEDULE UPDATE WORKER RUNNING");
                var mandates = await context.MandateSchedules
                    .Where(s =>
                        s.EndDate > DateOnly.FromDateTime(DateTime.Now) &&
                        !s.IsEnded &&
                        s.WorkflowStatus != WorkflowStatus.MANDATE_APPROVED_BY_BANK)
                    .ToListAsync(stoppingToken);

                foreach (var mandate in mandates)
                {
                    _logger.LogInformation("PROCESSING RECORD FOR {MandateMandateId}", mandate.MandateId);
                    var result = await _mandateService.GetMandateStatus(mandate.NibbsMandateCode);

                    if (result.Data?.WorkflowStatus == "Bank Approved")
                    {
                        mandate.WorkflowStatus = WorkflowStatus.MANDATE_APPROVED_BY_BANK;
                        mandate.Mandate.WorkflowStatus = WorkflowStatus.MANDATE_APPROVED_BY_BANK;
                        mandate.DateOfBankApproval = DateTime.UtcNow;
                    }
                    _logger.LogInformation("DONE PROCESSING RECORD FOR {MandateMandateId}", mandate.MandateId);
                }

                await context.SaveChangesAsync(stoppingToken);

                _logger.LogInformation("Worker completed cycle at: {Time}", DateTimeOffset.Now);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError("Exception while scheduling mandate for transaction {error}", e.Message);
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
