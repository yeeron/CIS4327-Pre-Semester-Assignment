using VolunteerMS.Models.ViewModels.Volunteer;
using VolunteerMS.Utilities;
using VolunteerMS.Models;

namespace VolunteerMS.Services.Interfaces;
public interface IVolunteerService : IGenericService<Volunteer>
{
    //Task<IEnumerable<Volunteer>> GetPendingApprovalsAsync();
    //Task<IEnumerable<Volunteer>> SearchAsync(string searchTerm);
    Task<IEnumerable<Volunteer>> GetFilteredAsync(VolunteerFilter filter, string? searchTerm);
    Task<bool> CreateAsync(VolunteerCreateVM model);  
    Task<bool> UpdateAsync(VolunteerEditVM model);  
    Task<VolunteerEditVM?> GetForEditAsync(int id);
    Task<VolunteerDetailsVM?> GetDetailsAsync(int id);
    Task<VolunteerSkillsVM?> GetVolunteerSkillsAsync(int volunteerId);
    Task<VolunteerCentersVM?> GetVolunteerCentersAsync(int volunteerId);
    Task<VolunteerOpportunitiesVM?> GetVolunteerOpportunitiesAsync(int volunteerId);
    Task UpdateVolunteerSkillsAsync(VolunteerSkillsVM model);       
    Task UpdateVolunteerCentersAsync(VolunteerCentersVM model); 
    Task UpdateVolunteerOpportunitiesAsync(VolunteerOpportunitiesVM model);

    // Additional methods for managing volunteer status    
    Task ApproveAsync(int id);
    Task DisapproveAsync(int id);
    Task PendingAsync(int id);
}