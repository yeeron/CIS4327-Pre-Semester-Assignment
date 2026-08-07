using VolunteerMS.Models;
using VolunteerMS.Models.ViewModels.Center;

namespace VolunteerMS.Services.Interfaces;
public interface ICenterService : IGenericService<Center>
{
    Task<CenterIndexVM> GetIndexVMAsync(string? searchTerm);

    Task<CenterDetailsVM?> GetDetailsVMAsync(int id);

    Task<CenterEditVM?> GetEditVMAsync(int id);

    Task<bool> CreateAsync(CenterCreateVM model);

    Task<bool> UpdateAsync(CenterEditVM model);

    Task<bool> DeleteCenterAsync(int id);
}