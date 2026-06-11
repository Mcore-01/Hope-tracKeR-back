using FluentResults;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.DTOs.Responses;
using Hope_tracKeR_back.Models.Entities;

namespace Hope_tracKeR_back.Services.Interfaces;

public interface IItemService
{
    Task<Result<IEnumerable<ItemResponseDto>>> GetItemsByFilters(ItemFilterDto filter);
    Task<Result<ItemResponseDto>> GetItemById(int id);
    Task<Result<int>> CreateItem(ItemModifyDto item);
    Task<Result> UpdateItem(ItemModifyDto item);
    Task<Result> RemoveItem(int id);
}