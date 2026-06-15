using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hope_tracKeR_back.Repositories;

public class AddressRepository : ICatalogRepository<Address>
{
    private readonly HTContext _context;
    public AddressRepository(HTContext context)
    {
        _context = context;
    }

    public async Task<int> Create(Address address)
    {
        var isExist = _context.Addresses.Any(a => a.Branch == address.Branch 
            && a.Building == address.Building && a.Floor == address.Floor && a.Room == address.Room);

        if (isExist)
            throw new InvalidOperationException("Такой адрес существует");

        await _context.Addresses.AddAsync(address);
        await _context.SaveChangesAsync();

        return address.Id;
    }

    public async Task<IEnumerable<Address>> GetAll()
    {
        var addresses = await _context.Addresses.ToListAsync();
        return addresses;
    }

    public async Task<Address?> GetById(int id)
    {
        var address = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == id);

        if (address == default)
            throw new NullReferenceException($"Объект с ID {id} не найден!");

        return address;
    }

    public async Task Remove(int id)
    {
        var existingAddress = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == id);

        if (existingAddress == default)
            throw new NullReferenceException($"Объект с ID {id} не найден!");

        _context.Addresses.Remove(existingAddress);

        await _context.SaveChangesAsync();
    }

    public async Task Update(Address address)
    {
        var isExist = _context.Addresses.Any(a => a.Branch == address.Branch
            && a.Building == address.Building && a.Floor == address.Floor && a.Room == address.Room);

        if (isExist)
            throw new InvalidOperationException("Такой адрес существует");

        var addressIsExist = _context.Addresses.Any(b => b.Id == address.Id);

        if (!addressIsExist)
            throw new NullReferenceException($"Объект с ID {address.Id} не найден!");

        _context.Addresses.Update(address);

        await _context.SaveChangesAsync();
    }
}