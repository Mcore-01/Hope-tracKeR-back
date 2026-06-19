
using Hope_tracKeR_back.Models.DTOs.Requests;

namespace Hope_tracKeR_back.Repositories.Interfaces;

public interface IItemRepository<T>
{
    Task<IEnumerable<T>> GetByFilters(ItemFilter filter);
    Task<T> GetById(int id);
    Task<int> Create(T Value);
    Task Update(T value);
    Task Remove(int id);
}