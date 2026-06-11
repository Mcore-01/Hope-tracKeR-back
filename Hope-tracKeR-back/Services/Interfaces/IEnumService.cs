using Hope_tracKeR_back.Models;

namespace Hope_tracKeR_back.Services.Interfaces;

public interface IEnumService
{
    Task<IEnumerable<Category>> GetAllCategories();
    Task<IEnumerable<Brand>> GetAllBrands();
    Task<IEnumerable<Address>> GetAllAddresses();
}