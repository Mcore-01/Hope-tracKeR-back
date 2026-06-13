using FluentResults;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.Entities;

namespace Hope_tracKeR_back.Repositories.Interfaces;

public interface IRepairRepository
{
    Task<Result> CreateRepair(StartRepairRequest repairRequest);
    Task<Result> CompleteRepair(CompleteRepairRequest repairRequest);
    Task<Result<Repair>> GetRepairById(int repairId);
}