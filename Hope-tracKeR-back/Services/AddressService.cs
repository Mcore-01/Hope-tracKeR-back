using FluentResults;
using Hope_tracKeR_back.Errors;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Hope_tracKeR_back.Services.Interfaces;

namespace Hope_tracKeR_back.Services;

public class AddressService : ICatalogService<Address>
{
    private readonly ICatalogRepository<Address> _repository;
    public AddressService(ICatalogRepository<Address> repository)
    {
        _repository = repository;
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
                return Result.Fail<Address>(new NotFoundError(nameof(Brand), id));

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

    public Task<Result> Remove(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> Update(Address address)
    {
        throw new NotImplementedException();
    }
}
