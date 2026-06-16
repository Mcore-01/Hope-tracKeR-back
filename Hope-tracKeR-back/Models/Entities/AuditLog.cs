namespace Hope_tracKeR_back.Models.Entities;

public class AuditLog
{
    public int Id { get; set; }
    public DateTime Date { get; set; } 
    public string UserLogin { get; set; }
    public string Action { get; set; }
    public string EntityName { get; set; }
    public string EntityId { get; set; }
    public string? OldValues { get; set; }
    public string NewValues { get; set; }
}