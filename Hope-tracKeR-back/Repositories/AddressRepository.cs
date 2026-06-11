using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hope_tracKeR_back.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly HTContext _context;
        public AddressRepository(HTContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Address>> GetAllAddresses()
        {
            return await _context.Addresses.ToListAsync();
        }
    }
}
