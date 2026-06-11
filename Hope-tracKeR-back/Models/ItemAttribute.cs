namespace Hope_tracKeR_back.Models;

public class ItemAttribute
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public Item Item { get; set; }
    public string Name { get; set; } = null!;
    public string Value { get; set; } = null!;
}