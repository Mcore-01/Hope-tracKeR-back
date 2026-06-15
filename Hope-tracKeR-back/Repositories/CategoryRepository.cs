using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hope_tracKeR_back.Repositories;

public class CategoryRepository : ICatalogRepository<Category>
{
    private readonly HTContext _context;
    public CategoryRepository(HTContext context)
    {
        _context = context;
    }
    public async Task<int> Create(Category category)
    {
        var existingBrand = await _context.Categories.FirstOrDefaultAsync(c => c.Name == category.Name);

        if (existingBrand != default)
            throw new InvalidOperationException($"Создать объект не удалось, так как категория с названием {category.Name} уже существует!");

        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        return category.Id;
    }

    public async Task<IEnumerable<Category>> GetAll()
    {
        var categories = await _context.Categories.ToListAsync();
        return categories;
    }

    public async Task<Category?> GetById(int id)
    {
        var existingCategory = await _context.Categories.FirstOrDefaultAsync(e => e.Id == id);

        if (existingCategory == default)
            throw new NullReferenceException($"Объект с ID {id} не найден!");

        return existingCategory;
    }

    public async Task Remove(int id)
    {
        var existingCategory = await _context.Categories.FirstOrDefaultAsync(e => e.Id == id);

        if (existingCategory == default)
            throw new NullReferenceException($"Объект с ID {id} не найден!");

        _context.Categories.Remove(existingCategory);

        await _context.SaveChangesAsync();
    }

    public async Task Update(Category category)
    {
        if (_context.Categories.Any(c => c.Name == category.Name))
            throw new InvalidOperationException($"Создать объект не удалось, так как категории с названием {category.Name} уже существует!");

        var existingCategory = await _context.Categories.FirstOrDefaultAsync(с => с.Id == category.Id);

        if (existingCategory == default)
            throw new NullReferenceException($"Объект с ID {category.Id} не найден!");
        
        _context.Categories.Update(category);

        await _context.SaveChangesAsync();
    }
}
