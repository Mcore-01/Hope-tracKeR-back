using FluentResults;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.Entities;

namespace Hope_tracKeR_back.Repositories.Interfaces
{
    public interface IItemRepository
    {
        Task<Result<IEnumerable<Item>>> GetItemsByFilters(ItemFilter filter);
        Task<Result<Item>> GetItemById(int id);
        Task<Result<int>> CreateItem(ItemModify item);
        Task<Result> UpdateItem(ItemModify item);
        Task<Result> RemoveItem(int id);
    }
}