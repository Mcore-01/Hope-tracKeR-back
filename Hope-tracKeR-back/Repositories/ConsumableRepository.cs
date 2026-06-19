using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;

namespace Hope_tracKeR_back.Repositories;

public class ConsumableRepository : IItemRepository<Consumable>
{
    public Task<int> Create(Consumable Value)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Consumable>> GetByFilters(ItemFilter filter)
    {
        throw new NotImplementedException();
    }

    public Task<Consumable> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Task Remove(int id)
    {
        throw new NotImplementedException();
    }

    public Task Update(Consumable value)
    {
        throw new NotImplementedException();
    }
}