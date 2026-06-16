using FluentResults;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.Entities;

namespace Hope_tracKeR_back.Repositories.Interfaces;

public interface IRepairRepository
{
    Task<int> CreateRepair(Repair repair);
    Task<Result> CompleteRepair(CompleteRepairRequest repairRequest);
    Task<Result<Repair>> GetRepairById(int repairId);
}