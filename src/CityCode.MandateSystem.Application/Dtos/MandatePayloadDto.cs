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
    }

}