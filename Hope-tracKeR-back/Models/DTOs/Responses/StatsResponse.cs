namespace Hope_tracKeR_back.Models.DTOs.Responses;

public class StatsResponse
{
    public required int Total { get; set; }
    public required Dictionary<string, int> StatusCounts { get; set; }
}