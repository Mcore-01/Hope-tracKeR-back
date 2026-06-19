using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hope_tracKeR_back.Repositories;

public class DeviceRepository : IItemRepository<Device, ItemFilter>
{
    private readonly HTContext _context;
    public DeviceRepository(HTContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Device>> GetByFilters(ItemFilter filter)
    {

        var query = _context.Devices
        .Include(i => i.Address)
        .Include(i => i.Brand)
        .Include(i => i.Category)
        .Include(i => i.Employee)
        .Include(i => i.Attributes)
        .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.SearchField))
        {
            var searchField = filter.SearchField.ToLower();
            query = query.Where(i => i.Name.ToLower().Contains(searchField) || i.SerialNumber.ToLower().Contains(searchField));
        }

        if (filter.Status.HasValue)
            query = query.Where(i => i.Status == filter.Status.Value);

        if (filter.AddedDateFrom.HasValue)
            query = query.Where(i => i.AddedDate >= filter.AddedDateFrom.Value);

        if (filter.AddedDateTo.HasValue)
            query = query.Where(i => i.AddedDate < filter.AddedDateTo.Value);

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

        var items = await query.ToListAsync();
        return items;
    }

    public async Task<Device> GetById(int id)
    {
        var item = await _context.Devices
            .Include(i => i.Address)
            .Include(i => i.Brand)
            .Include(i => i.Category)
            .Include(i => i.Employee)
            .Include(i => i.Attributes)
            .FirstOrDefaultAsync(i => i.Id == id);
        if (item　== default)
            throw new NullReferenceException($"Объект с ID {id} не найден!");

        return item;
    }

    public async Task<int> Create(Device item)
    {
        _context.Devices.Add(item);

        await _context.SaveChangesAsync();

        return item.Id;
    }

    public async Task Update(Device item)
    {
        var itemIsExist = _context.Devices.Include(i => i.Attributes).Any(i => i.Id == item.Id);

        if (!itemIsExist)
            throw new NullReferenceException($"Объект с ID {item.Id} не найден!");
        
        _context.Devices.Update(item);

        await _context.SaveChangesAsync();
    }

    public async Task Remove(int id)
    {
        var existingItem = await _context.Devices.FirstOrDefaultAsync(i => i.Id == id);

        if (existingItem == default)
            throw new NullReferenceException($"Объект с ID {id} не найден!");

        _context.Devices.Remove(existingItem);

        await _context.SaveChangesAsync();
    }
}