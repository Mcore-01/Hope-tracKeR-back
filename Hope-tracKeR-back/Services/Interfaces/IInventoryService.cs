using FluentResults;

namespace Hope_tracKeR_back.Services.Interfaces;

public interface IInventoryService
{
    Task<Result> IncreaseQuantity(int id, int amount);
    Task<Result> DecreaseQuantity(int id, int amount);
}