using Microsoft.EntityFrameworkCore;
using VolunteerMS.Utilities;
using VolunteerMS.Models;
using VolunteerMS.Data.Repositories.Interfaces;

namespace VolunteerMS.Data.Repositories;
public class VolunteerRepository : GenericRepository<Volunteer>, IVolunteerRepository
{
    public VolunteerRepository(AppDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Volunteer>> GetFilteredAsync(
    VolunteerFilter filter,
    string? searchTerm)
    {
        IQueryable<Volunteer> query = _dbSet;

        switch (filter)
        {
            case VolunteerFilter.Approved:
                query = query.Where(v =>
                    v.ApprovalStatus == ApprovalStatus.Approved);
                break;

            case VolunteerFilter.Pending:
                query = query.Where(v =>
                    v.ApprovalStatus == ApprovalStatus.Pending);
                break;

            case VolunteerFilter.Disapproved:
                query = query.Where(v =>
                    v.ApprovalStatus == ApprovalStatus.Disapproved);
                break;

            case VolunteerFilter.Default:
                query = query.Where(v =>
                    v.ApprovalStatus == ApprovalStatus.Approved ||
                    v.ApprovalStatus == ApprovalStatus.Pending);
                break;

            case VolunteerFilter.All:
            default:
                break;
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.Trim().ToLower();

            query = query.Where(v =>
                v.FirstName.ToLower().Contains(searchTerm) ||
                v.LastName.ToLower().Contains(searchTerm) ||
                (v.Email != null && v.Email.ToLower().Contains(searchTerm)) ||
                (v.CellPhone != null && v.CellPhone.ToLower().Contains(searchTerm)));
        }

        return await query
            .OrderBy(v => v.LastName)
            .ThenBy(v => v.FirstName)
            .ToListAsync();
    }

    public async Task<Volunteer?> GetVolunteerWithUserAsync(int id)
    {
        return await _dbSet
            .Include(v => v.User)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<Volunteer?> GetVolunteerDetailsAsync(int id)
    {
        return await _dbSet
            .Include(v => v.User)
            .Include(v => v.VolunteerSkills)
                .ThenInclude(vs => vs.Skill)
            .Include(v => v.VolunteerCenters)
                .ThenInclude(vc => vc.Center)
            .Include(v => v.VolunteerOpportunities)
                .ThenInclude(vo => vo.Opportunity)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<Volunteer?> GetVolunteerWithSkillsAsync(int id)
    {
        return await _dbSet
            .Include(v => v.VolunteerSkills)
            .ThenInclude(vs => vs.Skill)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<Volunteer?> GetVolunteerWithCentersAsync(int id)
    {
        return await _dbSet
            .Include(v => v.VolunteerCenters)
                .ThenInclude(vc => vc.Center)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<Volunteer?> GetVolunteerWithOpportunitiesAsync(int id)
    {
        return await _dbSet
            .Include(v => v.VolunteerCenters)
                .ThenInclude(vc => vc.Center)

            .Include(v => v.VolunteerOpportunities)
                .ThenInclude(vo => vo.Opportunity)

            .FirstOrDefaultAsync(v => v.Id == id);
    }
}