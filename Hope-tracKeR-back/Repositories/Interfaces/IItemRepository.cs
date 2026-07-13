
using Hope_tracKeR_back.Models.DTOs.Requests;
using X.PagedList;

namespace Hope_tracKeR_back.Repositories.Interfaces;

public interface IItemRepository<T>
{
    Task<IPagedList<T>> GetByFilters(ItemFilterRequest filter);
    Task<T> GetById(int id);
    Task<int> Create(T Value);
    Task Update(T value);
    Task Remove(int id);
}