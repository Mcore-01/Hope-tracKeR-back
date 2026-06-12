using FluentResults;
using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hope_tracKeR_back.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly HTContext _context;
    public AddressRepository(HTContext context)
    {
        _context = context;
    }
    public async Task<Result<IEnumerable<Address>>> GetAllAddresses()
    {
        try
        {
            var addresses = await _context.Addresses.ToListAsync();
            return  Result.Ok<IEnumerable<Address>>(addresses);
        }
        catch (Exception ex)
        {
            return Result.Fail<IEnumerable<Address>>(new Error("Ошибка базы данных!").CausedBy(ex));
        }
    }
}