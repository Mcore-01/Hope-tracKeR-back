namespace Hope_tracKeR_back.Models.DTOs.Requests
{
    public class DeviceModify
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string SerialId { get; set; } = string.Empty;
        public string Category { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime AddedDate { get; set; }
        public int AddressId { get; set; }
        public int BrandId { get; set; }
        public Dictionary<string, string> Attributes { get; set; } = new();
    }
}
