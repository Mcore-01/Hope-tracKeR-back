using FluentResults;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.Entities;

namespace Hope_tracKeR_back.Repositories.Interfaces
{
    public interface IItemRepository
    {
        Task<Result<IEnumerable<Item>>> GetItemsByFilters(ItemFilterDto filter);
        Task<Result<Item>> GetItemById(int id);
        Task<Result<int>> CreateItem(ItemModifyDto item);
        Task<Result> UpdateItem(ItemModifyDto item);
        Task<Result> RemoveItem(int id);
    }
}