namespace HealthCare.Data.Models;

public class Appointment : BaseAuditEntity
{
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public int AppointmentTypeId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public DateTime AppointmentTime { get; set; }
    public string Status { get; set; } = "Scheduled";
    public string? Notes { get; set; }
    public string? Location { get; set; }

    public Patient Patient { get; set; } = null!;
    public Doctor Doctor { get; set; } = null!;
    public AppointmentType AppointmentType { get; set; } = null!;
}
