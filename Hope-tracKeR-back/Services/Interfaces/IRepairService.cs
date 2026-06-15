
using FluentResults;
using Hope_tracKeR_back.Models.DTOs.Requests;

namespace Hope_tracKeR_back.Services.Interfaces;

public interface IRepairService
{
    Task<Result> StartRepairItem(StartRepairRequest repairRequest);
    Task<Result> CompleteRepair(CompleteRepairRequest repairRequest);
    Task<Result<byte[]>> GenerateRepairActToDocx(int repairId);
}