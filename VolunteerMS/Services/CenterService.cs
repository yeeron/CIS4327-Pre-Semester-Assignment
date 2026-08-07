using AutoMapper;
using VolunteerMS.Models;
using VolunteerMS.Models.ViewModels.Center;
using VolunteerMS.Services.Interfaces;
using VolunteerMS.UnitOfWorks.Interfaces;

namespace VolunteerMS.Services;
public class CenterService
    : GenericService<Center>, ICenterService
{
    private readonly IMapper _mapper;

    public CenterService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : base(unitOfWork.Centers, unitOfWork)
    {
        _mapper = mapper;
    }

    public async Task<CenterIndexVM> GetIndexVMAsync(string? searchTerm)
    {
        var centers = await UnitOfWork.Centers.SearchAsync(searchTerm);

        return new CenterIndexVM
        {
            SearchTerm = searchTerm,
            Centers = _mapper.Map<List<CenterListVM>>(centers)
        };
    }

    public async Task<CenterDetailsVM?> GetDetailsVMAsync(int id)
    {
        var center = await UnitOfWork.Centers.GetWithDetailsAsync(id);

        if (center == null)
            return null;

        return _mapper.Map<CenterDetailsVM>(center);
    }

    public async Task<CenterEditVM?> GetEditVMAsync(int id)
    {
        var center = await UnitOfWork.Centers.GetByIdAsync(id);

        if (center == null)
            return null;

        return _mapper.Map<CenterEditVM>(center);
    }

    public async Task<bool> CreateAsync(CenterCreateVM model)
    {       
        if (await UnitOfWork.Centers.ExistsAsync(model.Name))
            return false;

        var center = _mapper.Map<Center>(model);

        await UnitOfWork.Centers.AddAsync(center);

        await UnitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateAsync(CenterEditVM model)
    {
        var center = await UnitOfWork.Centers.GetByIdAsync(model.Id);

        if (center == null)
            return false;
        
          // Prevent duplicate center names
        if (await UnitOfWork.Centers.ExistsAsync(model.Name, model.Id))
            return false;

        center.Name = model.Name;

        UnitOfWork.Centers.UpdateAsync(center);

        await UnitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteCenterAsync(int id)
    {
        var center = await UnitOfWork.Centers.GetWithDetailsAsync(id);

        if (center == null)
            return false;

        /* Same thing as the above code, but using LINQ's Any() method instead of Count property.
        if (center.VolunteerCenters.Any())
            return false;
        */
        // Check if the center has any associated VolunteerCenters or Opportunities. Using Count property to check if there are any associated entities.
        if (center.VolunteerCenters.Count != 0)
            return false;

        if (center.Opportunities.Count != 0)
            return false;

        UnitOfWork.Centers.DeleteAsync(center);

        await UnitOfWork.SaveChangesAsync();

        return true;
    }
}