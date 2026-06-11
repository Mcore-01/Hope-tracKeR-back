using Hope_tracKeR_back.Enums;
using System.Collections.ObjectModel;

namespace Hope_tracKeR_back.Models;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string SerialId { get; set; }
    public ItemStatus Status { get; set; }
    public DateTime AddedDate { get; set; }

    public int AddressId { get; set; }
    public Address Address { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public int BrandId { get; set; }
    public Brand Brand { get; set; }

    public ICollection<ItemAttribute> Attributes { get; set; } = new List<ItemAttribute>();
}
