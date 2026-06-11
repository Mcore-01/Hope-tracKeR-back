using Hope_tracKeR_back.Models;
using Hope_tracKeR_back.Models.DTOs.Requests;

namespace Hope_tracKeR_back.Repositories.Interfaces
{
    public interface IItemRepository
    {
        Task<IEnumerable<Item>> GetItemsByFilters(ItemFilterDto filter);
    }
}