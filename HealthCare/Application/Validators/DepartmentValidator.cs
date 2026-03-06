using FluentValidation;
using HealthCare.Application.DTOs;

namespace HealthCare.Application.Validators;

public class CreateDepartmentValidator : AbstractValidator<CreateDepartmentDto>
{
    public CreateDepartmentValidator()
    {
        RuleFor(x => x.DepartmentName)
            .NotEmpty().WithMessage("Department name is required.")
            .MaximumLength(100).WithMessage("Department name cannot exceed 100 characters.");

        RuleFor(x => x.DepartmentCode)
            .NotEmpty().WithMessage("Department code is required.")
            .MaximumLength(10).WithMessage("Department code cannot exceed 10 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
    }
}

public class UpdateDepartmentValidator : AbstractValidator<UpdateDepartmentDto>
{
    public UpdateDepartmentValidator()
    {
        RuleFor(x => x.DepartmentName)
            .NotEmpty().WithMessage("Department name is required.")
            .MaximumLength(100).WithMessage("Department name cannot exceed 100 characters.");

        RuleFor(x => x.DepartmentCode)
            .NotEmpty().WithMessage("Department code is required.")
            .MaximumLength(10).WithMessage("Department code cannot exceed 10 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
    }
}
