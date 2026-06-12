using FluentResults;
using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hope_tracKeR_back.Repositories;

public class UserRepository : IUserRepository
{
    private readonly HTContext _context;
    public UserRepository(HTContext context)
    {
        _context = context;
    }

    public async Task<Result<User>> GetUserByLogin(string login)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);

            if (user == default)
                return Result.Fail<User>(new Error("Пользователь не найден!"));

            return Result.Ok(user);  
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Ошибка базы данных!").CausedBy(ex));
        }
    }
}