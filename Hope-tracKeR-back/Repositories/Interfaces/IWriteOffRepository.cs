using Hope_tracKeR_back.Models.Entities;

namespace Hope_tracKeR_back.Repositories.Interfaces;

public interface IWriteOffRepository
{
    Task<int> Create(WriteOff write);
}