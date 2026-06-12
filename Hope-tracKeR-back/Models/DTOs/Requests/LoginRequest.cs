namespace Hope_tracKeR_back.Models.DTOs.Requests;

public class LoginRequest
{
    public required string Login { get; set; }
    public required string Password { get; set; }
}