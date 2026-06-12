
using FluentResults;
using Hope_tracKeR_back.Models.DTOs.Requests;

namespace Hope_tracKeR_back.Services.Interfaces;

public interface IRepairService
{
    public Task<Result> StartRepairItem(StartRepairRequest repairRequest);
    public Task<Result> CompleteRepairItem(CompleteRepairRequest repairRequest);
}