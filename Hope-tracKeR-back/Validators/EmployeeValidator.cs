using FluentValidation;
using Hope_tracKeR_back.Models.Entities;

namespace Hope_tracKeR_back.Validators;

public class EmployeeValidator : AbstractValidator<Employee>
{
    public EmployeeValidator()
    {
        RuleFor(e => e.FullName)
            .NotEmpty().WithMessage("ФИО сотрудника не может быть пустым!");
        RuleFor(e => e.Staff)
            .NotEmpty().WithMessage("Должность сотрудника не может быть пустым!");
    }
}