using FluentResults;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.DTOs.Responses;

namespace Hope_tracKeR_back.Services.Interfaces;

public interface IItemService <TRequest, TResponse> where TResponse : class
{
    Task<Result<PagedListResponse<TResponse>>> GetByFilters(ItemFilterRequest filter);
    Task<Result<TResponse>> GetById(int id);
    Task<Result<int>> Create(TRequest item);
    Task<Result> Update(TRequest item);
    Task<Result> Remove(int id);
    Task<Result<Byte[]>> ExportItemsToExcel(ItemFilterRequest filter);
}