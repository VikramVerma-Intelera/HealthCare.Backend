namespace HealthCare.Application.DTOs;

public class DoctorDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public string Speciality { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class CreateDoctorDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public string Speciality { get; set; } = string.Empty;
}

public class UpdateDoctorDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public string Speciality { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
