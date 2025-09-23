using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityCode.MandateSystem.Domain.Entities
{
    public class Mandate : BaseAuditableEntity
    {
        public long MandateId { get; set; }
        public string MandateReference { get; set; } = string.Empty;
        public string? NibbsMandateCode { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public int BillerId { get; set; }
        public WorkflowStatus? WorkflowStatus { get; set; }
        public MandateStatus MandateStatus { get; set; } = MandateStatus.INACTIVE;
        public ProgressStatus ProgressStatus { get; set; }
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
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public PaymentFrequency PaymentFrequency { get; set; } = PaymentFrequency.Monthly;
        public string Location { get; set; } = string.Empty;
        public bool TakeCharge { get; set; }

        public void UpdateMandateStatus(MandateStatus status)
        {
            MandateStatus = status;
        }

        public void UpdateWorkflow(WorkflowStatus status)
        {
            WorkflowStatus = status;
        }

        public void UpdateApproverDetails(long approverId, string approvedBy)
        {
            CreatedById = approverId;
            CreatedBy = approvedBy;
        }
        public void SetProgressStatus(ProgressStatus status)
        {
            ProgressStatus = status;
        }

        public void UpdateNibbsMandateCode(string nibbsCode)
        {
            NibbsMandateCode = nibbsCode;
        }
    }
}