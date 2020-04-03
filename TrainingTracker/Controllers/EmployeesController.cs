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
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employees. Accept search and sorting order variables.
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

            var employees = from e in _context.Employees
                            select e;

            if (!String.IsNullOrEmpty(searchString))
            {


                if (searchString.Contains(" "))
                {
                    var splitString = searchString.Split(" ");

                    employees = employees.Where(e =>
                   e.LastName.Contains(splitString[1])
                   ||
                   e.FirstName.Contains(splitString[0]));

                }
                else
                {
                    employees = employees.Where(e =>
                   e.LastName.Contains(searchString)
                   ||
                   e.FirstName.Contains(searchString));
                }
            }
            switch (sortOrder)
            {
                case "name_desc":
                    employees = employees.OrderByDescending(e => e.LastName);
                    break;
                default:
                    employees = employees.OrderBy(e => e.LastName);
                    break;
            }
            int pageSize = 9;

            return View(await PaginatedList<Employee>.CreateAsync(employees.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,FirstName,LastName")] Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(employee);
                    await _context.SaveChangesAsync();

                    //Make blank training progress records for new employee. Pass employee object so employeeid may be accessed.
                    await AddProgressRecordsAsync(employee);
                    //Make a blank note so that the employee's notes section works correctly.
                    await AddBlankNote(employee);

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException ex)
            {
                //Log error
                ModelState.AddModelError(ex.ToString(), "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(employee);
        }

        public async Task<IActionResult> AddBlankNote(Employee employee)
        {
            try
            {
                var newRecord = new Note() { NoteContent = "Note Example - try writing one yourself!", EmployeeId = employee.EmployeeId };
                _context.Add(newRecord);
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
        public async Task<IActionResult> AddProgressRecordsAsync(Employee employee)
        {
            try
            {
                //All training modules.
                var trainingModules = _context.Trainings;

                //For each training module
                foreach (var item in trainingModules)
                {
                    //create new progress record with necessary values
                    var newRecord = new Progress() { Completed = false, EmployeeId = employee.EmployeeId, TrainingId = item.TrainingId };
                    //Add
                    _context.Add(newRecord);

                }
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


        // GET: Employees/Edit/ID
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/ID
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var employeeToUpdate = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == id);
            if (await TryUpdateModelAsync<Employee>(
                employeeToUpdate,
                "",
                e => e.FirstName, e => e.LastName))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    //Log error
                    ModelState.AddModelError(ex.ToString(), "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(employeeToUpdate);
        }



        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(employee);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }
    }
}
