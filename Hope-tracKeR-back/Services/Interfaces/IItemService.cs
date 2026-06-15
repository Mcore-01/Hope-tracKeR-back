using FluentResults;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.DTOs.Responses;

namespace Hope_tracKeR_back.Services.Interfaces;

public interface IItemService : IRepairService
{
    Task<Result<IEnumerable<DeviceResponse>>> GetByFilters(ItemFilter filter);
    Task<Result<DeviceResponse>> GetById(int id);
    Task<Result<int>> CreateItem(DeviceModify item);
    Task<Result> Update(DeviceModify item);
    Task<Result> Remove(int id);
    Task<Result<Byte[]>> ExportDevicesToExcel(ItemFilter filter);
}