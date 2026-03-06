namespace HealthCare.Data.Models;

public class Doctor : BaseAuditEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public string Speciality { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public Department Department { get; set; } = null!;
    public DoctorDetails? DoctorDetails { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
