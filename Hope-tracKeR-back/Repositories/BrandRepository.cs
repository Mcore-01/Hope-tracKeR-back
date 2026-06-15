using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hope_tracKeR_back.Repositories;

public class BrandRepository : ICatalogRepository<Brand>
{
    private readonly HTContext _context;

    public BrandRepository(HTContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Brand>> GetAll()
    {
        var brands = await _context.Brands.ToListAsync();
        return brands;
    }

    public async Task<Brand?> GetById(int id)
    {
        var brand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);

        if (brand == default)
            throw new NullReferenceException($"Объект с ID {id} не найден!");

        return brand;
    }

    public async Task<int> Create(Brand brand)
    {
        var existingBrand = await _context.Brands.FirstOrDefaultAsync(b => b.Name == brand.Name);

        if (existingBrand != default)
            throw new InvalidOperationException($"Создать объект не удалось, так как бренд с названием {brand.Name} уже существует!");

        var newBrand = new Brand() { Name = brand.Name };
        await _context.Brands.AddAsync(newBrand);
        await _context.SaveChangesAsync();

        return newBrand.Id;
    }

    public async Task Update(Brand brand)
    {
        if(_context.Brands.Any(b => b.Name == brand.Name))
            throw new InvalidOperationException($"Создать объект не удалось, так как бренд с названием {brand.Name} уже существует!");

        var brandIsExist = _context.Brands.Any(b => b.Id == brand.Id);

        if (!brandIsExist)
            throw new NullReferenceException($"Объект с ID {brand.Id} не найден!");

        _context.Brands.Update(brand);

        await _context.SaveChangesAsync();
    }

    public async Task Remove(int id)
    {
        var existingBrand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);

        if (existingBrand == default)
            throw new NullReferenceException($"Объект с ID {id} не найден!");

        _context.Brands.Remove(existingBrand);

        await _context.SaveChangesAsync();
    }
}