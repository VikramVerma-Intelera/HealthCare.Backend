namespace HealthCare.Data.Models;

public class Patient : BaseAuditEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public PatientDetails? PatientDetails { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
