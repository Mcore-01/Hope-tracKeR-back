using Hope_tracKeR_back.Enums;

namespace Hope_tracKeR_back.Models.Entities;

public class Address
{
    public int Id { get; set; }
    public string Branch { get; set; } = null!;
    public string Building { get; set; } = null!;
    public int Floor { get; set; }
    public string Room { get; set; } = null!;
    public AddressType AddressType { get; set; }
}