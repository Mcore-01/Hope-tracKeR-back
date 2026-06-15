using FluentValidation;
using Hope_tracKeR_back.Models.Entities;

namespace Hope_tracKeR_back.Validators;

public class BrandValidator : AbstractValidator<Brand>
{
    public BrandValidator()
    {
        RuleFor(b => b.Name)
            .NotEmpty().WithMessage("Название бренда не может быть пустым!");
    }
}