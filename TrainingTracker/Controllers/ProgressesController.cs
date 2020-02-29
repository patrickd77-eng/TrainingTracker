using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Index(int? id, string employeeName)
        {
            if (id == null)
            {
                //Employee doesn't exist.
                return NotFound();
            }
            else if (_context.Progresses.Where(m => m.EmployeeId == id).Count() == 0)
            {
                //The ID in URL doesn't match progress records for an employee, I.E. it is not a valid employee ID. Prevent viewing.
                return Forbid();
            }
            else
            {
                //All progress records for employee where ID matches.
                var applicationDbContext = _context.Progresses
                    .Include(p => p.Employee)
                    .Include(p => p.Training)
                    .Where(m => m.EmployeeId == id);

                ViewData["EmployeeName"] = employeeName;
                ViewData["EmployeeId"] = id;

                return View(await applicationDbContext.OrderBy(p => p.Training.CategoryName).ToListAsync());

            }

        }


        public async Task<IActionResult> MarkAllAsComplete(int id, string employeeName)
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
                    item.Completed = true;
                    // Update entity in DbSet
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


        public async Task<IActionResult> MarkAllAsUncomplete(int id, string employeeName)
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
                    // Update entity in DbSet
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



        //public async Task<IActionResult> AddProgressRecordsAsync(Employee employee)
        //{
        //    try
        //    {
        //        //All training modules.
        //        var trainingModules = _context.Trainings;

        //        //For each training module
        //        foreach (var item in trainingModules)
        //        {
        //            //create new progress record with necessary values
        //            var newRecord = new Progress() { Completed = false, EmployeeId = employee.EmployeeId, TrainingId = item.TrainingId };
        //            //Add
        //            _context.Add(newRecord);

        //        }
        //        await _context.SaveChangesAsync();
        //        return Ok();
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        //Log error
        //        ModelState.AddModelError(ex.ToString(), "Unable to save changes. " +
        //            "Try again, and if the problem persists " +
        //            "see your system administrator.");
        //        return BadRequest();
        //    }
        //}

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
