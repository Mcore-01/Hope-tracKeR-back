using Hope_tracKeR_back.Enums;

namespace Hope_tracKeR_back.Models.DTOs.Requests;

public class ItemFilterDto
{
    public string? SearchField { get; set; }  
    public ItemCategory? Category { get; set; }
    public ItemStatus? Status { get; set; }

    public DateTime? AddedDateFrom { get; set; }
    public DateTime? AddedDateTo { get; set; }

    public int? AddressId { get; set; }
    public int? BrandId { get; set; }

    public Dictionary<string, string>? Attributes { get; set; }
}