using FluentResults;

namespace Hope_tracKeR_back.Services.Interfaces;

public interface IItemService <TRequest, TFilter, TResponse> : IRepairService, IWriteOffService
{
    Task<Result<IEnumerable<TResponse>>> GetByFilters(TFilter filter);
    Task<Result<TResponse>> GetById(int id);
    Task<Result<int>> Create(TRequest item);
    Task<Result> Update(TRequest item);
    Task<Result> Remove(int id);
    Task<Result<Byte[]>> ExportItemsToExcel(TFilter filter);
}