using FluentResults;
using Hope_tracKeR_back.Models.DTOs.Responses;
using Hope_tracKeR_back.Repositories.Interfaces;
using Hope_tracKeR_back.Services.Interfaces;

namespace Hope_tracKeR_back.Services;

public class StatsService : IStatsService
{
    private readonly IStatsRepository _repository;
    public StatsService(IStatsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<StatsResponse>> GetDevicesStats()
    {
        try
        {
            var stats = await _repository.GetDevicesStats();
            return Result.Ok(stats);
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error($"Произошла ошибка: {ex.Message}"));
        }
    }
}