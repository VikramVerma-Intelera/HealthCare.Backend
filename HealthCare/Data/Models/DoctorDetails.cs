namespace HealthCare.Data.Models;

public class DoctorDetails : BaseAuditEntity
{
    public int DoctorId { get; set; }
    public string QualificationDetails { get; set; } = string.Empty;
    public string Experience { get; set; } = string.Empty;
    public decimal ConsultationFee { get; set; }
    public string ConsultationDuration { get; set; } = string.Empty;
    public string? ClinicAddress { get; set; }
    public string? Bio { get; set; }

    public Doctor Doctor { get; set; } = null!;
}
