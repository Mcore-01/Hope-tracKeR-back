namespace Hope_tracKeR_back.Models.DTOs.Responses;

public class AddressResponse
{
    public int Id { get; set; }
    public string Branch { get; set; } = null!;
    public string Building { get; set; } = null!;
    public int Floor { get; set; }
    public string Room { get; set; } = null!;
}