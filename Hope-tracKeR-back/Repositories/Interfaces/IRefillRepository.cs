using Hope_tracKeR_back.Models.Entities;

namespace Hope_tracKeR_back.Repositories.Interfaces;

public interface IRefillRepository
{
    Task Create(Refill refill);
    Task Update(Refill refill);
    Task<Cartridge> GetCartridgeById(int id);
    Task UpdateCartridge(Cartridge device);
    Task<Refill> GetRefillByItemId(int itemId);
    Task SaveChangesAsync();
}