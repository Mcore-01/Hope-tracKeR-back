using Hope_tracKeR_back.Models.Entities;

namespace Hope_tracKeR_back.Repositories.Interfaces;

public interface IRefillRepository
{
    Task<int> Create(Refill refill);
    Task Update(Refill refill);
    Task<Refill> GetRefillByItemId(int itemId);
}