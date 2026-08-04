using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VolunteerMS.Data;
using VolunteerMS.Models;

namespace VolunteerMS.Controllers;

public class OpportunitiesController : Controller
{
    private readonly AppDbContext _context;

    public OpportunitiesController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var opportunities = await _context.Opportunities
            .Include(o => o.Center)
            .ToListAsync();

        return View(opportunities);
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