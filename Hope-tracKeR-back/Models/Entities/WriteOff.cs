namespace Hope_tracKeR_back.Models.Entities;

public class WriteOff
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int ItemId { get; set; }
    public Device Item { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}