using VolunteerMS.Models.ViewModels.Skill;
using VolunteerMS.Models;
using VolunteerMS.Services.Interfaces;

namespace VolunteerMS.Services.Interfaces;
public interface ISkillService : IGenericService<Skill>
{
    Task<SkillIndexVM> GetAllAsync(string? searchTerm);

    //Task<SkillCreateVM> GetCreateVMAsync();

    Task<bool> CreateAsync(SkillCreateVM model);

    Task<SkillEditVM?> GetForEditAsync(int id);

    Task<bool> UpdateAsync(SkillEditVM model);

    Task<bool> DeleteSkillAsync(int id);
}