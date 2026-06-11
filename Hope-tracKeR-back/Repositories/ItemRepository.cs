using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Models;
using Hope_tracKeR_back.Models.DTOs.Requests;
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
}