using DocumentFormat.OpenXml.Office2010.Excel;
using FluentResults;
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

    public  async Task<int> Create(Repair repair)
    {
        _context.Repairs.Add(repair);

        await _context.SaveChangesAsync();

        return repair.Id;
    }

    public async Task Update(Repair repair)
    {
        var repairIsExist = _context.Repairs.Any(r => r.Id == repair.Id);
        if (!repairIsExist)
            throw new NullReferenceException($"Объект с ID {repair.Id} не найден!");

        _context.Repairs.Update(repair);

        await _context.SaveChangesAsync();
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
}