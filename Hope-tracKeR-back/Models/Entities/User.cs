using Hope_tracKeR_back.Enums;

namespace Hope_tracKeR_back.Models.Entities;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
    public UserRole Role { get; set; }
}