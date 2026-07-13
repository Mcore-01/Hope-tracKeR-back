using FluentResults;
using Hope_tracKeR_back.Models.DTOs.Responses;

namespace Hope_tracKeR_back.Services.Interfaces;

public interface IStatsService
{
    Task<Result<StatsResponse>> GetDevicesStats();
}