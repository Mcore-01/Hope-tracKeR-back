using FluentValidation;
using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Microsoft.EntityFrameworkCore;

namespace Hope_tracKeR_back.Validators.Requests;

public class CompleteRefillRequestValidator : AbstractValidator<CompleteRefillRequest>
{
    public CompleteRefillRequestValidator(HTContext context) 
    {
        RuleFor(s => s.ItemId)
            .NotEmpty().WithMessage("Пустой идентификатор предмета!")
            .MustAsync(async (id, ct) => await context.Devices.AnyAsync(a => a.Id == id, ct))
            .WithMessage($"Предмет не найден!");
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