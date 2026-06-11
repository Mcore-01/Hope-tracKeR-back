using Hope_tracKeR_back.Models;
using Hope_tracKeR_back.Repositories.Interfaces;
using Hope_tracKeR_back.Services.Interfaces;

namespace Hope_tracKeR_back.Services
{
    public class EnumService : IEnumService
    {
        private readonly IBrandRepository _brandRepository;
        public EnumService(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }
        public async Task<IEnumerable<Brand>> GetAllBrands()
        {
            return await _brandRepository.GetAllBrands();
        }

        public Task<IEnumerable<Category>> GetAllCategories()
        {
            throw new NotImplementedException();
        }
    }
}
