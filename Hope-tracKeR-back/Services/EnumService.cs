using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Hope_tracKeR_back.Services.Interfaces;

namespace Hope_tracKeR_back.Services
{
    public class EnumService : IEnumService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IAddressRepository _addressRepository;
        public EnumService(IBrandRepository brandRepository, IAddressRepository addressRepository)
        {
            _brandRepository = brandRepository;
            _addressRepository = addressRepository;
        }

        public async Task<IEnumerable<Address>> GetAllAddresses()
        {
            return await _addressRepository.GetAllAddresses();
        }

        public async Task<IEnumerable<Brand>> GetAllBrands()
        {
            return await _brandRepository.GetAllBrands();
        }
    }
}
