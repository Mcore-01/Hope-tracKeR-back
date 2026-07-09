using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hope_tracKeR_back.Repositories;

public class ConsumableRepository : IItemRepository<Consumable>
{
    private readonly HTContext _context;
    public ConsumableRepository(HTContext context)
    {
        _context = context;
    }

    public async Task<int> Create(Consumable item)
    {
        _context.Consumables.Add(item);

        await _context.SaveChangesAsync();

        return item.Id;
    }

    public async Task<IEnumerable<Consumable>> GetByFilters(ItemFilter filter)
    {
        var query = _context.Consumables
            .Include(i => i.Address)
            .Include(i => i.Brand)
            .Include(i => i.Attributes)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.SearchField))
        {
            var searchField = filter.SearchField.ToLower();
            query = query.Where(i => i.Name.ToLower().Contains(searchField));
        }

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

    public async Task<Consumable> GetById(int id)
    {
        var item = await _context.Consumables
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
        var existingItem = await _context.Consumables.FirstOrDefaultAsync(i => i.Id == id);

        if (existingItem == default)
            throw new NullReferenceException($"Объект с ID {id} не найден!");

        _context.Consumables.Remove(existingItem);

        await _context.SaveChangesAsync();
    }

    public async Task Update(Consumable item)
    {
        var itemIsExist = _context.Consumables.Include(i => i.Attributes).Any(i => i.Id == item.Id);

        if (!itemIsExist)
            throw new NullReferenceException($"Объект с ID {item.Id} не найден!");

        _context.Consumables.Update(item);

        await _context.SaveChangesAsync();
    }
}