namespace HealthCare.Data.Models;

public abstract class BaseAuditEntity
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? ModifiedOn { get; set; }
    public string? ModifiedBy { get; set; }
}
