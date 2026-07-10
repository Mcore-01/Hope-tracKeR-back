using Hope_tracKeR_back.Models.Entities;

namespace Hope_tracKeR_back.Repositories.Interfaces;

public interface IRepairRepository
{
    Task Create(Repair repair);
    Task Update(Repair repair);
    Task<Device> GetDeviceById(int deviceId);
    Task UpdateDevice(Device device);
    Task<Repair> GetRepairByItemId(int itemId);
    Task SaveChangesAsync();
}