using Hope_tracKeR_back.Models.Entities;

namespace Hope_tracKeR_back.Repositories.Interfaces;

public interface IRepairRepository
{
    Task<int> Create(Repair repair);
    Task Update(Repair repair);
    Task<Repair> GetRepairByItemId(int itemId);
}