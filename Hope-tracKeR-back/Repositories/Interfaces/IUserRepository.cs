using FluentResults;
using Hope_tracKeR_back.Models.Entities;

namespace Hope_tracKeR_back.Repositories.Interfaces;

public interface IUserRepository
{
    Task<Result<User>> GetUserByLogin(string login);
}