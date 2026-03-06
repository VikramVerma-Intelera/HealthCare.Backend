namespace HealthCare.Application.DTOs;

public class AppointmentDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public int AppointmentTypeId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public DateTime AppointmentTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string? Location { get; set; }
}

public class CreateAppointmentDto
{
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public int AppointmentTypeId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public DateTime AppointmentTime { get; set; }
    public string? Notes { get; set; }
    public string? Location { get; set; }
}

public class UpdateAppointmentDto
{
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public int AppointmentTypeId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public DateTime AppointmentTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string? Location { get; set; }
}
