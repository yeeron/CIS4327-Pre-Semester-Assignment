using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VolunteerMS.Data;
using VolunteerMS.Models;
using VolunteerMS.Models.ViewModels.Opportunity;

namespace VolunteerMS.Controllers;

public class OpportunitiesController : Controller
{
    private readonly AppDbContext _context;

    public OpportunitiesController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(
        string? searchTerm,
        bool recentOnly = false,
        int? centerId = null)
    {
        IQueryable<Opportunity> query = _context.Opportunities
            .Include(o => o.Center);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.Trim().ToLower();

            query = query.Where(o =>
                o.Name.ToLower().Contains(searchTerm) ||
                o.Description.ToLower().Contains(searchTerm));
        }

        if (recentOnly)
        {
            var sixtyDaysAgo = DateTime.UtcNow.AddDays(-60);

            query = query.Where(o => o.CreatedDate >= sixtyDaysAgo);
        }

        if (centerId.HasValue)
        {
            query = query.Where(o => o.CenterId == centerId.Value);
        }

        var opportunities = await query
            .OrderBy(o => o.StartDate)
            .ToListAsync();

        var centers = await _context.Centers
            .OrderBy(c => c.Name)
            .ToListAsync();

        var model = new OpportunityIndexVM
        {
            SearchTerm = searchTerm,
            RecentOnly = recentOnly,
            CenterId = centerId,
            Opportunities = opportunities,
            Centers = new SelectList(centers, "Id", "Name", centerId)
        };

        return View(model);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Centers = new SelectList(
            await _context.Centers.ToListAsync(),
            "Id",
            "Name");

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Create(Opportunity opportunity)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Centers = new SelectList(
                await _context.Centers.ToListAsync(),
                "Id",
                "Name",
                opportunity.CenterId);

            return View(opportunity);
        }

        opportunity.CreatedDate = DateTime.UtcNow;

        _context.Opportunities.Add(opportunity);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var opportunity = await _context.Opportunities.FindAsync(id);

        if (opportunity == null)
        {
            return NotFound();
        }

        ViewBag.Centers = new SelectList(
            await _context.Centers.ToListAsync(),
            "Id",
            "Name",
            opportunity.CenterId);

        return View(opportunity);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Edit(int id, Opportunity opportunity)
    {
        if (id != opportunity.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Centers = new SelectList(
                await _context.Centers.ToListAsync(),
                "Id",
                "Name",
                opportunity.CenterId);

            return View(opportunity);
        }

        var existingOpportunity = await _context.Opportunities.FindAsync(id);

        if (existingOpportunity == null)
            {
                return NotFound();
            }

        existingOpportunity.Name = opportunity.Name;
        existingOpportunity.Description = opportunity.Description;
        existingOpportunity.StartDate = opportunity.StartDate;
        existingOpportunity.Location = opportunity.Location;
        existingOpportunity.VolunteersNeeded = opportunity.VolunteersNeeded;
        existingOpportunity.CenterId = opportunity.CenterId;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var opportunity = await _context.Opportunities
            .FirstOrDefaultAsync(m => m.Id == id);

        if (opportunity == null)
        {
            return NotFound();
        }

        return View(opportunity);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var opportunity = await _context.Opportunities.FindAsync(id);

        if (opportunity != null)
        {
            _context.Opportunities.Remove(opportunity);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}