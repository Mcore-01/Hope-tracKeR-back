using Hope_tracKeR_back.Enums;

namespace Hope_tracKeR_back.Models.Entities;

public class Repair
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Description { get; set; } = null!;
    public string? Diagnosis { get; set; }
    public RepairStatus Status { get; set; }
    public int ItemId { get; set; }
    public Device Item { get; set; } = null!;
    public int AddressId { get; set; }
    public Address Address { get; set; } = null!;
}