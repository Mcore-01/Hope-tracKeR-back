using FluentValidation;
using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Enums;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Microsoft.EntityFrameworkCore;

namespace Hope_tracKeR_back.Validators.Requests;

public class StartRefillRequestValidator : AbstractValidator<StartRefillRequest>
{
    public StartRefillRequestValidator(HTContext context)
    {
        RuleFor(s => s.ItemId)
            .NotEmpty().WithMessage("Пустой идентификатор предмета!")
            .MustAsync(async (id, ct) => await context.Cartridges.AnyAsync(a => a.Id == id, ct))
            .WithMessage($"Предмет не найден!")
            .MustAsync(async (id, ct) => await context.Cartridges.AnyAsync(c => c.Id == id && c.Status != CartridgeStatus.Refilling, ct))
            .WithMessage("Картридж уже в процессе заправки!");
        RuleFor(s => s.AddressId)
           .NotEmpty().WithMessage("Пустой идентификатор адреса!")
           .MustAsync(async (id, ct) => await context.Addresses.AnyAsync(a => a.Id == id, ct))
           .WithMessage($"Адрес не найден!");
        RuleFor(s => s.UserId)
           .NotEmpty().WithMessage("Пользователь не авторизован!")
           .MustAsync(async (id, ct) => await context.Users.AnyAsync(a => a.Id == id, ct))
           .WithMessage($"Пользователь не найден!");
    }
}