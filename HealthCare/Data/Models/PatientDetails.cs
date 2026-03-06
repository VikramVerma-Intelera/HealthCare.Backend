namespace HealthCare.Data.Models;

public class PatientDetails : BaseAuditEntity
{
    public int PatientId { get; set; }
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string BloodType { get; set; } = string.Empty;
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? AllergiesDescription { get; set; }
    public string? MedicalHistoryNotes { get; set; }

    public Patient Patient { get; set; } = null!;
}
