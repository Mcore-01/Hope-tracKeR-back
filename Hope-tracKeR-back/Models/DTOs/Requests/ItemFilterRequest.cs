using Hope_tracKeR_back.Enums;

namespace Hope_tracKeR_back.Models.DTOs.Requests;

public class ItemFilterRequest
{
    public string? SearchField { get; set; }  
    public string? Status { get; set; }
    public DateTime? AddedDateFrom { get; set; }
    public DateTime? AddedDateTo { get; set; }

    public string? Branch { get; set; }
    public string? Building { get; set; }
    public int? Floor { get; set; }
    public string? Room { get; set; }
    public int? AddressType { get; set; }
    public int? BrandId { get; set; }

    public string? PrinterModel { get; set; }

    public Dictionary<string, string>? Attributes { get; set; }

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}