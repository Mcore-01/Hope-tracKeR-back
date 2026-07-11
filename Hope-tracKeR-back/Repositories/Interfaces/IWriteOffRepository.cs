using Hope_tracKeR_back.Models.Entities;

namespace Hope_tracKeR_back.Repositories.Interfaces;

public interface IWriteOffRepository
{
    Task Create(WriteOff write);
    Task<Device> GetDeviceById(int deviceId);
    Task UpdateDevice(Device device);
    Task SaveChangesAsync();
}