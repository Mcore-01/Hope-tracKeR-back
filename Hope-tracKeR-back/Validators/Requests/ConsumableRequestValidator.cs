using FluentValidation;
using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Microsoft.EntityFrameworkCore;

namespace Hope_tracKeR_back.Validators.Requests;

public class ConsumableRequestValidator : AbstractValidator<ConsumableRequest>
{
    public ConsumableRequestValidator(HTContext context)
    {
        RuleFor(d => d.Name)
            .NotEmpty().WithMessage("Название не может быть пустым!");
        RuleFor(d => d.Quantity)
            .NotEmpty().WithMessage("Количество не может быть пустым!")
            .GreaterThan(0).WithMessage("Количество должно быть больше 0!");
        RuleFor(d => d.AddressId)
            .NotEmpty().WithMessage("Адрес не может быть пустым!")
            .MustAsync(async (id, ct) => await context.Addresses.AnyAsync(a => a.Id == id, ct))
            .WithMessage("Адрес не найден!");
        RuleFor(d => d.BrandId)
            .NotEmpty().WithMessage("Бренд не может быть пустым!")
            .MustAsync(async (id, ct) => await context.Brands.AnyAsync(a => a.Id == id, ct))
            .WithMessage("Бренд не найден!");
    }
}