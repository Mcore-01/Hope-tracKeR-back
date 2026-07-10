using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Enums;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hope_tracKeR_back.Repositories;

public class DeviceRepository : IItemRepository<Device>
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

        if (!string.IsNullOrWhiteSpace(filter.Status))
            if (Enum.TryParse<DeviceStatus>(filter.Status, true, out var status))
                query = query.Where(i => i.Status == status);

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

        var items = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();
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
        var existing = await _context.Devices
            .Include(d => d.Attributes)
            .FirstOrDefaultAsync(d => d.Id == item.Id);

        if (existing == null)
            throw new NullReferenceException($"Объект с ID {item.Id} не найден!");

        existing.Name = item.Name;
        existing.SerialNumber = item.SerialNumber;
        existing.Status = item.Status;
        existing.AddedDate = item.AddedDate;
        existing.AddressId = item.AddressId;
        existing.BrandId = item.BrandId;
        existing.CategoryId = item.CategoryId;
        existing.EmployeeId = item.EmployeeId;

        _context.ItemAttributes.RemoveRange(existing.Attributes);

        var newAttributes = item.Attributes.Select(a => new ItemAttribute {
            Name = a.Name,
            Value = a.Value,
            ItemId = existing.Id 
        }).ToList();

        await _context.ItemAttributes.AddRangeAsync(newAttributes);

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