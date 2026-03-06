namespace HealthCare.Application.DTOs;

public class AppointmentTypeDto
{
    public int Id { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public string TypeCode { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DurationInMinutes { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
}

public class CreateAppointmentTypeDto
{
    public string TypeName { get; set; } = string.Empty;
    public string TypeCode { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DurationInMinutes { get; set; }
    public decimal Price { get; set; }
}

public class UpdateAppointmentTypeDto
{
    public string TypeName { get; set; } = string.Empty;
    public string TypeCode { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DurationInMinutes { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
}
