using Hope_tracKeR_back.Models.Entities;

namespace Hope_tracKeR_back.Repositories.Interfaces;

public interface IAddressRepository
{
    Task<IEnumerable<Address>> GetAllAddresses();
}