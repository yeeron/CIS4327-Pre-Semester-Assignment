using VolunteerMS.Utilities;
using VolunteerMS.Models;

namespace VolunteerMS.Data.Repositories.Interfaces;
public interface IVolunteerRepository : IGenericRepository<Volunteer>
{
    //Task<IEnumerable<Volunteer>> GetPendingApprovalsAsync();
    //Task<IEnumerable<Volunteer>> SearchAsync(string searchTerm);
    Task<IEnumerable<Volunteer>> GetFilteredAsync(VolunteerFilter filter, string? searchTerm);
    Task<Volunteer?> GetVolunteerWithUserAsync(int id);
    Task<Volunteer?> GetVolunteerDetailsAsync(int id);
    Task<Volunteer?> GetVolunteerWithSkillsAsync(int id);
    Task<Volunteer?> GetVolunteerWithCentersAsync(int id);
    Task<Volunteer?> GetVolunteerWithOpportunitiesAsync(int id);
}