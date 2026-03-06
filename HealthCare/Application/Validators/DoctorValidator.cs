using FluentValidation;
using HealthCare.Application.DTOs;

namespace HealthCare.Application.Validators;

public class CreateDoctorValidator : AbstractValidator<CreateDoctorDto>
{
    public CreateDoctorValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\d{3}-\d{4}$|^\d{10}$|^\+\d{1,3}\d{9,14}$")
            .WithMessage("Phone number must be in a valid format.");

        RuleFor(x => x.LicenseNumber)
            .NotEmpty().WithMessage("License number is required.")
            .MaximumLength(20).WithMessage("License number cannot exceed 20 characters.");

        RuleFor(x => x.DepartmentId)
            .GreaterThan(0).WithMessage("Department ID must be greater than 0.");

        RuleFor(x => x.Speciality)
            .NotEmpty().WithMessage("Speciality is required.")
            .MaximumLength(100).WithMessage("Speciality cannot exceed 100 characters.");
    }
}

public class UpdateDoctorValidator : AbstractValidator<UpdateDoctorDto>
{
    public UpdateDoctorValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\d{3}-\d{4}$|^\d{10}$|^\+\d{1,3}\d{9,14}$")
            .WithMessage("Phone number must be in a valid format.");

        RuleFor(x => x.LicenseNumber)
            .NotEmpty().WithMessage("License number is required.")
            .MaximumLength(20).WithMessage("License number cannot exceed 20 characters.");

        RuleFor(x => x.DepartmentId)
            .GreaterThan(0).WithMessage("Department ID must be greater than 0.");

        RuleFor(x => x.Speciality)
            .NotEmpty().WithMessage("Speciality is required.")
            .MaximumLength(100).WithMessage("Speciality cannot exceed 100 characters.");
    }
}
