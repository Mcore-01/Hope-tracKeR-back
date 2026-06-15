
namespace Hope_tracKeR_back.Repositories.Interfaces
{
    public interface IItemRepository<T, TFilter>
    {
        Task<IEnumerable<T>> GetByFilters(TFilter filter);
        Task<T> GetById(int id);
        Task<int> Create(T Value);
        Task Update(T value);
        Task Remove(int id);
    }
}