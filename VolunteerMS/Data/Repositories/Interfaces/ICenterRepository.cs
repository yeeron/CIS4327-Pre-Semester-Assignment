using VolunteerMS.Models;

namespace VolunteerMS.Data.Repositories.Interfaces;
public interface ICenterRepository : IGenericRepository<Center>
{
    Task<IEnumerable<Center>> SearchAsync(string? searchTerm);
    Task<Center?> GetWithDetailsAsync(int id);
    Task<bool> ExistsAsync(string centerName);
    Task<bool> ExistsAsync(string centerName, int excludeId);
}