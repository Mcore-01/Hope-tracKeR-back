using FluentResults;
using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Enums;
using Hope_tracKeR_back.Models.DTOs.Requests;
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

    public  async Task<Result> CreateRepair(StartRepairRequest repairRequest)
    {
        try
        {
            var existingItem = await _context.Items.FirstOrDefaultAsync(i => i.Id == repairRequest.ItemId);
            if(existingItem == default)
                return Result.Fail(new Error("Предмет не найден!"));

            existingItem.Status = ItemStatus.Repair;
            existingItem.AddressId = repairRequest.CurrentAddressId;

            var repair = new Repair()
            {
                StartDate = repairRequest.StartDate,
                Description = repairRequest.DescriptionFailure,
                Status = RepairStatus.InProgress,
                ItemId = repairRequest.ItemId,
                AddressId = repairRequest.CurrentAddressId,
            };
            _context.Repairs.Add(repair);

            await _context.SaveChangesAsync();
            
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Ошибка базы данных!").CausedBy(ex));
        }
    }

    public async Task<Result> CompleteRepair(CompleteRepairRequest repairRequest)
    {
        try
        {
            var existingItem = await _context.Items.FirstOrDefaultAsync(i => i.Id == repairRequest.ItemId);
            if (existingItem == default)
                return Result.Fail(new Error("Предмет не найден!"));

            existingItem.Status = ItemStatus.InStock;
            existingItem.AddressId = repairRequest.CurrentAddressId;

            var existingRepair = await _context.Repairs.FirstOrDefaultAsync(r => r.ItemId == repairRequest.ItemId && r.EndDate == null);
            if (existingRepair == default)
                return Result.Fail(new Error("Предмет отсуствует в перечне ремонта!"));

            existingRepair.EndDate = repairRequest.EndDate;
            existingRepair.Status = RepairStatus.Completed;
            existingRepair.Diagnosis = repairRequest.Diagnosis;
            existingRepair.ItemId = repairRequest.ItemId;
            existingRepair.AddressId = repairRequest.CurrentAddressId;

            await _context.SaveChangesAsync();

            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Ошибка базы данных!").CausedBy(ex));
        }
    }
}