namespace HealthCare.Application.DTOs;

public class PatientDetailsDto
{
    public int Id { get; set; }
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
}

public class CreatePatientDetailsDto
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
}

public class UpdatePatientDetailsDto
{
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string BloodType { get; set; } = string.Empty;
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? AllergiesDescription { get; set; }
    public string? MedicalHistoryNotes { get; set; }
}
