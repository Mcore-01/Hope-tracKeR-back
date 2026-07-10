using DocumentFormat.OpenXml.Office2010.Excel;
using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hope_tracKeR_back.Repositories;

public class RepairRepository : IRepairRepository
{
    private readonly HTContext _context;
    public RepairRepository(HTContext context)
    {
        _context = context;
    }

    public  async Task Create(Repair repair)
    {
        await _context.Repairs.AddAsync(repair);
    }

    public async Task Update(Repair repair)
    {
        var repairIsExist = await _context.Repairs.AnyAsync(r => r.Id == repair.Id);
        if (!repairIsExist)
            throw new NullReferenceException($"Объект с ID {repair.Id} не найден!");

        _context.Repairs.Update(repair);
    }

    public async Task<Repair> GetRepairByItemId(int itemId)
    {
        var repair = await _context.Repairs
            .Include(r => r.Item)
            .Include(r => r.Address)
            .Include(r => r.User)
            .OrderBy(r => r.StartDate)
            .LastOrDefaultAsync(r => r.ItemId == itemId);

        if (repair == default)
            throw new NullReferenceException($"Объект с ID {itemId} не найден!");

        return repair;
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