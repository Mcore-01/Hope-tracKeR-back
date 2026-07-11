using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hope_tracKeR_back.Repositories;

public class WriteOffRepository : IWriteOffRepository
{
    private readonly HTContext _context;
    public WriteOffRepository(HTContext context)
    {
        _context = context;
    }

    public async Task Create(WriteOff write)
    {
        await _context.WriteOffs.AddAsync(write);
    }

    public async Task<Device> GetDeviceById(int deviceId)
    {
        var item = await _context.Devices
            .Include(i => i.Address)
            .Include(i => i.Brand)
            .Include(i => i.Category)
            .Include(i => i.Employee)
            .Include(i => i.Attributes)
            .FirstOrDefaultAsync(i => i.Id == deviceId);
        if (item == default)
            throw new NullReferenceException($"Объект с ID {deviceId} не найден!");

        return item;
    }

    public async Task UpdateDevice(Device device)
    {
        var existing = await _context.Devices
            .Include(d => d.Attributes)
            .FirstOrDefaultAsync(d => d.Id == device.Id);

        if (existing == null)
            throw new NullReferenceException($"Объект с ID {device.Id} не найден!");

        existing.Name = device.Name;
        existing.SerialNumber = device.SerialNumber;
        existing.Status = device.Status;
        existing.AddedDate = device.AddedDate;
        existing.AddressId = device.AddressId;
        existing.BrandId = device.BrandId;
        existing.CategoryId = device.CategoryId;
        existing.EmployeeId = device.EmployeeId;

        _context.ItemAttributes.RemoveRange(existing.Attributes);

        var newAttributes = device.Attributes.Select(a => new ItemAttribute
        {
            Name = a.Name,
            Value = a.Value,
            ItemId = existing.Id
        }).ToList();

        await _context.ItemAttributes.AddRangeAsync(newAttributes);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}