using Hope_tracKeR_back.Enums;

namespace Hope_tracKeR_back.Models.Entities;

public class Cartridge : Item
{
    public CartridgeStatus Status { get; set; }
    public string PrinterModel { get; set; } = null!;
}