using FluentValidation;
using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Enums;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Microsoft.EntityFrameworkCore;

namespace Hope_tracKeR_back.Validators.Requests;

public class CartridgeRequestValidator : AbstractValidator<CartridgeRequest>
{
    public CartridgeRequestValidator(HTContext context)
    {
        RuleFor(d => d.Name)
            .NotEmpty().WithMessage("Название не может быть пустым!");
        RuleFor(d => d.PrinterModel)
            .NotEmpty().WithMessage("Модель принтера не может быть пустым!");
        RuleFor(d => d.Status)
            .NotEmpty().WithMessage("Статус не может быть пустым!")
            .IsEnumName(typeof(CartridgeStatus)).WithMessage("Некорректный статус!");
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