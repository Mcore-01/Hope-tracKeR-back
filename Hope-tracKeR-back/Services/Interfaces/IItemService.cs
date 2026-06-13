using FluentResults;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.DTOs.Responses;

namespace Hope_tracKeR_back.Services.Interfaces;

public interface IItemService : IRepairService
{
    Task<Result<IEnumerable<ItemResponse>>> GetItemsByFilters(ItemFilter filter);
    Task<Result<ItemResponse>> GetItemById(int id);
    Task<Result<int>> CreateItem(ItemModify item);
    Task<Result> UpdateItem(ItemModify item);
    Task<Result> RemoveItem(int id);
    Task<Result<Byte[]>> ExportItemsToExcel(ItemFilter filter);
}