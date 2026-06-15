using Hope_tracKeR_back.Enums;

namespace Hope_tracKeR_back.Models.Entities;

public class Device : Item
{
    public string SerialNumber { get; set; } = string.Empty;
    public DeviceStatus Status { get; set; }
    public DateTime AddedDate { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}