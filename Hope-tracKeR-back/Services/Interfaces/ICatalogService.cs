using FluentResults;

namespace Hope_tracKeR_back.Services.Interfaces;

public interface ICatalogService<T>
{
    Task<Result<IEnumerable<T>>> GetAll();
    Task<Result<T>> GetById(int id);
    Task<Result<int>> Create(T value);
    Task<Result> Update(T value);
    Task<Result> Remove(int id);
}