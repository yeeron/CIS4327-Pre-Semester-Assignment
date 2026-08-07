
using VolunteerMS.Models;

namespace VolunteerMS.Data.Repositories.Interfaces;
public interface IOpportunityRepository : IGenericRepository<Opportunity>
{
    Task<IEnumerable<Opportunity>> GetByCenterIdsAsync(IEnumerable<int> centerIds);
}