using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Enums;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hope_tracKeR_back.Repositories;

public class CartridgeRepository : IItemRepository<Cartridge>
{
    private readonly HTContext _context;
    public CartridgeRepository(HTContext context)
    {
        _context = context;
    }

    public async Task<int> Create(Cartridge item)
    {
        _context.Cartridges.Add(item);

        await _context.SaveChangesAsync();

        return item.Id;
    }

    public async Task<IEnumerable<Cartridge>> GetByFilters(ItemFilter filter)
    {
        var query = _context.Cartridges
            .Include(i => i.Address)
            .Include(i => i.Brand)
            .Include(i => i.Attributes)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.SearchField))
        {
            var searchField = filter.SearchField.ToLower();
            query = query.Where(i => i.Name.ToLower().Contains(searchField) || i.PrinterModel.ToLower().Contains(searchField));
        }

        if (!string.IsNullOrWhiteSpace(filter.Status))
            if (Enum.TryParse<CartridgeStatus>(filter.Status, true, out var status))
                query = query.Where(i => i.Status == status);

        if (filter.AddressId.HasValue)
            query = query.Where(i => i.AddressId == filter.AddressId.Value);

        if (filter.BrandId.HasValue)
            query = query.Where(i => i.BrandId == filter.BrandId.Value);

        if (filter.Attributes is not null && filter.Attributes.Count > 0)
        {
            foreach (var attr in filter.Attributes)
            {
                query = query.Where(i => i.Attributes.Any(a => a.Name == attr.Key && a.Value == attr.Value));
            }
        }

        var items = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();
        return items;
    }

    public async Task<Cartridge> GetById(int id)
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

    public async Task Remove(int id)
    {
        var existingItem = await _context.Cartridges.FirstOrDefaultAsync(i => i.Id == id);

        if (existingItem == default)
            throw new NullReferenceException($"Объект с ID {id} не найден!");

        _context.Cartridges.Remove(existingItem);

        await _context.SaveChangesAsync();
    }

    public async Task Update(Cartridge item)
    {
        var itemIsExist = _context.Cartridges.Include(i => i.Attributes).Any(i => i.Id == item.Id);

        if (!itemIsExist)
            throw new NullReferenceException($"Объект с ID {item.Id} не найден!");

        _context.Cartridges.Update(item);

        await _context.SaveChangesAsync();
    }
}