using Microsoft.EntityFrameworkCore;
using VolunteerMS.Data.Repositories.Interfaces;
using VolunteerMS.Models;

namespace VolunteerMS.Data.Repositories;
public class OpportunityRepository : GenericRepository<Opportunity>, IOpportunityRepository
{
    public OpportunityRepository(AppDbContext context)
        : base(context)
    {
    }

   public async Task<IEnumerable<Opportunity>> GetByCenterIdsAsync(IEnumerable<int> centerIds)
    {
        return await _dbSet
            .Include(o => o.Center)
            .Where(o => centerIds.Contains(o.CenterId))
            .ToListAsync();
    }
}