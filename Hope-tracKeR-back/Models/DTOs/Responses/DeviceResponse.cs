namespace Hope_tracKeR_back.Models.DTOs.Responses;

public class DeviceResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string SerialId { get; set; } = string.Empty;
    public string Status { get; set; } = null!;
    public DateTime AddedDate { get; set; }
    public int AddressId { get; set; }
    public string Address { get; set; } = null!;
    public int BrandId { get; set; }
    public string Brand { get; set; } = null!;
    public int CategoryId { get; set; }
    public string Category { get; set; }
    public int? EmployeeId { get; set; }
    public string? Employee { get; set; }
    public Dictionary<string, string> Attributes { get; set; } = new();
}