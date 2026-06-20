namespace Hope_tracKeR_back.Models.DTOs.Responses;

public class CartridgeResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string PrinterModel { get; set; } = null!;
    public string Status { get; set; } = null!;
    public int AddressId { get; set; }
    public string Address { get; set; } = null!;
    public int BrandId { get; set; }
    public string Brand { get; set; } = null!;
    public Dictionary<string, string> Attributes { get; set; } = new();
}