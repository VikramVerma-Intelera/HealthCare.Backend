using FluentValidation;
using HealthCare.Application.DTOs;

namespace HealthCare.Application.Validators;

public class CreateAppointmentValidator : AbstractValidator<CreateAppointmentDto>
{
    public CreateAppointmentValidator()
    {
        RuleFor(x => x.PatientId)
            .GreaterThan(0).WithMessage("Patient ID must be greater than 0.");

        RuleFor(x => x.DoctorId)
            .GreaterThan(0).WithMessage("Doctor ID must be greater than 0.");

        RuleFor(x => x.AppointmentTypeId)
            .GreaterThan(0).WithMessage("Appointment Type ID must be greater than 0.");

        RuleFor(x => x.AppointmentDate)
            .NotEmpty().WithMessage("Appointment date is required.")
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("Appointment date cannot be in the past.");

        RuleFor(x => x.AppointmentTime)
            .NotEmpty().WithMessage("Appointment time is required.");
    }
}

public class UpdateAppointmentValidator : AbstractValidator<UpdateAppointmentDto>
{
    public UpdateAppointmentValidator()
    {
        RuleFor(x => x.PatientId)
            .GreaterThan(0).WithMessage("Patient ID must be greater than 0.");

        RuleFor(x => x.DoctorId)
            .GreaterThan(0).WithMessage("Doctor ID must be greater than 0.");

        RuleFor(x => x.AppointmentTypeId)
            .GreaterThan(0).WithMessage("Appointment Type ID must be greater than 0.");

        RuleFor(x => x.AppointmentDate)
            .NotEmpty().WithMessage("Appointment date is required.")
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("Appointment date cannot be in the past.");

        RuleFor(x => x.AppointmentTime)
            .NotEmpty().WithMessage("Appointment time is required.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required.")
            .Must(x => x == "Scheduled" || x == "Confirmed" || x == "Completed" || x == "Cancelled")
            .WithMessage("Status must be Scheduled, Confirmed, Completed, or Cancelled.");
    }
}
