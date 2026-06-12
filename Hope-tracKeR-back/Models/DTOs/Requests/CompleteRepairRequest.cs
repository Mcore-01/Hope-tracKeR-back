namespace Hope_tracKeR_back.Models.DTOs.Requests;

public class CompleteRepairRequest
{
    public int ItemId { get; set; }
    public int CurrentAddressId { get; set; }
    public int UserId { get; set; }
    public string Diagnosis { get; set; } = null!;
    public DateTime EndDate { get; set; }
}