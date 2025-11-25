using System.Globalization;
using AutoMapper;
using CityCode.MandateSystem.Domain.Enums;
using CityCode.MandateSystem.Infrastructure.Data;
using CsvHelper;
using CsvHelper.Configuration;

namespace CityCode.MandateSystem.WorkerService
{
    public class DataMigrationWorkflow(IServiceScopeFactory factory, IMapper mapper, IConfiguration configuration) : BackgroundService
    {
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var runMigrations = configuration.GetValue<bool>("RUN_MIGRATIONS");
            if (!runMigrations)
            {
                return;
            }
            var scope = factory.CreateScope();
            var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                PrepareHeaderForMatch = args => args.Header.Trim(),
                MissingFieldFound = null,
                HeaderValidated = null,
                IgnoreBlankLines = true
            };

            using var reader = new StreamReader("Data_Migration.csv");
            using var csv = new CsvReader(reader, config);

            var csvRecords = csv.GetRecords<dynamic>().ToList();

            using var transaction = await _context.Database.BeginTransactionAsync(stoppingToken);

            foreach (var csvRecord in csvRecords)
            {
                var mandate = new Mandate();

                mandate.MandateReference = string.IsNullOrEmpty(GetString(csvRecord, "MandateReference")) ? GenerateMandateRefernce() : GetString(csvRecord, "MandateReference");
                mandate.NibbsMandateCode = GetString(csvRecord, "NibbsMandateCode");
                mandate.ProductId = GetInt(csvRecord, "ProductId");
                mandate.BillerId = GetInt(csvRecord, "BillerId");
                mandate.WorkflowStatus = GetEnum<WorkflowStatus>(csvRecord, "WorkflowStatus", WorkflowStatus.MANDATE_APPROVED_BY_BANK);
                mandate.MandateStatus = GetEnum<MandateStatus>(csvRecord, "MandateStatus", MandateStatus.INACTIVE);
                mandate.ProgressStatus = GetEnum<ProgressStatus>(csvRecord, "ProgressStatus", ProgressStatus.NIBBS_REJECTED);
                mandate.SubscriberCode = GetString(csvRecord, "SubscriberCode");
                mandate.ProductTotalAmount = GetDecimal(csvRecord, "ProductTotalAmount");
                mandate.TransactionAmount = GetDecimal(csvRecord, "TransactionAmount");
                mandate.BankCode = GetString(csvRecord, "BankCode");
                mandate.PayerName = GetString(csvRecord, "PayerName");
                mandate.PayerAddress = GetString(csvRecord, "PayerAddress");
                mandate.PayerEmail = GetString(csvRecord, "PayerEmail");
                mandate.PayerPhoneNumber = GetString(csvRecord, "PayerPhoneNumber");
                mandate.AccountName = GetString(csvRecord, "AccountName");
                mandate.PayerAccountNumber = GetString(csvRecord, "PayerAccountNumber");
                mandate.PayerBvn = GetString(csvRecord, "PayerBvn");
                mandate.BanksAccountNumber = GetString(csvRecord, "BanksAccountNumber");
                mandate.BanksAccountName = GetString(csvRecord, "BanksAccountName");
                mandate.BanksBvn = GetString(csvRecord, "BanksBvn");
                mandate.DestinationInstitutionCode = GetString(csvRecord, "DestinationInstitutionCode");
                mandate.SourceInstitutionCode = GetString(csvRecord, "SourceInstitutionCode");
                mandate.Narration = GetString(csvRecord, "Narration");
                mandate.MandateType = GetInt(csvRecord, "MandateType");
                mandate.StartDate = GetDateOnly(csvRecord, "StartDate");
                mandate.EndDate = GetDateOnly(csvRecord, "EndDate");
                mandate.PaymentFrequency = GetEnum<PaymentFrequency>(csvRecord, "PaymentFrequency", PaymentFrequency.Monthly);
                mandate.Location = GetString(csvRecord, "Location");
                mandate.TakeCharge = GetBool(csvRecord, "TakeCharge");
                mandate.CreatedById = GetLong(csvRecord, "CreatedById");
                mandate.CreatedBy = GetString(csvRecord, "CreatedById");
                mandate.DateCreated = GetDateOnly(csvRecord, "DateCreated");
                mandate.TimeCreated = GetDateTimeOffset(csvRecord, "DateCreated", "TimeCreated");
                mandate.LastModifiedBy = GetString(csvRecord, "LastModifiedBy");
                mandate.LastModifiedDate = GetNullableDateOnly(csvRecord, "LastModifiedDate");
                mandate.LastModifiedTime = GetNullableDateTimeOffset(csvRecord, "LastModifiedDate", "LastModifiedTime");
                mandate.ApprovedBy = GetString(csvRecord, "ApprovedBy");
                mandate.DateApproved = GetNullableDateOnly(csvRecord, "DateApproved");
                mandate.TimeApproved = CombineDateAndTime(csvRecord, "DateApproved", "TimeApproved");
                mandate.Status = GetString(csvRecord, "Status");
                mandate.HashValue = GetString(csvRecord, "HashValue");
                mandate.DeletedFlag = GetBool(csvRecord, "DeletedFlag");
                mandate.DeletedBy = GetString(csvRecord, "DeletedBy");
                mandate.IsDeleted = GetBool(csvRecord, "IsDeleted");
                mandate.DateDeleted = GetNullableDateOnly(csvRecord, "DateDeleted");
                mandate.TimeDeleted = CombineDateAndTime(csvRecord, "DateDeleted", "TimeDeleted");

                _context.Mandates.Add(mandate);
                var mandateRequest = mapper.Map<MandateRequest>(mandate);
                _context.MandateRequests.Add(mandateRequest);
                await _context.SaveChangesAsync(stoppingToken); // Needed to get MandateId

                var schedule = new MandateSchedule(
                    mandate.MandateId,
                    mandate.MandateReference,
                    mandate.NibbsMandateCode!,
                    mandate.WorkflowStatus ?? WorkflowStatus.MANDATE_APPROVED_BY_BANK,
                    mandate.StartDate,
                    mandate.EndDate,
                    mandate.PaymentFrequency
                );

                _context.MandateSchedules.Add(schedule);
            }

            await _context.SaveChangesAsync(stoppingToken);
            await transaction.CommitAsync(stoppingToken);
        }

        private static string GetString(dynamic record, string fieldName)
        {
            try
            {
                var dict = (IDictionary<string, object>)record;
                return dict.ContainsKey(fieldName) && dict[fieldName] != null
                    ? dict[fieldName].ToString() ?? string.Empty
                    : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        private static int GetInt(dynamic record, string fieldName)
        {
            var value = GetString(record, fieldName);
            return int.TryParse(value, out int result) ? result : 0;
        }

        private static long GetLong(dynamic record, string fieldName)
        {
            var value = GetString(record, fieldName);
            return long.TryParse(value, out long result) ? result : 0;
        }

        private static decimal GetDecimal(dynamic record, string fieldName)
        {
            var value = GetString(record, fieldName);
            return decimal.TryParse(value, out decimal result) ? result : 0m;
        }

        private static bool GetBool(dynamic record, string fieldName)
        {
            var value = GetString(record, fieldName);
            return bool.TryParse(value, out bool result) && result;
        }

        private static DateOnly GetDateOnly(dynamic record, string fieldName)
        {
            var value = GetString(record, fieldName);
            return DateOnly.TryParse(value, out DateOnly result) ? result : DateOnly.FromDateTime(DateTime.UtcNow);
        }

        private static DateOnly? GetNullableDateOnly(dynamic record, string fieldName)
        {
            var value = GetString(record, fieldName);
            return DateOnly.TryParse(value, out DateOnly result) ? result : null;
        }

        private static DateTimeOffset GetDateTimeOffset(dynamic record, string dateField, string timeField)
        {
            var dateStr = GetString(record, dateField);
            var timeStr = GetString(record, timeField);

            if (string.IsNullOrWhiteSpace(dateStr))
                return DateTimeOffset.UtcNow;

            if (DateTime.TryParse(dateStr, out DateTime date))
            {
                if (!string.IsNullOrWhiteSpace(timeStr))
                {
                    if (TimeSpan.TryParse(timeStr, out TimeSpan time))
                    {
                        return new DateTimeOffset(date.Add(time));
                    }
                }
                return new DateTimeOffset(date);
            }
            return DateTimeOffset.UtcNow;
        }
        private static DateTimeOffset? GetNullableDateTimeOffset(dynamic record, string dateField, string timeField)
        {
            var dateStr = GetString(record, dateField);
            var timeStr = GetString(record, timeField);

            if (string.IsNullOrWhiteSpace(dateStr))
                return null;

            if (DateTime.TryParse(dateStr, out DateTime date))
            {
                if (!string.IsNullOrWhiteSpace(timeStr))
                {
                    if (TimeSpan.TryParse(timeStr, out TimeSpan time))
                    {
                        return new DateTimeOffset(date.Add(time));
                    }
                }
                return new DateTimeOffset(date);
            }

            return null;
        }

        public string GenerateMandateRefernce()
        {
            var random = new Random();
            return $"MND-{DateTime.UtcNow:yyyyMMddHHmmss}-{random.Next(10, 1000000)}";
        }

        private static T GetEnum<T>(dynamic record, string fieldName, T defaultValue) where T : struct, Enum
        {
            var value = GetString(record, fieldName);
            return Enum.TryParse<T>(value, true, out T result) ? result : defaultValue;
        }

        private static DateTime? CombineDateAndTime(dynamic record, string dateField, string timeField)
        {
            var dateStr = GetString(record, dateField);
            var timeStr = GetString(record, timeField);

            if (string.IsNullOrWhiteSpace(dateStr))
                return null;

            if (DateTime.TryParse(dateStr, out DateTime date))
            {
                if (!string.IsNullOrWhiteSpace(timeStr))
                {
                    TimeSpan time;
                    if (TimeSpan.TryParse(timeStr, out time))
                    {
                        return date.Add(time);
                    }
                }
                return date;
            }

            return null;
        }
    }
}