using FluentResults;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Services.Interfaces;

namespace Hope_tracKeR_back.Services;

public class AddressService : ICatalogService<Address>
{
    public Task<Result<int>> Create(Address value)
    {
        throw new NotImplementedException();
    }

    /*public async Task<Result<IEnumerable<AddressResponse>>> GetAllAddresses()
    {
        var result = await _addressRepository.GetAllAddresses();

        if (result.IsFailed)
            return Result.Fail<IEnumerable<AddressResponse>>(result.Errors);

        return Result.Ok(result.Value.Select(a => new AddressResponse() { Id = a.Id, Branch = a.Branch, Building = a.Building, Floor = a.Floor, Room = a.Room }));
    }*/

    public Task<Result<IEnumerable<Address>>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<Result<Address>> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> Remove(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> Update(Address value)
    {
        throw new NotImplementedException();
    }
}
