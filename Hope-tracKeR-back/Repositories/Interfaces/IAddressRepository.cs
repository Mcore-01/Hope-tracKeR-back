using Hope_tracKeR_back.Models;

namespace Hope_tracKeR_back.Repositories.Interfaces;

public interface IAddressRepository
{
    Task<IEnumerable<Address>> GetAllAddresses();
}