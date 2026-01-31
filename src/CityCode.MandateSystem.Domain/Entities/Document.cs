namespace CityCode.MandateSystem.Domain.Entities
{
    public class Document : BaseAuditableEntity
    {
        public long DocumentId { get; set; }
        public string? MandateReference { get; set; }
        public string DocumentName { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public byte[] FileData { get; set; } = null!;
    }
}
