using Microsoft.AspNetCore.Mvc;
using VolunteerMS.Models.ViewModels.Skill;
using VolunteerMS.Services.Interfaces;

namespace VolunteerMS.Controllers;
public class SkillController : BaseController
{
    private readonly ISkillService _skillService;

    public SkillController(ISkillService skillService)
    {
        _skillService = skillService;
    }

    public async Task<IActionResult> Index(string? searchTerm, string? returnUrl)
    {
        var model = await _skillService.GetAllAsync(searchTerm);

        ViewBag.ReturnUrl = returnUrl;

        return View(model);
    }


    [HttpGet]
    public async Task<IActionResult> Create()
    {
        //var model = await _skillService.GetCreateVMAsync();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SkillCreateVM model)    
    {
        if (!ModelState.IsValid)
            return View(model);

        bool created = await _skillService.CreateAsync(model);

        if (!created)
        {
            ModelState.AddModelError("", "A skill with this name already exists.");
            return View(model);
        }

        TempData["SuccessMessage"] = "Skill created successfully.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, string? returnUrl)
    {
        var model = await _skillService.GetForEditAsync(id);

        ViewBag.ReturnUrl = returnUrl;

        if (model == null)
            return NotFound();

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(SkillEditVM model, string? returnUrl)
    {
        if (!ModelState.IsValid)
            return View(model);

        bool updated = await _skillService.UpdateAsync(model);

        /*if (!updated)
            return NotFound();*/
            
        if (!updated)
        {
            ModelState.AddModelError("", "A skill with this name already exists.");
            return View(model);
        }

        TempData["SuccessMessage"] = "Skill updated successfully.";

        if (!string.IsNullOrEmpty(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, string? returnUrl)
    {
        bool deleted = await _skillService.DeleteSkillAsync(id);

        if (!deleted)
        {
            TempData["ErrorMessage"] = "This skill cannot be deleted because it is assigned to one or more volunteers.";
        }
        else
        {
            TempData["SuccessMessage"] = "Skill deleted successfully.";
        }

        if (!string.IsNullOrEmpty(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction(nameof(Index));
    }
}