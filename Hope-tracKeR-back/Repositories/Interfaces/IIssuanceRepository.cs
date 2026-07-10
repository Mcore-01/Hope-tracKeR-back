using Hope_tracKeR_back.Models.Entities;

namespace Hope_tracKeR_back.Repositories.Interfaces;

public interface IIssuanceRepository
{
    Task Create(Issuance issuance);
    Task<Device> GetDeviceById(int deviceId);
    Task UpdateDevice(Device device);
    Task SaveChangesAsync();
}