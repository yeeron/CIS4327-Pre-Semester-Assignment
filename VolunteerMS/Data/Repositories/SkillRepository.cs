using Microsoft.EntityFrameworkCore;
using VolunteerMS.Data.Repositories.Interfaces;
using VolunteerMS.Models;

namespace VolunteerMS.Data.Repositories;
public class SkillRepository : GenericRepository<Skill>, ISkillRepository
{
    public SkillRepository(AppDbContext context)
        : base(context)
    {
    }
    
    public async Task<IEnumerable<Skill>> SearchAsync(string? searchTerm)
    {
        IQueryable<Skill> query = _dbSet
            .Include(s => s.VolunteerSkills);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(s =>s.Name.ToLower().Contains(searchTerm.ToLower()));
        }

        return await query
            .OrderBy(s => s.Name)
            .ToListAsync();        
    }

    public async Task<bool> ExistsAsync(string skillName, int excludeId)
    {
        return await _dbSet.AnyAsync(c =>
            c.Id != excludeId &&
            c.Name.ToLower() == skillName.ToLower());
    }
}