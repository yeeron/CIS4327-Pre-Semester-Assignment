using Microsoft.EntityFrameworkCore;
using VolunteerMS.Data.Repositories.Interfaces;
using VolunteerMS.Models;

namespace VolunteerMS.Data.Repositories;
public class CenterRepository : GenericRepository<Center>, ICenterRepository
{
    public CenterRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Center>> SearchAsync(string? searchTerm)
    {
        IQueryable<Center> query = _dbSet
            .Include(c => c.VolunteerCenters)
            .Include(c => c.Opportunities);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(c =>
                c.Name.ToLower().Contains(searchTerm.ToLower()) );
        }

        return await query
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Center?> GetWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(c => c.VolunteerCenters)
            .Include(c => c.Opportunities)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> ExistsAsync(string centerName)
    {
        return await _dbSet.AnyAsync(c =>
            c.Name.ToLower() == centerName.ToLower()); //c.CenterName.Equals(centerName, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<bool> ExistsAsync(string centerName, int excludeId)
    {
        return await _dbSet.AnyAsync(c =>
            c.Id != excludeId &&
            c.Name.ToLower() == centerName.ToLower());
    }
}