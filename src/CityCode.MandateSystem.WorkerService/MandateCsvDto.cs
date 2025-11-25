namespace CityCode.MandateSystem.WorkerService;

using CsvHelper.Configuration.Attributes;

public class MandateCsvDto
{
    [Name("MandateId")]
    public string? MandateId { get; set; }

    [Name("MandateReference")]
    public string? MandateReference { get; set; }

    [Name("NibbsMandateCode")]
    public string? NibbsMandateCode { get; set; }

    [Name("ProductId")]
    public string? ProductId { get; set; }

    [Name("BillerId")]
    public string? BillerId { get; set; }

    [Name("WorkflowStatus")]
    public string? WorkflowStatus { get; set; }

    [Name("MandateStatus")]
    public string? MandateStatus { get; set; }

    [Name("ProgressStatus")]
    public string? ProgressStatus { get; set; }

    [Name("SubscriberCode")]
    public string? SubscriberCode { get; set; }

    // NOTE: header has spaces: " ProductTotalAmount "
    [Name(" ProductTotalAmount ")]
    public string? ProductTotalAmount { get; set; }

    [Name("TransactionAmount")]
    public string? TransactionAmount { get; set; }

    [Name("BankCode")]
    public string? BankCode { get; set; }

    [Name("PayerName")]
    public string? PayerName { get; set; }

    [Name("PayerAddress")]
    public string? PayerAddress { get; set; }

    [Name("PayerEmail")]
    public string? PayerEmail { get; set; }

    [Name("PayerPhoneNumber")]
    public string? PayerPhoneNumber { get; set; }

    [Name("AccountName")]
    public string? AccountName { get; set; }

    [Name("PayerAccountNumber")]
    public string? PayerAccountNumber { get; set; }

    [Name("PayerBvn")]
    public string? PayerBvn { get; set; }

    [Name("BanksAccountNumber")]
    public string? BanksAccountNumber { get; set; }

    [Name("BanksAccountName")]
    public string? BanksAccountName { get; set; }

    [Name("BanksBvn")]
    public string? BanksBvn { get; set; }

    [Name("DestinationInstitutionCode")]
    public string? DestinationInstitutionCode { get; set; }

    [Name("SourceInstitutionCode")]
    public string? SourceInstitutionCode { get; set; }

    [Name("Narration")]
    public string? Narration { get; set; }

    [Name("MandateType")]
    public string? MandateType { get; set; }

    [Name("StartDate")]
    public string? StartDate { get; set; }

    [Name("EndDate")]
    public string? EndDate { get; set; }

    [Name("Location")]
    public string? Location { get; set; }

    [Name("Column16336")]
    public string? Column16336 { get; set; }

    [Name("CreatedById")]
    public string? CreatedById { get; set; }

    [Name("DateCreated")]
    public string? DateCreated { get; set; }

    [Name("TimeCreated")]
    public string? TimeCreated { get; set; }

    [Name("LastModifiedBy")]
    public string? LastModifiedBy { get; set; }

    [Name("LastModifiedDate")]
    public string? LastModifiedDate { get; set; }

    [Name("LastModifiedTime")]
    public string? LastModifiedTime { get; set; }

    [Name("ApprovedBy")]
    public string? ApprovedBy { get; set; }

    [Name("DateApproved")]
    public string? DateApproved { get; set; }

    [Name("TimeApproved")]
    public string? TimeApproved { get; set; }

    [Name("Status")]
    public string? Status { get; set; }

    [Name("HashValue")]
    public string? HashValue { get; set; }

    [Name("DeletedFlag")]
    public string? DeletedFlag { get; set; }

    [Name("DeletedBy")]
    public string? DeletedBy { get; set; }

    [Name("IsDeleted")]
    public string? IsDeleted { get; set; }

    [Name("DateDeleted")]
    public string? DateDeleted { get; set; }

    [Name("TimeDeleted")]
    public string? TimeDeleted { get; set; }

    [Name("RowVersion")]
    public string? RowVersion { get; set; }

    [Name("PaymentFrequency")]
    public string? PaymentFrequency { get; set; }

    [Name("TakeCharge")]
    public string? TakeCharge { get; set; }
}
