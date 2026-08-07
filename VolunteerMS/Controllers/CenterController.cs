using Microsoft.AspNetCore.Mvc;
using VolunteerMS.Models.ViewModels.Center;
using VolunteerMS.Services.Interfaces;

namespace VolunteerMS.Controllers;
public class CenterController : BaseController
{
    private readonly ICenterService _centerService;

    public CenterController(ICenterService centerService)
    {
        _centerService = centerService;
    }

    public async Task<IActionResult> Index(string? searchTerm, string? returnUrl)
    {
        var model = await _centerService.GetIndexVMAsync(searchTerm);

        ViewBag.ReturnUrl = returnUrl;

        return View(model);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CenterCreateVM model)
    {
        Console.WriteLine("STEP Create 1");

        if (!ModelState.IsValid)
            return View(model);

        bool created = await _centerService.CreateAsync(model);

        Console.WriteLine("STEP Create 2");

        if (!created)
        {
            ModelState.AddModelError("", "A center with this name already exists.");
            return View(model);
        }

        SetSuccessMessage("Center created successfully.");

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, string? returnUrl)
    {
        var model = await _centerService.GetEditVMAsync(id);

        ViewBag.ReturnUrl = returnUrl;

        if (model == null)
            return NotFound();

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CenterEditVM model, string? returnUrl)
    {
        if (!ModelState.IsValid)
            return View(model);

        Console.WriteLine("STEP 1");

        bool updated = await _centerService.UpdateAsync(model);

        Console.WriteLine("STEP 2");

        if (!updated)
        {
            ModelState.AddModelError("", "A center with this name already exists.");
            return View(model);
        }

        SetSuccessMessage("Center updated successfully.");

         if (!string.IsNullOrEmpty(returnUrl))
            return Redirect(returnUrl);
        
        Console.WriteLine("STEP 3");

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id, string? returnUrl)
    {
        var model = await _centerService.GetDetailsVMAsync(id);

        ViewBag.ReturnUrl = returnUrl;

        if (model == null)
            return NotFound();

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, string? returnUrl)
    {
        bool deleted = await _centerService.DeleteCenterAsync(id);

        if (!deleted)
        {
            TempData["ErrorMessage"] = "This center cannot be deleted because it has volunteers or opportunities assigned.";
        }
        else
        {
            TempData["SuccessMessage"] = "Center deleted successfully.";
        }

        if (!string.IsNullOrEmpty(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction(nameof(Index));
    }
}