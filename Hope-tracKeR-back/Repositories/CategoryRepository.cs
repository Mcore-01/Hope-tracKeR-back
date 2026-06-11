using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Models;
using Hope_tracKeR_back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hope_tracKeR_back.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly HTContext _context;
    public CategoryRepository(HTContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Category>> GetAllCategories()
    {
        return await _context.Categories.ToListAsync();
    }
}
