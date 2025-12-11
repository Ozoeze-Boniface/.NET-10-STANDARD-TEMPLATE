namespace CityCode.MandateSystem.Domain.Entities
{
    public class MandateTransaction : BaseAuditableEntity
    {
        public long MandateTransactionId { get; set; }
        public long MandateScheduleId { get; set; }
        public long MandateId { get; set; }
        public string TransactionReference { get; set; } = default!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = default!;
        public DateOnly TransactionDate { get; set; }
        public DateTime? TransactionTime { get; set; }
        public string? TransactionId { get; set; }
        public string TransactionStatus { get; set; }
        public string? StatusMessage { get; set; }
        public MandateSchedule MandateSchedule { get; set; } = default!;

        // From nibbs response payload
        public string ResponseCode { get; set; } = default!;
        public string SessionId { get; set; } = default!;
        public int ChannelCode { get; set; } = default!;
        public string NameEnquiryRef { get; set; } = default!;
        public string DestinationInstitutionCode { get; set; } = default!;

        // Beneficiary
        public string BeneficiaryAccountName { get; set; } = default!;
        public string BeneficiaryAccountNumber { get; set; } = default!;
        public string BeneficiaryKYCLevel { get; set; } = default!;
        public string BeneficiaryBankVerificationNumber { get; set; } = default!;

        // Originator
        public string OriginatorAccountName { get; set; } = default!;
        public string OriginatorAccountNumber { get; set; } = default!;
        public string OriginatorBankVerificationNumber { get; set; } = default!;
        public string OriginatorKYCLevel { get; set; } = default!;

        // Transaction details
        public string TransactionLocation { get; set; } = default!;
        public string Narration { get; set; } = default!;
        public string PaymentReference { get; set; } = default!;

        public MandateTransaction(
            long mandateScheduleId,
            string transactionReference,
            decimal amount,
            string currency,
            DateOnly transactionDate,
            string transactionStatus,
            long mandateId = 0)
        {
            MandateScheduleId = mandateScheduleId;
            TransactionReference = transactionReference;
            Amount = amount;
            Currency = currency;
            TransactionDate = transactionDate;
            TransactionStatus = transactionStatus;
            MandateId = mandateId;
        }

        /// <summary>
        /// Updates properties from the API response
        /// </summary>
        public void UpdateFromResponse(
            string responseCode,
            string sessionId,
            int channelCode,
            string nameEnquiryRef,
            string destinationInstitutionCode,
            string beneficiaryAccountName,
            string beneficiaryAccountNumber,
            string beneficiaryKYCLevel,
            string beneficiaryBankVerificationNumber,
            string originatorAccountName,
            string originatorAccountNumber,
            string originatorBankVerificationNumber,
            string originatorKYCLevel,
            string transactionLocation,
            string narration,
            string paymentReference)
        {
            ResponseCode = responseCode;
            SessionId = sessionId;
            ChannelCode = channelCode;
            NameEnquiryRef = nameEnquiryRef;
            DestinationInstitutionCode = destinationInstitutionCode;

            BeneficiaryAccountName = beneficiaryAccountName;
            BeneficiaryAccountNumber = beneficiaryAccountNumber;
            BeneficiaryKYCLevel = beneficiaryKYCLevel;
            BeneficiaryBankVerificationNumber = beneficiaryBankVerificationNumber;

            OriginatorAccountName = originatorAccountName;
            OriginatorAccountNumber = originatorAccountNumber;
            OriginatorBankVerificationNumber = originatorBankVerificationNumber;
            OriginatorKYCLevel = originatorKYCLevel;

            TransactionLocation = transactionLocation;
            Narration = narration;
            PaymentReference = paymentReference;
        }


        /// <summary>
        /// Updates transaction status details
        /// </summary>
        public void UpdateStatus(string status, string? message = null, string? transactionId = null)
        {
            TransactionStatus = status;
            StatusMessage = message;
            TransactionId = transactionId;
        }
    }

}