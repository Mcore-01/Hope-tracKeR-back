namespace Hope_tracKeR_back.Models.DTOs.Responses;

public class LoginResponse
{
    public int UserId { get; set; }
    public required string UserName { get; set; }
    public required string Token { get; set; }
}