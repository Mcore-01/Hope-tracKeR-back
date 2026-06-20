using FluentResults;
using Hope_tracKeR_back.Config;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.DTOs.Responses;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Hope_tracKeR_back.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Hope_tracKeR_back.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<LoginResponse>> Login(LoginRequest loginRequest)
    {
        var result = await _userRepository.GetUserByLogin(loginRequest.Login);

        if(result.IsFailed)
            return Result.Fail<LoginResponse>(result.Errors);

        var user = result.Value;

        if(user.Password != GetHashPassword(loginRequest.Password))
            return Result.Fail<LoginResponse>("Неправильный пароль!");

        return Result.Ok(new LoginResponse() { UserId = user.Id, UserName = user.FullName, Token = CreateToken(user) });
    }

    private string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.FullName),
            new("login", user.Login)
        };
        var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    private string GetHashPassword(string password)
    {
        byte[] bytesPassword = Encoding.ASCII.GetBytes(password);
        byte[] hashPassword = MD5.HashData(bytesPassword);

        return string.Join(string.Empty, hashPassword.Select(e => e.ToString("X2")));
    }
}
