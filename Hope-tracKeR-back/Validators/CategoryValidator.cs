using FluentValidation;
using Hope_tracKeR_back.Models.Entities;

namespace Hope_tracKeR_back.Validators;

public class CategoryValidator : AbstractValidator<Category>
{
    public CategoryValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Имя категории не может быть пустым!");
    }
}