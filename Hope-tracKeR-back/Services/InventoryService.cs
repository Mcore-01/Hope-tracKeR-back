using FluentResults;
using Hope_tracKeR_back.Errors;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Hope_tracKeR_back.Services.Interfaces;

namespace Hope_tracKeR_back.Services;

public class InventoryService : IInventoryService
{
    private readonly IItemRepository<Consumable> _repository;
    public InventoryService(IItemRepository<Consumable> repository)
    {
        _repository = repository;
    }

    public async Task<Result> IncreaseQuantity(int id, int amount)
    {
        if (amount <= 0)
            return Result.Fail(new ValidationError("Количество должно быть строго больше нуля!"));

        try
        {
            var item = await _repository.GetById(id);

            item.Quantity += amount;
            await _repository.Update(item);
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
            return Result.Fail(new ValidationError("Количество должно быть строго больше нуля!"));

        try
        {
            var item = await _repository.GetById(id);

            if (item.Quantity < amount)
                return Result.Fail(new ValidationError($"Недостаточно на складе, сейчас: {item.Quantity}!"));

            item.Quantity -= amount;
            await _repository.Update(item);
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