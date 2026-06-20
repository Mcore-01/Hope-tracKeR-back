namespace Hope_tracKeR_back.Models.DTOs.Requests;

public class StartRefillRequest
{
    public int CartridgeId { get; set; }
    public int AddressId { get; set; }
    public int UserId { get; set; }
    public DateTime StartDate { get; set; } = DateTime.Now;
}