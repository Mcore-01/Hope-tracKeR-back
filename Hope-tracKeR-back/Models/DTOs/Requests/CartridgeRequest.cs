namespace Hope_tracKeR_back.Models.DTOs.Requests;

public class CartridgeRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string PrinterModel { get; set; } = null!;
    public string Status { get; set; } = null!;
    public int AddressId { get; set; }
    public int BrandId { get; set; }
    public Dictionary<string, string> Attributes { get; set; } = new();
}