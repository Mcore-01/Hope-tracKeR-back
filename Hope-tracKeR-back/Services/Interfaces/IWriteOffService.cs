using FluentResults;

namespace Hope_tracKeR_back.Services.Interfaces;

public interface IWriteOffService
{
    Task<Result> WriteOff(int itemId, int userId);
}