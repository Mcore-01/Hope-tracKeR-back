using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.DTOs.Responses;
using Hope_tracKeR_back.Models.Entities;

namespace Hope_tracKeR_back.Services.Interfaces;

public interface IItemService
{
    Task<IEnumerable<ItemResponseDto>> GetItemsByFilters(ItemFilterDto filter);
    Task<ItemResponseDto?> GetItemById(int id);
    Task<int> CreateItem(ItemModifyDto item);
    Task<bool> UpdateItem(ItemModifyDto item);
    Task<bool> RemoveItem(int id);
}