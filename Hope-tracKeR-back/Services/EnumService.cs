using Hope_tracKeR_back.Models;
using Hope_tracKeR_back.Repositories.Interfaces;
using Hope_tracKeR_back.Services.Interfaces;

namespace Hope_tracKeR_back.Services
{
    public class EnumService : IEnumService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly ICategoryRepository _categoryRepository;
        public EnumService(IBrandRepository brandRepository, ICategoryRepository categoryRepository)
        {
            _brandRepository = brandRepository;
            _categoryRepository = categoryRepository;
        }
        public async Task<IEnumerable<Brand>> GetAllBrands()
        {
            return await _brandRepository.GetAllBrands();
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _categoryRepository.GetAllCategories();
        }
    }
}
