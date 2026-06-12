using FluentResults;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.Entities;

namespace Hope_tracKeR_back.Repositories.Interfaces;

public interface IBrandRepository
{
    Task<Result<IEnumerable<Brand>>> GetAllBrands();
    Task<Result<Brand>> GetBrandById(int id);
    Task<Result<int>> CreateBrand(BrandDto brand);
    Task<Result> UpdateBrand(BrandDto brand);
    Task<Result> RemoveBrand(int id);
}