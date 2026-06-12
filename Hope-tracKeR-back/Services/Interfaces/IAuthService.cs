using FluentResults;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.DTOs.Responses;

namespace Hope_tracKeR_back.Services.Interfaces;

public interface IAuthService
{
    Task<Result<LoginResponse>> Login(LoginRequest loginRequest);
}