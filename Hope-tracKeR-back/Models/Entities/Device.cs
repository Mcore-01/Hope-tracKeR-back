using Hope_tracKeR_back.Enums;

namespace Hope_tracKeR_back.Models.Entities;

public class Device : Item
{
    public string SerialId { get; set; } = string.Empty;
    public DeviceStatus Status { get; set; }
    public DateTime AddedDate { get; set; }
}