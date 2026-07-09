namespace Hope_tracKeR_back.Models.DTOs.Responses;

public class PagedListResponse<T> where T : class
{
    public required int PageNumber { get; set; }
    public required int PageSize { get; set; }
    public required IEnumerable<T> Items { get; set; }
}