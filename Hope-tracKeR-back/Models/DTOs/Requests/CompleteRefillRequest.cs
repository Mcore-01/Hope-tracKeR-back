namespace Hope_tracKeR_back.Models.DTOs.Requests;

public class CompleteRefillRequest
{
    public int CartridgeId { get; set; }
    public int AddressId { get; set; }
    public int UserId { get; set; }
    public DateTime EndDate { get; set; } = DateTime.Now;
}