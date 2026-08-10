using AutoMapper;
using VolunteerMS.Models.ViewModels.Skill;
using VolunteerMS.Models;
using VolunteerMS.Services.Interfaces;
using VolunteerMS.UnitOfWorks.Interfaces;

namespace VolunteerMS.Services;
public class SkillService
    : GenericService<Skill>, ISkillService
{
    private readonly IMapper _mapper;

    public SkillService(IUnitOfWork unitOfWork, IMapper mapper)
        : base(unitOfWork.Skills, unitOfWork)
    {
        _mapper = mapper;
    }

    public async Task<SkillIndexVM> GetAllAsync(string? searchTerm)
    {
        var skills = await UnitOfWork.Skills.SearchAsync(searchTerm);

        return new SkillIndexVM
        {
            SearchTerm = searchTerm,
            Skills = _mapper.Map<List<SkillListVM>>(skills)
        };
    }

    // Get the SkillCreateVM for creating a new skill
    /*public async Task<SkillCreateVM> GetCreateVMAsync()
    {
        return await Task.FromResult(new SkillCreateVM());
    }*/

    public async Task<bool> CreateAsync(SkillCreateVM model)
    {
        var exists = (await UnitOfWork.Skills.SearchAsync(model.Name))
            .Any(s => s.Name.ToLower() == model.Name.ToLower()); // Check for existing skill with the same name (case-insensitive)

        if (exists)
            return false;

        var skill = _mapper.Map<Skill>(model);

        await UnitOfWork.Skills.AddAsync(skill);

        await UnitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<SkillEditVM?> GetForEditAsync(int id)
    {
        var skill = await UnitOfWork.Skills.GetByIdAsync(id);

        if (skill == null)
            return null;

        return _mapper.Map<SkillEditVM>(skill);
    }

    public async Task<bool> UpdateAsync(SkillEditVM model)
    {
        var skill = await UnitOfWork.Skills.GetByIdAsync(model.Id);

        if (skill == null)
            return false;
        
          // Prevent duplicate center names
        if (await UnitOfWork.Skills.ExistsAsync(model.Name, model.Id))
            return false;

        skill.Name = model.Name;

        UnitOfWork.Skills.UpdateAsync(skill);

        await UnitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteSkillAsync(int id)
    {
        var skill = await UnitOfWork.Skills.GetByIdAsync(id);

        if (skill == null)
            return false;

        if (skill.VolunteerSkills.Any())
            return false;

        UnitOfWork.Skills.DeleteAsync(skill);

        await UnitOfWork.SaveChangesAsync();

        return true;
    }
}