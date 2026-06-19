namespace Hope_tracKeR_back.Models.Entities;

public class Issuance
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int ItemId { get; set; }
    public Device Item { get; set; } = null!;
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
