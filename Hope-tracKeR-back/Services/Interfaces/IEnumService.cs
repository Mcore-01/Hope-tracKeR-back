using FluentResults;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.DTOs.Responses;

namespace Hope_tracKeR_back.Services.Interfaces;

public interface IEnumService
{
    Task<Result<IEnumerable<AddressResponseDto>>> GetAllAddresses();
    Task<Result<IEnumerable<BrandDto>>> GetAllBrands();
    Task<Result<BrandDto>> GetBrandById(int id);
    Task<Result<int>> CreateBrand(BrandDto brand);
    Task<Result> UpdateBrand(BrandDto brand);
    Task<Result> RemoveBrand(int id);
}