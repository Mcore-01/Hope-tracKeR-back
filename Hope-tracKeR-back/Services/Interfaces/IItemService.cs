using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.DTOs.Responses;

namespace Hope_tracKeR_back.Services.Interfaces;

public interface IItemService
{
    Task<IEnumerable<ItemResponseDto>> GetItemsByFilters(ItemFilterDto filter);
}