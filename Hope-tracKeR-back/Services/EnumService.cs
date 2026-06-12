using FluentResults;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.DTOs.Responses;
using Hope_tracKeR_back.Repositories.Interfaces;
using Hope_tracKeR_back.Services.Interfaces;

namespace Hope_tracKeR_back.Services;

public class EnumService : IEnumService
{
    private readonly IBrandRepository _brandRepository;
    private readonly IAddressRepository _addressRepository;
    public EnumService(IBrandRepository brandRepository, IAddressRepository addressRepository)
    {
        _brandRepository = brandRepository;
        _addressRepository = addressRepository;
    }

    public async Task<Result<IEnumerable<AddressResponseDto>>> GetAllAddresses()
    {
        var result = await _addressRepository.GetAllAddresses();

        if (result.IsFailed)
            return Result.Fail<IEnumerable<AddressResponseDto>>(result.Errors);

        return Result.Ok(result.Value.Select(a => new AddressResponseDto() { Id = a.Id, Branch = a.Branch, Building = a.Building, Floor = a.Floor, Room = a.Room }));
    }
    
    public async Task<Result<IEnumerable<BrandDto>>> GetAllBrands()
    {
        var result = await _brandRepository.GetAllBrands();

        if (result.IsFailed)
            return Result.Fail<IEnumerable<BrandDto>>(result.Errors);

        return Result.Ok(result.Value.Select(b => new BrandDto() { Id = b.Id, Name = b.Name}));
    }

    public async Task<Result<BrandDto>> GetBrandById(int id)
    {
        var result = await _brandRepository.GetBrandById(id);

        if (result.IsFailed)
            return Result.Fail<BrandDto>(result.Errors);

        var brand = result.Value;

        return Result.Ok(new BrandDto() { Id = brand.Id, Name = brand.Name });
    }

    public Task<Result<int>> CreateBrand(BrandDto brand)
    {
        return _brandRepository.CreateBrand(brand); 
    }
   
    public Task<Result> UpdateBrand(BrandDto brand)
    {
        return _brandRepository.UpdateBrand(brand);
    }

    public Task<Result> RemoveBrand(int id)
    {
        return _brandRepository.RemoveBrand(id);
    }
}