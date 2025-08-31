using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CityCode.MandateSystem.Application.Common.Interfaces;
using CityCode.MandateSystem.Application.Services.UtilityServices.Interfaces;
using CityCode.MandateSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Npgsql;

namespace CityCode.MandateSystem.WorkerService
{
    public class TransactionPostingService : BackgroundService
    {
        private readonly ILogger<TransactionPostingService> _logger;
        private readonly IServiceScopeFactory _factory;
        private readonly IMandateService _mandateService;
        private readonly IDbConnection _connection;

        public TransactionPostingService(
            ILogger<TransactionPostingService> logger,
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
                using var scope = _factory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

                var today = DateOnly.FromDateTime(DateTime.Now);

                var schedules = await context.MandateSchedules.Include(p => p.Mandate)
                    .Where(s => s.NextRunDate == today && !s.IsEnded && s.WorkflowStatus == WorkflowStatus.MANDATE_APPROVED_BY_BANK)
                    .Where(s => !context.MandateTransactions
                        .Any(t => t.MandateId == s.MandateId && t.TransactionDate == today))
                    .ToListAsync(stoppingToken);

                var mandates = schedules
                    .Select(s => s.Mandate)
                    .ToList();

                foreach (var mandateSchedule in schedules)
                {
                    var result = await _mandateService.DoFundsTransfer(mandateSchedule.Mandate);

                    var transaction = new MandateTransaction(
                        mandateScheduleId: mandateSchedule.MandateScheduleId,
                        transactionReference: result.PaymentReference ?? string.Empty,
                        amount: result.Amount,
                        currency: "NGN",
                        transactionDate: today,
                        transactionStatus: result.ResponseCode == "00" ? TransactionStatus.SUCCESSFUL.ToString() : TransactionStatus.FAILED.ToString());
                    transaction.UpdateFromResponse(result.ResponseCode, result.SessionID, result.ChannelCode, result.NameEnquiryRef, result.DestinationInstitutionCode, result.BeneficiaryAccountName, result.BeneficiaryAccountNumber, result.BeneficiaryKYCLevel, result.BeneficiaryBankVerificationNumber,
                         result.OriginatorAccountName, result.OriginatorAccountNumber, result.OriginatorBankVerificationNumber, result.OriginatorKYCLevel, result.TransactionLocation, result.Narration, result.PaymentReference!);
                    transaction.UpdateStatus(transaction.TransactionStatus, "SUCCESSFUL", result.TransactionId);

                    context.MandateTransactions.Add(transaction);
                    await context.SaveChangesAsync(stoppingToken);
                }

                await context.SaveChangesAsync(stoppingToken);

                _logger.LogInformation("Worker completed cycle at: {Time}", DateTimeOffset.Now);

                await Task.Delay(TimeSpan.FromHours(2), stoppingToken);
            }
        }
    }
}