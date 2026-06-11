using Hope_tracKeR_back.Enums;

namespace Hope_tracKeR_back.Models;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string SerialId { get; set; } = string.Empty;
    public ItemCategory Category { get; set; }
    public ItemStatus Status { get; set; }
    public DateTime AddedDate { get; set; }

    public int AddressId { get; set; }
    public Address Address { get; set; } =null!;
    public int BrandId { get; set; }
    public Brand Brand { get; set; } = null!;

    public ICollection<ItemAttribute> Attributes { get; set; } = new List<ItemAttribute>();
}