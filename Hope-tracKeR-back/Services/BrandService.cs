using FluentResults;
using FluentValidation;
using Hope_tracKeR_back.Errors;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Hope_tracKeR_back.Services.Interfaces;

namespace Hope_tracKeR_back.Services;

public class BrandService : ICatalogService<Brand>
{
    private readonly ICatalogRepository<Brand> _repository;
    private readonly IValidator<Brand> _validator;
    public BrandService(ICatalogRepository<Brand> repository, IValidator<Brand> validator)
    {
        _repository = repository;
        _validator = validator;
    }
    
    public async Task<Result<IEnumerable<Brand>>> GetAll()
    {
        try
        {
            var brands = await _repository.GetAll();

            return Result.Ok(brands);
        }
        catch (Exception ex)
        {
            return Result.Fail<IEnumerable<Brand>>(new Error($"Произошла ошибка: {ex.Message}"));
        }
    }

    public async Task<Result<Brand>> GetById(int id)
    {
        try
        {
            var brand = await _repository.GetById(id);

            if (brand is null)
                return Result.Fail<Brand>(new NotFoundError(nameof(Brand), id));

            return Result.Ok(brand);
        }
        catch (Exception ex)
        {
            return Result.Fail<Brand>(new Error($"Произошла ошибка: {ex.Message}"));
        }
    }

    public async Task<Result<int>> Create(Brand brand)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(brand);   
            if (!validationResult.IsValid)
            {
                var errors = string.Join("\n", validationResult.Errors);
                return Result.Fail<int>(new ValidationError(errors));
            }

            var brandId = await _repository.Create(brand);

            return Result.Ok(brandId);
        }
        catch (InvalidOperationException ex)
        {
            return Result.Fail<int>(new InvalidOperationError(ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Fail<int>(new Error($"Произошла ошибка: {ex.Message}"));
        }
    }
   
    public async Task<Result> Update(Brand brand)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(brand);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("\n", validationResult.Errors);
                return Result.Fail(new ValidationError(errors));
            }

            await _repository.Update(brand);

            return Result.Ok();
        }
        catch (NullReferenceException ex)
        {
            return Result.Fail(new NotFoundError(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return Result.Fail(new InvalidOperationError(ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error($"Произошла ошибка: {ex.Message}"));
        }
    }

    public async Task<Result> Remove(int id)
    {
        try
        {
            await _repository.Remove(id);

            return Result.Ok();
        }
        catch (NullReferenceException ex)
        {
            return Result.Fail(new NotFoundError(ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error($"Произошла ошибка: {ex.Message}"));
        }
    }
}