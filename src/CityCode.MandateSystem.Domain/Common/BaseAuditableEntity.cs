namespace CityCode.MandateSystem.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public string? CreatedBy { get; set; } = string.Empty;
    public long? CreatedById { get; set; }
    public DateOnly DateCreated { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public DateTimeOffset TimeCreated { get; set; } = DateTimeOffset.UtcNow;
    public virtual string? LastModifiedBy { get; set; }
    public DateOnly? LastModifiedDate { get; set; }
    public DateTimeOffset? LastModifiedTime { get; set; }
    public string? ApprovedBy { get; set; }
    public DateOnly? DateApproved { get; set; }
    public DateTime? TimeApproved { get; set; }
    public string? Status { get; set; }
    public string? HashValue { get; set; }
    public bool DeletedFlag { get; set; } = false;
    public string? DeletedBy { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;
    public DateOnly? DateDeleted { get; set; }
    public DateTime? TimeDeleted { get; set; }
    public ulong? RowVersion { get; set; }

}
