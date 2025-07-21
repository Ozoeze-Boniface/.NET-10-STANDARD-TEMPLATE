using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityCode.MandateSystem.Application.Common.Models;
using CityCode.MandateSystem.Application.Common.Models.View;
using CityCode.MandateSystem.Domain.Enums;

namespace CityCode.MandateSystem.Application.Query
{
    public class GetMandateRequestQuery : IRequest<Common.Models.View.Result<PaginatedList<MandateRequest>>>
    {
        public int PageNumber { get; set; } = 1; // Default to first page
        public int PageSize { get; set; } = 10;  // Default to 10 items per page
        public string? SearchTerm { get; set; } // Optional search term for filtering
        public string? SearchField { get; set; } // Optional field to search against
        public long? MandateRequestId { get; set; }
        public string? MandateReference { get; set; }
        public int? ProductId { get; set; }
        public int? BillerId { get; set; }
        public string? SubscriberCode { get; set; }
        public decimal? ProductTotalAmount { get; set; }
        public decimal? TransactionAmount { get; set; }
        public string? BankCode { get; set; }
        public string? PayerName { get; set; }
        public string? PayerAddress { get; set; }
        public string? PayerEmail { get; set; }
        public string? PayerPhoneNumber { get; set; }
        public string? AccountName { get; set; }
        public string? PayerAccountNumber { get; set; }
        public string? PayerBvn { get; set; }
        public string? BanksAccountNumber { get; set; }
        public string? BanksAccountName { get; set; }
        public string? BanksBvn { get; set; }
        public string? DestinationInstitutionCode { get; set; }
        public string? SourceInstitutionCode { get; set; }
        public string? Narration { get; set; }
        public int? MandateType { get; set; }
        public MandateRequestStatus? MandateRequestStatus { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public PaymentFrequency? PaymentFrequency { get; set; }
        public string? Location { get; set; }
    }
}