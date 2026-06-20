using FluentValidation;
using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Enums;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Microsoft.EntityFrameworkCore;

namespace Hope_tracKeR_back.Validators.Requests;

public class WriteOffDeviceRequestValidator : AbstractValidator<WriteOffDeviceRequest>
{
    public WriteOffDeviceRequestValidator(HTContext context)
    {
        RuleFor(r => r.ItemId)
            .NotEmpty().WithMessage("Пустой идентификатор предмета!")
            .MustAsync(async (id, ct) => await context.Devices.AnyAsync(d => d.Id == id, ct))
            .WithMessage("Предмет не найден!")
            .MustAsync(async (id, ct) => await context.Devices.AnyAsync(d => d.Id == id && d.Status != DeviceStatus.WriteOff, ct))
            .WithMessage("Предмет уже списан!");

        RuleFor(r => r.UserId)
            .NotEmpty().WithMessage("Пользователь не авторизован!")
            .MustAsync(async (id, ct) => await context.Users.AnyAsync(u => u.Id == id, ct))
            .WithMessage("Пользователь не найден!");
    }
}