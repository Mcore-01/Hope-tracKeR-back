using FluentResults;
using Hope_tracKeR_back.Models.DTOs.Requests;

namespace Hope_tracKeR_back.Repositories.Interfaces;

public interface IRepairRepository
{
    Task<Result> CreateRepair(StartRepairRequest repairRequest);
    Task<Result> CompleteRepair(CompleteRepairRequest repairRequest);
}