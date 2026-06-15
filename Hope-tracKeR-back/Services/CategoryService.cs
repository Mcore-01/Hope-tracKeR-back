using FluentResults;
using FluentValidation;
using Hope_tracKeR_back.Errors;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Hope_tracKeR_back.Services.Interfaces;

namespace Hope_tracKeR_back.Services;

public class CategoryService : ICatalogService<Category>
{
    private readonly ICatalogRepository<Category> _repository;
    private readonly IValidator<Category> _validator;

    public CategoryService(ICatalogRepository<Category> repository, IValidator<Category> validator)
    {
        _repository = repository;
        _validator = validator;
    }
    public async Task<Result<int>> Create(Category category)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(category);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("\n", validationResult.Errors);
                return Result.Fail<int>(new ValidationError(errors));
            }

            var categoryId = await _repository.Create(category);

            return Result.Ok(categoryId);
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

    public async Task<Result<IEnumerable<Category>>> GetAll()
    {
        try
        {
            var categories = await _repository.GetAll();

            return Result.Ok(categories);
        }
        catch (Exception ex)
        {
            return Result.Fail<IEnumerable<Category>>(new Error($"Произошла ошибка: {ex.Message}"));
        }
    }

    public async Task<Result<Category>> GetById(int id)
    {
        try
        {
            var category = await _repository.GetById(id);

            if (category is null)
                return Result.Fail<Category>(new NotFoundError(nameof(Category), id));

            return Result.Ok(category);
        }
        catch (Exception ex)
        {
            return Result.Fail<Category>(new Error($"Произошла ошибка: {ex.Message}"));
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

    public async Task<Result> Update(Category category)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(category);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("\n", validationResult.Errors);
                return Result.Fail(new ValidationError(errors));
            }

            await _repository.Update(category);

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
}