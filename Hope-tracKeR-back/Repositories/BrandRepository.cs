using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hope_tracKeR_back.Repositories;

public class BrandRepository : IBrandRepository
{
    private readonly HTContext _context;

    public BrandRepository(HTContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Brand>> GetAllBrands()
    {
        return await _context.Brands.ToListAsync();
    }
}