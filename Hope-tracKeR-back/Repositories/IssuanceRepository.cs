using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;

namespace Hope_tracKeR_back.Repositories;

public class IssuanceRepository : IIssuanceRepository
{
    private readonly HTContext _context;

    public IssuanceRepository(HTContext context)
    {
        _context = context;
    }

    public async Task<int> Create(Issuance issuance)
    {
        await _context.Issuances.AddAsync(issuance);
        await _context.SaveChangesAsync();

        return issuance.Id;
    }
}