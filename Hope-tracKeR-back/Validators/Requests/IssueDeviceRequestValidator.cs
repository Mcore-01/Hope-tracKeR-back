using FluentValidation;
using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Enums;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Microsoft.EntityFrameworkCore;

namespace Hope_tracKeR_back.Validators.Requests;

public class IssueDeviceRequestValidator : AbstractValidator<IssueDeviceRequest>
{
    public IssueDeviceRequestValidator(HTContext context)
    {
        RuleFor(r => r.ItemId)
            .NotEmpty().WithMessage("Пустой идентификатор предмета!")
            .MustAsync(async (id, ct) => await context.Devices.AnyAsync(d => d.Id == id, ct))
            .WithMessage("Предмет не найден!")
            .MustAsync(async (id, ct) => await context.Devices.AnyAsync(d => d.Id == id && d.Status == DeviceStatus.InStock, ct))
            .WithMessage("Предмета нет в наличии!");

        RuleFor(r => r.EmployeeId)
            .NotEmpty().WithMessage("Пустой идентификатор сотрудника!")
            .MustAsync(async (id, ct) => await context.Employees.AnyAsync(e => e.Id == id, ct))
            .WithMessage("Сотрудник не найден!");

        RuleFor(r => r.UserId)
            .NotEmpty().WithMessage("Пользователь не авторизован!")
            .MustAsync(async (id, ct) => await context.Users.AnyAsync(u => u.Id == id, ct))
            .WithMessage("Пользователь не найден!");
    }
}
