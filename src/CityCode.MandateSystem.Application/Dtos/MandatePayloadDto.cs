using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CityCode.MandateSystem.Application.Dtos
{
    public class MandatePayloadDto
    {
        public int ProductId { get; set; }
        public int BillerId { get; set; }
        public string? AccountNumber { get; set; }
        public string? BankCode { get; set; }
        public string? PayerName { get; set; }
        public int MandateType { get; set; }
        public string? PayerAddress { get; set; }
        public string? AccountName { get; set; }
        public decimal Amount { get; set; }
        public int Frequency { get; set; }
        public string? Narration { get; set; }
        public string? PhoneNumber { get; set; }
        public string? SubscriberCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? PayerEmail { get; set; }
    }

    public class MandateCreationResponse
    {
        [JsonPropertyName("responseMessage")]
        public string? ResponseMessage { get; set; }

        [JsonPropertyName("responseCode")]
        public string? ResponseCode { get; set; }

        [JsonPropertyName("data")]
        public MandateData? Data { get; set; }
    }

    public class MandateData
    {
        [JsonPropertyName("mandateCode")]
        public string? MandateCode { get; set; }

        [JsonPropertyName("subscriberCode")]
        public string? SubscriberCode { get; set; }

        [JsonPropertyName("phoneNumber")]
        public string? PhoneNumber { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("accountName")]
        public string? AccountName { get; set; }

        [JsonPropertyName("accountNumber")]
        public string? AccountNumber { get; set; }

        [JsonPropertyName("mandateStatus")]
        public string? MandateStatus { get; set; }

        [JsonPropertyName("workflowStatus")]
        public string? WorkflowStatus { get; set; }

        [JsonPropertyName("rejectionReason")]
        public string? RejectionReason { get; set; }

        [JsonPropertyName("rejectionComment")]
        public string? RejectionComment { get; set; }

        [JsonPropertyName("mandateAdviceStatus")]
        public string? MandateAdviceStatus { get; set; }

        [JsonPropertyName("mandateAdviceSent")]
        public int MandateAdviceSent { get; set; }
    }

    public class MandateTransactionResponse
    {
        public string ResponseCode { get; set; } = string.Empty;
        public string SessionID { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
        public int ChannelCode { get; set; }
        public string NameEnquiryRef { get; set; } = string.Empty;
        public string DestinationInstitutionCode { get; set; } = string.Empty;
        public string BeneficiaryAccountName { get; set; } = string.Empty;
        public string BeneficiaryAccountNumber { get; set; } = string.Empty;
        public string BeneficiaryKYCLevel { get; set; } = string.Empty;
        public string BeneficiaryBankVerificationNumber { get; set; } = string.Empty;
        public string OriginatorAccountName { get; set; } = string.Empty;
        public string OriginatorAccountNumber { get; set; } = string.Empty;
        public string OriginatorBankVerificationNumber { get; set; } = string.Empty;
        public string OriginatorKYCLevel { get; set; } = string.Empty;
        public string TransactionLocation { get; set; } = string.Empty;
        public string Narration { get; set; } = string.Empty;
        public string PaymentReference { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}