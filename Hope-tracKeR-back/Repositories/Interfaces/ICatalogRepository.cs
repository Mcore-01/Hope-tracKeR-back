namespace Hope_tracKeR_back.Repositories.Interfaces;

public interface ICatalogRepository<T>
{
    Task<IEnumerable<T>> GetAll();
    Task<T?> GetById(int id);
    Task<int> Create(T value);
    Task Update(T value);
    Task Remove(int id);
}