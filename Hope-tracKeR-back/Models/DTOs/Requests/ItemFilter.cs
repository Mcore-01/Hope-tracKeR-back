using Hope_tracKeR_back.Enums;

namespace Hope_tracKeR_back.Models.DTOs.Requests;

public class ItemFilter
{
    public string? SearchField { get; set; }  
    public string? Status { get; set; }
    public DateTime? AddedDateFrom { get; set; }
    public DateTime? AddedDateTo { get; set; }

    public int? AddressId { get; set; }
    public int? BrandId { get; set; }

    public string? PrinterModel { get; set; }

    public Dictionary<string, string>? Attributes { get; set; }

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}