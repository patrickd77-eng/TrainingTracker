using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrainingTracker.Data;
using TrainingTracker.Models;

namespace TrainingTracker.Controllers
{
    [Authorize]
    public class TrainingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Trainings
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;


            var trainingSections = from t in _context.Trainings
                                   select t;

            if (!String.IsNullOrEmpty(searchString))
            {
                trainingSections = trainingSections.
                    Where(t => t.ModuleName.Contains(searchString)
                         || t.ModuleName.Contains(searchString)
                         || t.CategoryName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    trainingSections = trainingSections.OrderBy(t => t.ModuleName);
                    break;
                default:
                    trainingSections = trainingSections.OrderBy(t => t.TrainingId);
                    break;
            }
            int pageSize = 9;
            return View(await PaginatedList<Training>.CreateAsync(trainingSections.AsNoTracking(), pageNumber ?? 1, pageSize));


            //return View(await trainingSections.ToListAsync());
        }


        // GET: Trainings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Trainings/Create   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrainingId,CategoryName,ModuleName")] Training training)
        {
            if (ModelState.IsValid)
            {
                _context.Add(training);
                await _context.SaveChangesAsync();
                //TODO: Add new blank progress record for all employees. 

                await AddProgressRecordsAsync(training);

                return RedirectToAction(nameof(Index));
            }
            return View(training);
        }

        public async Task<IActionResult> AddProgressRecordsAsync(Training training)
        {
            try
            {
                //All employees
                var employeesInDb = _context.Employees;

                //For each employee
                foreach (var item in employeesInDb)
                {
                    //create new progress record with necessary values
                    var newRecord = new Progress() { Completed = false, EmployeeId = item.EmployeeId, TrainingId = training.TrainingId };
                    //Add to db.
                    _context.Add(newRecord);

                }
                //save, return ok.
                await _context.SaveChangesAsync();
                return Ok();
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






        // GET: Trainings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training = await _context.Trainings.FindAsync(id);
            if (training == null)
            {
                return NotFound();
            }
            return View(training);
        }

        // POST: Trainings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TrainingId,CategoryName,ModuleName")] Training training)
        {
            if (id != training.TrainingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(training);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingExists(training.TrainingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(training);
        }

        // GET: Trainings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training = await _context.Trainings
                .FirstOrDefaultAsync(m => m.TrainingId == id);
            if (training == null)
            {
                return NotFound();
            }

            return View(training);
        }

        // POST: Trainings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var training = await _context.Trainings.FindAsync(id);
            _context.Trainings.Remove(training);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingExists(int id)
        {
            return _context.Trainings.Any(e => e.TrainingId == id);
        }
    }
}
