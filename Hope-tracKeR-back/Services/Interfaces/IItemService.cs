using FluentResults;
using Hope_tracKeR_back.Models.DTOs.Requests;

namespace Hope_tracKeR_back.Services.Interfaces;

public interface IItemService <TRequest, TResponse>
{
    Task<Result<IEnumerable<TResponse>>> GetByFilters(ItemFilter filter);
    Task<Result<TResponse>> GetById(int id);
    Task<Result<int>> Create(TRequest item);
    Task<Result> Update(TRequest item);
    Task<Result> Remove(int id);
    Task<Result<Byte[]>> ExportItemsToExcel(ItemFilter filter);
}