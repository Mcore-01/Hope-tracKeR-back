namespace Hope_tracKeR_back.Models.DTOs.Requests;

public class StartRepairRequest
{
    public int ItemId { get; set; }
    public int CurrentAddressId { get; set; }
    public int UserId { get; set; }
    public string DescriptionFailure { get; set; } = null!;
    public DateTime StartDate { get; set; }
}