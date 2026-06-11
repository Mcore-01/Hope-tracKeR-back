namespace Hope_tracKeR_back.Models.DTOs.Responses;

public class ItemResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string SerialId { get; set; } = string.Empty;
    public string Category { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime AddedDate { get; set; }
    public int AddressId { get; set; }
    public string Address { get; set; } = null!;
    public int BrandId { get; set; }
    public string Brand { get; set; } = null!;
    public Dictionary<string, string> Attributes { get; set; } = new();
}