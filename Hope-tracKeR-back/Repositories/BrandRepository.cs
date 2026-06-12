using FluentResults;
using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Models.DTOs.Requests;
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

    public async Task<Result<IEnumerable<Brand>>> GetAllBrands()
    {
        try
        {
            var brands = await _context.Brands.ToListAsync();
            return Result.Ok<IEnumerable<Brand>>(brands);
        }
        catch (Exception ex)
        {
            return Result.Fail<IEnumerable<Brand>>(new Error("Ошибка базы данных!").CausedBy(ex));
        }
    }

    public async Task<Result<Brand>> GetBrandById(int id)
    {
        try
        {
            var brand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);

            if (brand == default)
                return Result.Fail<Brand>(new Error("Бренд не найден!"));

            return Result.Ok<Brand>(brand);
        }
        catch (Exception ex)
        {
            return Result.Fail<Brand>(new Error("Ошибка базы данных!").CausedBy(ex));
        }
    }

    public async Task<Result<int>> CreateBrand(BrandDto brand)
    {
        try
        {
            var existingBrand = await _context.Brands.FirstOrDefaultAsync(b => b.Name == brand.Name);

            if (existingBrand != default)
                return Result.Fail<int>(new Error("Такой бренд существует!"));

            var newBrand = new Brand() { Name = brand.Name };
            await _context.Brands.AddAsync(newBrand);
            await _context.SaveChangesAsync();

            return Result.Ok<int>(newBrand.Id);
        }
        catch (Exception ex)
        {
            return Result.Fail<int>($"Ошибка создания бренда: {ex.Message}");
        }
    }

    public async Task<Result> UpdateBrand(BrandDto brand)
    {
        try
        {
            var existingBrand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == brand.Id);

            if (existingBrand == default)
                return Result.Fail(new Error("Бренд не найден!"));

            existingBrand.Name = brand.Name;

            await _context.SaveChangesAsync();

            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail($"Ошибка модификации бренда: {ex.Message}");
        }
    }

    public async Task<Result> RemoveBrand(int id)
    {
        try
        {
            var existingBrand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);

            if (existingBrand == default)
                return Result.Fail(new Error("Бренд не найден!"));

            _context.Brands.Remove(existingBrand);

            await _context.SaveChangesAsync();

            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail($"Ошибка удаления бренда: {ex.Message}");
        }
    }
}