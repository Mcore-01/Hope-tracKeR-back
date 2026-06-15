using FluentResults;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.Entities;

namespace Hope_tracKeR_back.Repositories.Interfaces
{
    public interface IItemRepository
    {
        Task<Result<IEnumerable<Device>>> GetByFilters(ItemFilter filter);
        Task<Result<Device>> GetById(int id);
        Task<Result<int>> Create(DeviceModifyRequest item);
        Task<Result> Update(DeviceModifyRequest item);
        Task<Result> Remove(int id);
    }
}