using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.Entities;

namespace Hope_tracKeR_back.Repositories.Interfaces
{
    public interface IItemRepository
    {
        Task<IEnumerable<Item>> GetItemsByFilters(ItemFilterDto filter);
        Task<Item?> GetItemById(int id);
        Task<int> CreateItem(ItemModifyDto item);
        Task<bool> UpdateItem(ItemModifyDto item);
        Task<bool> RemoveItem(int id);
    }
}