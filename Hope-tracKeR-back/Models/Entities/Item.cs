namespace Hope_tracKeR_back.Models.Entities;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int AddressId { get; set; }
    public Address Address { get; set; } = null!;
    public int BrandId { get; set; }
    public Brand Brand { get; set; } = null!;
    
    public ICollection<ItemAttribute> Attributes { get; set; } = new List<ItemAttribute>();
}