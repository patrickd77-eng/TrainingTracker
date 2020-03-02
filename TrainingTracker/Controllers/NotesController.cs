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
    public class NotesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Notes
        public async Task<IActionResult> Index(int? id, string employeeName)
        {
            if (id == null)
            {
                //Note doesn't exist.
                return Forbid();
            }
            else if (_context.Employees.Where(e => e.EmployeeId == id).Any() == false)
            {
                //The ID given is not a valid employee ID. 404.
                return NotFound();
            }
            ViewData["EmployeeName"] = employeeName;
            ViewData["EmployeeId"] = id;

            var applicationDbContext = _context.Note
                .Include(n => n.Employee)
                .Where(n => n.EmployeeId == id);

            return View(await applicationDbContext.OrderBy(p => p.NoteId).ToListAsync());


        }

        // GET: Notes/Create
        public IActionResult Create(int employeeId, string employeeName)
        {
            ViewData["EmployeeName"] = employeeName;
            ViewData["EmployeeId"] = employeeId;

            //ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FirstName");
            return View();
        }

        // POST: Notes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NoteId,EmployeeId,NoteContent")] Note note, int employeeId, string employeeName)
        {
            if (ModelState.IsValid)
            {
                _context.Add(note);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { id = note.EmployeeId, employeeName });

            }
            //ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FirstName", note.EmployeeId);
            ViewData["EmployeeId"] = employeeId;
            ViewData["employeeName"] = employeeName;
            return View(note);
        }

        // GET: Notes/Edit/5
        public async Task<IActionResult> Edit(int? id, string employeeName)
        {
            ViewData["EmployeeName"] = employeeName;

            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Note.FindAsync(id);

            if (note == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FirstName", note.EmployeeId);
            return View(note);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NoteId,EmployeeId,NoteContent")] Note note, string employeeName)
        {
            if (id != note.NoteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(note);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(note.NoteId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewData["EmployeeName"] = employeeName;
                return RedirectToAction("Index", new { id = note.EmployeeId, employeeName });

            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FirstName", note.EmployeeId);
            return View(note);
        }

        // GET: Notes/Delete/5
        public async Task<IActionResult> Delete(int? id, string employeeName)
        {
            ViewData["EmployeeName"] = employeeName;

            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Note
                .Include(n => n.Employee)
                .FirstOrDefaultAsync(m => m.NoteId == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string employeeName)
        {
            var note = await _context.Note.FindAsync(id);
            _context.Note.Remove(note);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = note.EmployeeId, employeeName });

        }

        private bool NoteExists(int id)
        {
            return _context.Note.Any(e => e.NoteId == id);
        }
    }
}
