using VolunteerMS.Utilities;
using VolunteerMS.Models;
using VolunteerMS.Services.Interfaces;
using VolunteerMS.UnitOfWorks.Interfaces;
using VolunteerMS.Models.ViewModels.Opportunity;
using VolunteerMS.Models.ViewModels.Center;
using VolunteerMS.Models.ViewModels.Skill;
using VolunteerMS.Models.ViewModels.Volunteer;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace VolunteerMS.Services;
public class VolunteerService : GenericService<Volunteer>, IVolunteerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public VolunteerService(IUnitOfWork unitOfWork, IMapper mapper)
        : base(unitOfWork.Volunteers, unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    private async Task UpdateApprovalStatusAsync(int volunteerId, ApprovalStatus status)
    {
        var volunteer = await _unitOfWork.Volunteers.GetByIdAsync(volunteerId);

        if (volunteer == null)
            throw new Exception("Volunteer not found.");

        volunteer.ApprovalStatus = status;

        _unitOfWork.Volunteers.UpdateAsync(volunteer);

        await _unitOfWork.SaveChangesAsync();
    }

    public Task<IEnumerable<Volunteer>> GetFilteredAsync(VolunteerFilter filter, string? searchTerm)
    {
        return _unitOfWork.Volunteers.GetFilteredAsync(filter, searchTerm);
    }

    public async Task<bool> CreateAsync(VolunteerCreateVM model)
    {
         var existingUser = await UnitOfWork.Users.GetByUsernameAsync(model.Username); // Check whether the username already exists

        if (existingUser != null)
        {
            return false; // Username already exists
        }

        var volunteer = _mapper.Map<Volunteer>(model);
        volunteer.ApprovalStatus = ApprovalStatus.Pending;

        // Create login account
        var hasher = new PasswordHasher<User>();

        var user = new User
        {
            Username = model.Username,
            PasswordHash = PasswordHasher.HashPassword(model.Password),

            Role = UserRole.Volunteer,
            Volunteer = volunteer
        };

        volunteer.User = user;

        await UnitOfWork.Volunteers.AddAsync(volunteer);
        await UnitOfWork.SaveChangesAsync();

        return true;
    }

    
    public async Task<VolunteerEditVM?> GetForEditAsync(int id)
    {
        var volunteer = await UnitOfWork.Volunteers.GetVolunteerWithUserAsync(id);

        if (volunteer == null)
        {
            return null;
        }

        return _mapper.Map<VolunteerEditVM>(volunteer);
    }

    public async Task<bool> UpdateAsync(VolunteerEditVM model)
    {
        //var volunteer = await UnitOfWork.Volunteers.GetByIdAsync(model.Id);
        var volunteer = await UnitOfWork.Volunteers.GetVolunteerWithUserAsync(model.Id);

        if (volunteer == null)
        {
            return false;
        }

        _mapper.Map(model, volunteer);
        UnitOfWork.Volunteers.UpdateAsync(volunteer);

        // Update the associated User entity
        volunteer.User!.Username = model.Username;
        if (!string.IsNullOrWhiteSpace(model.Password)) // Not update password if field is empty
        {
            volunteer.User!.PasswordHash = PasswordHasher.HashPassword(model.Password);
        }

        await UnitOfWork.SaveChangesAsync();

        return true;
    }

    
    public async Task<VolunteerDetailsVM?> GetDetailsAsync(int id)
    {
        var volunteer = await UnitOfWork.Volunteers
            .GetVolunteerDetailsAsync(id);

        if (volunteer == null)
        {
            return null;
        }

        return _mapper.Map<VolunteerDetailsVM>(volunteer);
    }

    public async Task<VolunteerSkillsVM?> GetVolunteerSkillsAsync(int volunteerId)
    {
        var volunteer = await UnitOfWork.Volunteers
            .GetVolunteerWithSkillsAsync(volunteerId);

        if (volunteer == null)
        {
            return null;
        }

        var allSkills = await UnitOfWork.Skills.GetAllAsync();

        var model = new VolunteerSkillsVM
        {
            VolunteerId = volunteer.Id,
            VolunteerName = $"{volunteer.FirstName} {volunteer.LastName}",
            Skills = [.. allSkills.Select(s => new SkillSelectionVM
            {
                SkillId = s.Id,
                SkillName = s.Name,
                Selected = volunteer.VolunteerSkills
                        .Any(vs => vs.SkillId == s.Id)
            })]
        };

        return model;
    }

    public async Task<VolunteerCentersVM?> GetVolunteerCentersAsync(int volunteerId)
    {
        var volunteer = await UnitOfWork.Volunteers
            .GetVolunteerWithCentersAsync(volunteerId);

        if (volunteer == null)
        {
            return null;
        }

        var allCenters = await UnitOfWork.Centers.GetAllAsync();

        var model = new VolunteerCentersVM
        {
            VolunteerId = volunteer.Id,
            VolunteerName = $"{volunteer.FirstName} {volunteer.LastName}",
            Centers = [.. allCenters.Select(c => new CenterSelectionVM
            {
                CenterId = c.Id,
                Name = c.Name,
                Selected = volunteer.VolunteerCenters
                        .Any(vs => vs.CenterId == c.Id)
            })]
        };

        return model;
    }

    public async Task<VolunteerOpportunitiesVM?> GetVolunteerOpportunitiesAsync(int volunteerId)
    {
        var volunteer = await UnitOfWork.Volunteers
            .GetVolunteerWithOpportunitiesAsync(volunteerId);

        if (volunteer == null)
        {
            return null;
        }

        //var allOpportunities = await UnitOfWork.Opportunities.GetAllAsync();
        var centerIds = volunteer.VolunteerCenters
            .Select(vc => vc.CenterId)
            .ToList();

        var opportunities = await UnitOfWork.Opportunities
            .GetByCenterIdsAsync(centerIds);

        var model = new VolunteerOpportunitiesVM
        {
            VolunteerId = volunteer.Id,
            VolunteerName = $"{volunteer.FirstName} {volunteer.LastName}",
            Opportunities = [.. opportunities.Select(o => new OpportunitySelectionVM
            {
                OpportunityId = o.Id,
                OpportunityName = o.Name,
                CenterName = o.Center?.Name ?? string.Empty,
                IsActive = o.IsActive,
                StartDate = o.StartDate,
                Selected = volunteer.VolunteerOpportunities
                        .Any(vs => vs.OpportunityId == o.Id)
            })]
        };

        return model;
    }

    
    public async Task UpdateVolunteerSkillsAsync(VolunteerSkillsVM model)
    {
        var volunteer = await UnitOfWork.Volunteers
            .GetVolunteerWithSkillsAsync(model.VolunteerId);

        if (volunteer == null)
        {
            return;
        }

        volunteer.VolunteerSkills.Clear();

        foreach (var skill in model.Skills.Where(s => s.Selected))
        {
            volunteer.VolunteerSkills.Add(new VolunteerSkill
            {
                VolunteerId = volunteer.Id,
                SkillId = skill.SkillId
            });
        }

        await UnitOfWork.SaveChangesAsync();
    }    

    
    public async Task UpdateVolunteerCentersAsync(VolunteerCentersVM model)
    {
        var volunteer = await UnitOfWork.Volunteers
            .GetVolunteerWithCentersAsync(model.VolunteerId);

        if (volunteer == null)
        {
            return;
        }

        volunteer.VolunteerCenters.Clear();

        foreach (var center in model.Centers.Where(c => c.Selected))
        {
            volunteer.VolunteerCenters.Add(new VolunteerCenter
            {
                VolunteerId = volunteer.Id,
                CenterId = center.CenterId
            });
        }

        await UnitOfWork.SaveChangesAsync();
    }


    public async Task UpdateVolunteerOpportunitiesAsync(VolunteerOpportunitiesVM model)
    {
        var volunteer = await UnitOfWork.Volunteers
            .GetVolunteerWithOpportunitiesAsync(model.VolunteerId);

        if (volunteer == null)
        {
            return;
        }

        volunteer.VolunteerOpportunities.Clear();

        foreach (var opportunity in model.Opportunities.Where(o => o.Selected))
        {
            volunteer.VolunteerOpportunities.Add(new VolunteerOpportunity
            {
                VolunteerId = volunteer.Id,
                OpportunityId = opportunity.OpportunityId
            });
        }

        await UnitOfWork.SaveChangesAsync();
    }

    // Additional methods for managing volunteer status
    public async Task ApproveAsync(int id)
    {
        await UpdateApprovalStatusAsync(id, ApprovalStatus.Approved);
    }

    public async Task DisapproveAsync(int id)
    {
        await UpdateApprovalStatusAsync(id, ApprovalStatus.Disapproved);
    }

    public async Task PendingAsync(int id)
    {
        await UpdateApprovalStatusAsync(id, ApprovalStatus.Pending);
    }
}