using DocumentFormat.OpenXml.Office2010.Excel;
using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hope_tracKeR_back.Repositories;

public class RefillRepository : IRefillRepository
{
    private readonly HTContext _context;
    public RefillRepository(HTContext context)
    {
        _context = context;
    }

    public async Task Create(Refill refill)
    {
        await _context.Refills.AddAsync(refill);
    }

    public async Task Update(Refill refill)
    {
        var refillIsExist = _context.Refills.Any(r => r.Id == refill.Id);
        if (!refillIsExist)
            throw new NullReferenceException($"Объект с ID {refill.Id} не найден!");

        _context.Refills.Update(refill);

        await _context.SaveChangesAsync();
    }

    public async Task<Refill> GetRefillByItemId(int itemId)
    {
        var refill = await _context.Refills
            .Include(r => r.Item)
            .Include(r => r.Address)
            .Include(r => r.User)
            .OrderBy(r => r.StartDate)
            .LastOrDefaultAsync(r => r.ItemId == itemId);

        if (refill == default)
            throw new NullReferenceException($"Объект с ID {itemId} не найден!");

        return refill;
    }

    public async Task<Cartridge> GetCartridgeById(int id)
    {
        var item = await _context.Cartridges
            .Include(i => i.Address)
            .Include(i => i.Brand)
            .Include(i => i.Attributes)
            .FirstOrDefaultAsync(i => i.Id == id);
        if (item == default)
            throw new NullReferenceException($"Объект с ID {id} не найден!");

        return item;
    }

    public async Task UpdateCartridge(Cartridge item)
    {
        var itemIsExist = await _context.Cartridges
            .Include(i => i.Attributes)
            .AnyAsync(i => i.Id == item.Id);

        if (!itemIsExist)
            throw new NullReferenceException($"Объект с ID {item.Id} не найден!");

        _context.Cartridges.Update(item);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}