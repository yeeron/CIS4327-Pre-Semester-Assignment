using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VolunteerMS.Models.ViewModels.Volunteer;
using VolunteerMS.Utilities;
using VolunteerMS.Services.Interfaces;

namespace VolunteerMS.Controllers;
public class VolunteerController : BaseController
{
    private readonly IVolunteerService _volunteerService;
    private readonly IMapper _mapper;

    public VolunteerController(IVolunteerService volunteerService, IMapper mapper)
    {
        _volunteerService = volunteerService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index( string? searchTerm, VolunteerFilter filter = VolunteerFilter.Default)
    {
        var volunteers = await _volunteerService.GetFilteredAsync(filter, searchTerm);

        var model = new VolunteerIndexVM
        {
            SearchTerm = searchTerm,
            Filter = filter,
            Volunteers = _mapper.Map<IEnumerable<VolunteerListVM>>(volunteers)
        };            

        return View(model);
    }

    // GET
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(VolunteerCreateVM model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        bool created = await _volunteerService.CreateAsync(model);

        if (!created)
        {
            ModelState.AddModelError(nameof(model.Username),
                "This username is already in use.");

            return View(model);
        }

        TempData["SuccessMessage"] = "Volunteer created successfully.";

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, string? returnUrl)
    {
        var model = await _volunteerService.GetForEditAsync(id);

        ViewBag.ReturnUrl = returnUrl;

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(VolunteerEditVM model, string? returnUrl)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        bool updated = await _volunteerService.UpdateAsync(model);

        if (!updated)
        {
            return NotFound();
        }

        TempData["SuccessMessage"] = "Volunteer updated successfully.";

        if (!string.IsNullOrEmpty(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id, string? returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;

        var model = await _volunteerService.GetDetailsAsync(id);

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    //GET
    public async Task<IActionResult> Skills(int id, string? returnUrl)
    {
        var model = await _volunteerService.GetVolunteerSkillsAsync(id);

        ViewBag.ReturnUrl = returnUrl;

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Skills(VolunteerSkillsVM model, string? returnUrl)
    {
        await _volunteerService.UpdateVolunteerSkillsAsync(model);

        TempData["SuccessMessage"] = "Volunteer skills updated successfully.";

        if (!string.IsNullOrEmpty(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction(nameof(Index));
    }

    //GET
    public async Task<IActionResult> Centers(int id, string? returnUrl)
    {
        var model = await _volunteerService.GetVolunteerCentersAsync(id);

        ViewBag.ReturnUrl = returnUrl;

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Centers(VolunteerCentersVM model, string? returnUrl)
    {
        await _volunteerService.UpdateVolunteerCentersAsync(model);

        TempData["SuccessMessage"] = "Volunteer centers updated successfully.";

        if (!string.IsNullOrEmpty(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction(nameof(Index));
    }

    //GET
    public async Task<IActionResult> Matches(int id, string? returnUrl)
    {
        var model = await _volunteerService.GetVolunteerOpportunitiesAsync(id);

        ViewBag.ReturnUrl = returnUrl;

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Matches(VolunteerOpportunitiesVM model, string? returnUrl)
    {
        await _volunteerService.UpdateVolunteerOpportunitiesAsync(model);

        TempData["SuccessMessage"] = "Volunteer matches updated successfully.";

        if (!string.IsNullOrEmpty(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int id, string? returnUrl)
    {
        await _volunteerService.ApproveAsync(id);

        SetSuccessMessage("Volunteer approved.");

        if (!string.IsNullOrEmpty(returnUrl))
        return Redirect(returnUrl);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Disapprove(int id, string? returnUrl)
    {
        await _volunteerService.DisapproveAsync(id);

        SetSuccessMessage("Volunteer disapproved.");

        if (!string.IsNullOrEmpty(returnUrl))
        return Redirect(returnUrl);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Pending(int id, string? returnUrl)
    {
        await _volunteerService.PendingAsync(id);

        SetSuccessMessage("Volunteer set to pending.");

        if (!string.IsNullOrEmpty(returnUrl))
        return Redirect(returnUrl);

        return RedirectToAction(nameof(Index));
    }
}