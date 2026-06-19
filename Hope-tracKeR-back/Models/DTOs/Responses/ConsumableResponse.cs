namespace Hope_tracKeR_back.Models.DTOs.Responses;

public class ConsumableResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Quantity { get; set; }
    public int AddressId { get; set; }
    public string Address { get; set; } = null!;
    public int BrandId { get; set; }
    public string Brand { get; set; } = null!;
    public Dictionary<string, string> Attributes { get; set; } = new();
}