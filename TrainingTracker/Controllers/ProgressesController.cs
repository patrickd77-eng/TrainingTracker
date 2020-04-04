/*=============================================================================
 |   Author and Copyright: Patrick Davis, s4901703
 |
 |   Designed in: 2019-2020 for Screwfix Poole Parkstone
 |
 |   As part of: Bournemouth University, Business Information Technology Final Year Project 
 |
 |   This code: Contains all CRUD functions for interacting with the progress record model.
 |   Scaffolded from .NET CORE MVC, with many custom changes.
 |              
 *===========================================================================*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Index(int? id, string employeeName, string searchString)
        {

            ViewData["CurrentFilter"] = searchString;
            ViewData["EmployeeName"] = employeeName;
            ViewData["EmployeeId"] = id;

            if (id == null)
            {
                //Prevent viewing of all employee training records. Unwanted feature and too messy. One employee at a time!
                return Forbid();
            }
            else if (_context.Employees.Where(e => e.EmployeeId == id).Any() == false)
            {
                //The ID given is not a valid employee ID. 404.
                return Forbid();
            }
            else
            {

                //All progress records for employee where given ID matches.
                if (!String.IsNullOrEmpty(searchString))
                {
                    var applicationDbContext = _context.Progresses
                        .Include(p => p.Employee)
                        .Include(p => p.Training)
                        .Where(
                        p => p.EmployeeId == id && p.Training.ModuleName.Contains(searchString)
                        || p.EmployeeId == id && p.Training.CategoryName.Contains(searchString));

                    return View(await applicationDbContext.ToListAsync());

                }
                else
                {
                    var applicationDbContext = _context.Progresses
                        .Include(p => p.Employee)
                        .Include(p => p.Training)
                        .Where(m => m.EmployeeId == id);
                    return View(await applicationDbContext.OrderBy(p => p.Training.CategoryName).ToListAsync());
                }
            }
        }

        //For use with progress update buttons in progress index view. Updates progress records for an employee depending on what button pressed, "update type".
        public async Task<IActionResult> CategoryUpdate(int id, string employeeName, string updateTarget, string updateType)
        {

            //All progress records for this employee
            var applicationDbContext = _context.Progresses
                   .Include(p => p.Employee)
                   .Include(p => p.Training)
                   .Where(m => m.EmployeeId == id)
                   //Which category?
                   .Where(m => m.Training.CategoryName.Contains(updateTarget));
            try
            {
                foreach (var item in applicationDbContext)
                {
                    // Make changes on entity
                    if (updateType == "Complete")
                    {
                        item.Completed = true;
                    }
                    else if (updateType == "Incomplete")
                    {
                        item.Completed = false;
                    }
                    _context.Update(item);
                }
                // Save changes in database
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { id, employeeName });
            }
            catch (DbUpdateException ex)
            {
                //Log error
                ModelState.AddModelError(ex.ToString(), "Unable to save changes. " +
                  "Try again, and if the problem persists " +
                  "see your system administrator.");
                return BadRequest();
            }
        }

        //As per progress update buttons, completes all of an employee's records.
        public async Task<IActionResult> MarkAllAsComplete(int id, string employeeName)
        {
            //All progress records for an employee.
            var applicationDbContext = _context.Progresses
                   .Include(p => p.Employee)
                   .Include(p => p.Training)
                   .Where(m => m.EmployeeId == id);
            try
            {
                foreach (var item in applicationDbContext)
                {
                    // Make changes on entity
                    item.Completed = true;
                    _context.Update(item);
                }
                // Save changes in database
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { id, employeeName });
            }
            catch (DbUpdateException ex)
            {
                //Log error
                ModelState.AddModelError(ex.ToString(), "Unable to save changes. " +
                  "Try again, and if the problem persists " +
                  "see your system administrator.");
                return BadRequest();
            }
        }

        //As per progress update buttons, marks all of an employee's records as incomplete.
        public async Task<IActionResult> MarkAllAsIncomplete(int id, string employeeName)
        {
            var applicationDbContext = _context.Progresses
                   .Include(p => p.Employee)
                   .Include(p => p.Training)
                   .Where(m => m.EmployeeId == id);
            try
            {
                foreach (var item in applicationDbContext)
                {
                    // Make changes on entity
                    item.Completed = false;
                    _context.Update(item);
                }
                // Save changes in database
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", new { id, employeeName });
            }
            catch (DbUpdateException ex)
            {
                //Log error
                ModelState.AddModelError(ex.ToString(), "Unable to save changes. " +
                  "Try again, and if the problem persists " +
                  "see your system administrator.");
                return BadRequest();
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
        public async Task<IActionResult> Edit(int id, [Bind("ProgressId,EmployeeId,TrainingId,Completed")] Progress progress, string employeeFirstName, string employeeLastName)
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
                return RedirectToAction("Index", new { id = progress.EmployeeId, employeeName = employeeFirstName + " " + employeeLastName });
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
