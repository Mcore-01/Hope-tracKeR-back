namespace Hope_tracKeR_back.Models.DTOs.Requests
{
    public class DeviceRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string SerialNumber { get; set; } = string.Empty;
        public string Status { get; set; } = null!;
        public DateTime AddedDate { get; set; }
        public int AddressId { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public Dictionary<string, string> Attributes { get; set; } = new();
    }
}
