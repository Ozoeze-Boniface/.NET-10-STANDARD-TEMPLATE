using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityCode.MandateSystem.Application.Common.Models.View;
using CityCode.MandateSystem.Domain.Enums;

namespace CityCode.MandateSystem.Application.Commands
{
    public class CreateMandateCommand : IRequest<Common.Models.View.Result<MandateRequest>>
    {
        public string InitiatedBy { get; set; } = string.Empty;
        public long InitiatedById { get; set; }
        public int ProductId { get; set; }
        public int BillerId { get; set; }
        public string SubscriberCode { get; set; } = string.Empty;
        public decimal ProductTotalAmount { get; set; }
        public decimal TransactionAmount { get; set; }
        public string BankCode { get; set; } = string.Empty;
        public string PayerName { get; set; } = string.Empty;
        public string PayerAddress { get; set; } = string.Empty;
        public string PayerEmail { get; set; } = string.Empty;
        public string PayerPhoneNumber { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public string PayerAccountNumber { get; set; } = string.Empty;
        public string PayerBvn { get; set; } = string.Empty;
        public string BanksAccountNumber { get; set; } = string.Empty;
        public string BanksAccountName { get; set; } = string.Empty;
        public string BanksBvn { get; set; } = string.Empty;
        public string DestinationInstitutionCode { get; set; } = string.Empty;
        public string SourceInstitutionCode { get; set; } = string.Empty;
        public string Narration { get; set; } = string.Empty;
        public int MandateType { get; set; }
        public MandateRequestStatus MandateRequestStatus { get; set; } = MandateRequestStatus.IN_REVIEW;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Location { get; set; } = string.Empty;
    }
}