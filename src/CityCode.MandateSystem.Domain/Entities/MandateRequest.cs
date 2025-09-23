using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace CityCode.MandateSystem.Domain.Entities
{
    public class MandateRequest : BaseAuditableEntity
    {
        public long MandateRequestId { get; set; }
        public string MandateReference { get; set; } = string.Empty;
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
        public PaymentFrequency PaymentFrequency { get; set; } = PaymentFrequency.Monthly;
        public string Location { get; set; } = string.Empty;
        public bool TakeCharge { get; set; } = false;

        public void GenerateMandateRefernce()
        {
            MandateReference = $"MND-{DateTime.UtcNow:yyyyMMddHHmmss}-{MandateRequestId}";
        }
        public void UpdateMandateRequestStatus(MandateRequestStatus status)
        {
            MandateRequestStatus = status;
        }

        public void SetBillerAndProductDetails(int productId, int billerId)
        {
            ProductId = productId;
            BillerId = billerId;
        }

        public void SetInitiatorDetails(string initiatedBy, long initiatedById)
        {
            CreatedBy = initiatedBy;
            CreatedById = initiatedById;
        }
    }
}