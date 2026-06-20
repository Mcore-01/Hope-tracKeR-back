using Hope_tracKeR_back.Enums;

namespace Hope_tracKeR_back.Models.Entities;

public class Refill
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public RefillStatus Status { get; set; }
    public int ItemId { get; set; }
    public Cartridge Item { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int AddressId { get; set; }
    public Address Address { get; set; } = null!;
}