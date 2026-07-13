using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Models.DTOs.Responses;
using Hope_tracKeR_back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hope_tracKeR_back.Repositories;

public class StatsRepository : IStatsRepository
{
    private readonly HTContext _context;
    public StatsRepository(HTContext context)
    {
        _context = context;
    }
    public async Task<StatsResponse> GetDevicesStats()
    {
        var total = await _context.Devices.CountAsync();

        var statusCounts = await _context.Devices
            .GroupBy(d => d.Status)
            .Select(g => new { Status = g.Key.ToString(), Count = g.Count() })
            .ToDictionaryAsync(g => g.Status, g => g.Count);

        return new StatsResponse { Total = total, StatusCounts = statusCounts };
    }
}