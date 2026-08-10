using VolunteerMS.Models;

namespace VolunteerMS.Data.Repositories.Interfaces;
public interface ISkillRepository : IGenericRepository<Skill>
{
     Task<IEnumerable<Skill>> SearchAsync(string? searchTerm);
     Task<bool> ExistsAsync(string skillName, int excludeId);
}