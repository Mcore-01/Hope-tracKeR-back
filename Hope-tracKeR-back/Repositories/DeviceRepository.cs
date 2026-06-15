using FluentResults;
using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Enums;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hope_tracKeR_back.Repositories;

public class DeviceRepository : IItemRepository
{
    private readonly HTContext _context;
    public DeviceRepository(HTContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<Device>>> GetByFilters(ItemFilter filter)
    {
        try
        {
            var query = _context.Devices
            .Include(i => i.Address)
            .Include(i => i.Brand)
            .Include(i => i.Attributes)
            .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.SearchField))
            {
                var searchField = filter.SearchField.ToLower();
                query = query.Where(i => i.Name.ToLower().Contains(searchField) || i.SerialId.ToLower().Contains(searchField));
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
            return Result.Ok<IEnumerable<Device>>(items);
        }
        catch (Exception ex)
        {
            return Result.Fail<IEnumerable<Device>>(new Error("Ошибка базы данных!").CausedBy(ex));
        }
    }

    public async Task<Result<Device>> GetById(int id)
    {
        try
        {
            var item = await _context.Devices
            .Include(i => i.Address)
            .Include(i => i.Brand)
            .Include(i => i.Attributes)
            .FirstOrDefaultAsync(i => i.Id == id);
            if (item != default)
                return Result.Ok<Device>(item);

            return Result.Fail<Device>(new Error("Предмет не найден!"));
        }
        catch (Exception ex)
        {
            return Result.Fail<Device>(new Error("Ошибка базы данных!").CausedBy(ex));
        }
    }

    public async Task<Result<int>> Create(DeviceModifyRequest item)
    {
        try
        {
            var newItem = new Device
            {
                Name = item.Name,
                SerialId = item.SerialId,
                Status = Enum.Parse<DeviceStatus>(item.Status),
                AddedDate = item.AddedDate,
                AddressId = item.AddressId,
                BrandId = item.BrandId,
                Attributes = item.Attributes.Select(a => new ItemAttribute { Name = a.Key, Value = a.Value }).ToList()
            };

            _context.Devices.Add(newItem);

            await _context.SaveChangesAsync();

            return Result.Ok(newItem.Id);
        }
        catch (Exception ex)
        {
            return Result.Fail<int>($"Ошибка создания предмета: {ex.Message}");
        }
    }

    public async Task<Result> Update(DeviceModifyRequest item)
    {
        
        try
        {
            var existingItem = await _context.Devices.Include(i => i.Attributes).FirstOrDefaultAsync(i => i.Id == item.Id);

            if (existingItem == default)
                return Result.Fail(new Error("Предмет не найден!"));

            existingItem.Name = item.Name;
            existingItem.SerialId = item.SerialId;
            existingItem.AddedDate = item.AddedDate;
            existingItem.AddressId = item.AddressId;
            existingItem.BrandId = item.BrandId;
            existingItem.Status = Enum.Parse<DeviceStatus>(item.Status);

            _context.ItemAttributes.RemoveRange(existingItem.Attributes);

            existingItem.Attributes = item.Attributes.Select(a => new ItemAttribute { Name = a.Key, Value = a.Value }).ToList();

            await _context.SaveChangesAsync();

            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail($"Ошибка модификации предмета: {ex.Message}");
        }
    }
    public async Task<Result> Remove(int id)
    {
        try
        {
            var existingItem = await _context.Devices.FirstOrDefaultAsync(i => i.Id == id);

            if (existingItem == default)
                return Result.Fail((new Error("Предмет не найден!")));

            _context.Devices.Remove(existingItem);

            await _context.SaveChangesAsync();

            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail($"Ошибка удаления предмета: {ex.Message}");
        }
    }
}