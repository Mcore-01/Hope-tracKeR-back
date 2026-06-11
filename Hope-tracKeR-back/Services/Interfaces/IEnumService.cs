using Hope_tracKeR_back.Models.Entities;

namespace Hope_tracKeR_back.Services.Interfaces;

public interface IEnumService
{
    Task<IEnumerable<Brand>> GetAllBrands();
    Task<IEnumerable<Address>> GetAllAddresses();
}