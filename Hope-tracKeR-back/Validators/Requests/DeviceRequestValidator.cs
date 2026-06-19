using FluentValidation;
using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Enums;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Microsoft.EntityFrameworkCore;

namespace Hope_tracKeR_back.Validators.Requests;

public class DeviceRequestValidator : AbstractValidator<DeviceRequest>
{
    public DeviceRequestValidator(HTContext context)
    {
        RuleFor(d => d.Name)
            .NotEmpty().WithMessage("Название не может быть пустым!");
        RuleFor(d => d.Status)
            .NotEmpty().WithMessage("Статус не может быть пустым!")
            .IsEnumName(typeof(DeviceStatus)).WithMessage("Некорректный статус!");
        RuleFor(d => d.AddedDate)
            .NotEmpty().WithMessage("Дата не может быть пустым!");
        RuleFor(d => d.AddressId)
            .NotEmpty().WithMessage("Адрес не может быть пустым!")
            .MustAsync(async (id, ct) => await context.Addresses.AnyAsync(a => a.Id == id, ct))
            .WithMessage("Адрес не найден!");
        RuleFor(d => d.BrandId)
            .NotEmpty().WithMessage("Бренд не может быть пустым!")
            .MustAsync(async (id, ct) => await context.Brands.AnyAsync(a => a.Id == id, ct))
            .WithMessage("Бренд не найден!");
        RuleFor(d => d.CategoryId)
            .NotEmpty().WithMessage("Категория не может быть пустым!")
            .MustAsync(async (id, ct) => await context.Categories.AnyAsync(a => a.Id == id, ct))
            .WithMessage("Категория не найдена!");
        RuleFor(d => d.EmployeeId)
            .MustAsync(async (id, ct) => id is null || await context.Employees.AnyAsync(e => e.Id == id, ct))
            .WithMessage("Сотрудник не найден!");
    }
}