using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrainingTracker.Data;
using TrainingTracker.Models;

namespace TrainingTracker.Controllers
{
    [Authorize]
    public class ProgressesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProgressesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Progresses
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                var applicationDbContext = _context.Progresses.Include(p => p.Employee).Include(p => p.Training);

                return View(await applicationDbContext.ToListAsync());

            }
            else
            {
                var applicationDbContext = _context.Progresses
                    .Include(p => p.Employee)
                    .Include(p => p.Training)
                    .Where(m => m.EmployeeId == id);

                

                return View(await applicationDbContext.OrderByDescending(p => p.Completed).ToListAsync());

            }

        }

        // GET: Progresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var progress = await _context.Progresses.FindAsync(id);
            if (progress == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FirstName", progress.EmployeeId);
            ViewData["TrainingId"] = new SelectList(_context.Trainings, "TrainingId", "ModuleName", progress.TrainingId);
            return View(progress);
        }

        // POST: Progresses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProgressId,EmployeeId,TrainingId,Completed")] Progress progress)
        {
            if (id != progress.ProgressId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(progress);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProgressExists(progress.ProgressId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { id=progress.EmployeeId });
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FirstName", progress.EmployeeId);
            ViewData["TrainingId"] = new SelectList(_context.Trainings, "TrainingId", "ModuleName", progress.TrainingId);
            return View(progress);
        }

        private bool ProgressExists(int id)
        {
            return _context.Progresses.Any(e => e.ProgressId == id);
        }
    }
}
