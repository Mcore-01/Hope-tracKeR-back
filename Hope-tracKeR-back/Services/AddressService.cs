using FluentResults;
using FluentValidation;
using Hope_tracKeR_back.Errors;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Hope_tracKeR_back.Services.Interfaces;

namespace Hope_tracKeR_back.Services;

public class AddressService : BaseCatalogService<Address>
{
    private readonly ICatalogRepository<Address> _repository;
    private readonly IValidator<Address> _validator;
    public AddressService(ICatalogRepository<Address> repository, IValidator<Address> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result<IEnumerable<Address>>> GetAll()
    {
        try
        {
            var addresses = await _repository.GetAll();

            return Result.Ok(addresses);
        }
        catch (Exception ex)
        {
            return Result.Fail<IEnumerable<Address>>(new Error($"Произошла ошибка: {ex.Message}"));
        }
    }

    public async Task<Result<Address>> GetById(int id)
    {
        try
        {
            var address = await _repository.GetById(id);

            if (address is null)
                return Result.Fail<Address>(new NotFoundError(nameof(Address), id));

            return Result.Ok(address);
        }
        catch (Exception ex)
        {
            return Result.Fail<Address>(new Error($"Произошла ошибка: {ex.Message}"));
        }
    }

    public async Task<Result<int>> Create(Address address)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(address);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("\n", validationResult.Errors);
                return Result.Fail(new ValidationError(errors));
            }

            var addressId = await _repository.Create(address);

            return Result.Ok(addressId);
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

    public async Task<Result> Update(Address address)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(address);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("\n", validationResult.Errors);
                return Result.Fail(new ValidationError(errors));
            }

            await _repository.Update(address);

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
