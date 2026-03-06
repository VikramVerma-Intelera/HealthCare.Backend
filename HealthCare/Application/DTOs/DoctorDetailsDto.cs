namespace HealthCare.Application.DTOs;

public class DoctorDetailsDto
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public string QualificationDetails { get; set; } = string.Empty;
    public string Experience { get; set; } = string.Empty;
    public decimal ConsultationFee { get; set; }
    public string ConsultationDuration { get; set; } = string.Empty;
    public string? ClinicAddress { get; set; }
    public string? Bio { get; set; }
}

public class CreateDoctorDetailsDto
{
    public int DoctorId { get; set; }
    public string QualificationDetails { get; set; } = string.Empty;
    public string Experience { get; set; } = string.Empty;
    public decimal ConsultationFee { get; set; }
    public string ConsultationDuration { get; set; } = string.Empty;
    public string? ClinicAddress { get; set; }
    public string? Bio { get; set; }
}

public class UpdateDoctorDetailsDto
{
    public string QualificationDetails { get; set; } = string.Empty;
    public string Experience { get; set; } = string.Empty;
    public decimal ConsultationFee { get; set; }
    public string ConsultationDuration { get; set; } = string.Empty;
    public string? ClinicAddress { get; set; }
    public string? Bio { get; set; }
}
