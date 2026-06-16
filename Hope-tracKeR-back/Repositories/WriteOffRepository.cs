using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;

namespace Hope_tracKeR_back.Repositories;

public class WriteOffRepository : IWriteOffRepository
{
    private readonly HTContext _context;
    public WriteOffRepository(HTContext context)
    {
        _context = context;
    }

    public async Task<int> Create(WriteOff write)
    {
        await _context.WriteOffs.AddAsync(write);
        await _context.SaveChangesAsync();

        return write.Id;
    }
}