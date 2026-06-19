namespace Hope_tracKeR_back.Models.DTOs.Requests;

public class ConsumableRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Quantity { get; set; }
    public int AddressId { get; set; }
    public int BrandId { get; set; }
    public Dictionary<string, string> Attributes { get; set; } = new();
}