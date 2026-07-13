using Hope_tracKeR_back.Models.DTOs.Responses;

namespace Hope_tracKeR_back.Repositories.Interfaces;

public interface IStatsRepository
{
    Task<StatsResponse> GetDevicesStats();
}