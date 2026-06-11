using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Enums;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hope_tracKeR_back.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly HTContext _context;
    public ItemRepository(HTContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Item>> GetItemsByFilters(ItemFilterDto filter)
    {
        var query = _context.Items
            .Include(i => i.Address)
            .Include(i => i.Brand)
            .Include(i => i.Attributes)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.SearchField))
        {
            var searchField = filter.SearchField.ToLower();
            query = query.Where(i => i.Name.ToLower().Contains(searchField) || i.SerialId.ToLower().Contains(searchField));
        }
       
        if (filter.Category.HasValue)
            query = query.Where(i => i.Category == filter.Category.Value);

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
                query = query.Where(i => i.Attributes.Any(a =>  a.Name == attr.Key && a.Value == attr.Value));
            }
        }

        return await query.ToListAsync();
    }

    public async Task<Item?> GetItemById(int id)
    {
        return await _context.Items
            .Include(i => i.Address)
            .Include(i => i.Brand)
            .Include(i => i.Attributes)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<int> CreateItem(ItemModifyDto item)
    {
        var newItem = new Item
        {
            Name = item.Name,
            SerialId = item.SerialId,
            Category = Enum.Parse<ItemCategory>(item.Category), 
            Status = Enum.Parse<ItemStatus>(item.Status),      
            AddedDate = item.AddedDate,
            AddressId = item.AddressId,
            BrandId = item.BrandId,
            Attributes = item.Attributes.Select(a => new ItemAttribute { Name = a.Key, Value = a.Value}).ToList()
        };

        _context.Items.Add(newItem);

        await _context.SaveChangesAsync();

        return newItem.Id;
    }

    public async Task<bool> UpdateItem(ItemModifyDto item)
    {
        var existingItem = await _context.Items.Include(i => i.Attributes).FirstOrDefaultAsync(i => i.Id == item.Id);

        if (existingItem is null)
            return false;

        existingItem.Name = item.Name;
        existingItem.SerialId = item.SerialId;
        existingItem.AddedDate = item.AddedDate;
        existingItem.AddressId = item.AddressId;
        existingItem.BrandId = item.BrandId;
        existingItem.Category = Enum.Parse<ItemCategory>(item.Category);
        existingItem.Status = Enum.Parse<ItemStatus>(item.Status);

        _context.ItemAttributes.RemoveRange(existingItem.Attributes);

        existingItem.Attributes = item.Attributes.Select(a => new ItemAttribute { Name = a.Key, Value = a.Value }).ToList();

        await _context.SaveChangesAsync();  
        
        return true;
    }
    public async Task<bool> RemoveItem(int id)
    {
        var existingItem = await _context.Items.FirstOrDefaultAsync(i => i.Id == id);

        if (existingItem is null)
            return false;

        _context.Items.Remove(existingItem);

        await _context.SaveChangesAsync();

        return true;
    }
}