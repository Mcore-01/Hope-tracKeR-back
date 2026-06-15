using FluentValidation;
using Hope_tracKeR_back.Models.Entities;

namespace Hope_tracKeR_back.Validators;

public class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        RuleFor(a => a.Branch)
            .NotEmpty().WithMessage("Название филиала не может быть пустым!");
        RuleFor(a => a.Building)
            .NotEmpty().WithMessage("Название корпуса не может быть пустым!");
        RuleFor(a => a.Floor)
            .GreaterThan(0).WithMessage("Номер этажа не может быть меньше 1!");
        RuleFor(a => a.Room)
            .NotEmpty().WithMessage("Название комнаты не может быть пустым!");
    }
}