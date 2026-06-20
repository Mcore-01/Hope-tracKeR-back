using FluentResults;
using Hope_tracKeR_back.Constants;
using Hope_tracKeR_back.Errors;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Hope_tracKeR_back.Services.Interfaces;

namespace Hope_tracKeR_back.Services;

public class InventoryService : IInventoryService
{
    private readonly IItemRepository<Consumable> _repository;
    private readonly IAuditLogService _auditLog;

    public InventoryService(IItemRepository<Consumable> repository, IAuditLogService auditLog)
    {
        _repository = repository;
        _auditLog = auditLog;
    }

    public async Task<Result> IncreaseQuantity(int id, int amount)
    {
        if (amount <= 0)
        {
            var message = "Количество должно быть строго больше нуля!";
            return Result.Fail(new ValidationError(message));
        }

        try
        {
            var item = await _repository.GetById(id);
            var oldQuantity = item.Quantity;

            item.Quantity += amount;
            await _repository.Update(item);

            await _auditLog.LogAsync(AuditActions.IncreaseQuantity, nameof(Consumable), id.ToString(), new { Quantity = item.Quantity, Added = amount });

            return Result.Ok();
        }
        catch (NullReferenceException ex)
        {
            return Result.Fail(new NotFoundError(ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error($"Произошла ошибка: {ex.Message}"));
        }
    }

    public async Task<Result> DecreaseQuantity(int id, int amount)
    {
        if (amount <= 0)
        {
            var message = "Количество должно быть строго больше нуля!";
            return Result.Fail(new ValidationError(message));
        }

        try
        {
            var item = await _repository.GetById(id);

            if (item.Quantity < amount)
            {
                var message = $"Недостаточно на складе, сейчас: {item.Quantity}!";
                return Result.Fail(new ValidationError(message));
            }

            var oldQuantity = item.Quantity;
            item.Quantity -= amount;
            await _repository.Update(item);

            await _auditLog.LogAsync(AuditActions.DecreaseQuantity, nameof(Consumable), id.ToString(), new { Quantity = item.Quantity, Removed = amount });

            return Result.Ok();
        }
        catch (NullReferenceException ex)
        {
            return Result.Fail(new NotFoundError(ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error($"Произошла ошибка: {ex.Message}"));
        }
    }
}