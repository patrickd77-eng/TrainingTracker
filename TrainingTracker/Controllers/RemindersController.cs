/*=============================================================================
 |   Author and Copyright: Patrick Davis, s4901703
 |
 |   Designed in: 2019-2020 for Screwfix Poole Parkstone
 |
 |   As part of: Bournemouth University, Business Information Technology Final Year Project 
 |
 |   This code: Contains all CRUD functions for interacting with the Reminders model.
 |   Scaffolded from .NET CORE MVC.
 |              
 *===========================================================================*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TrainingTracker.Data;
using TrainingTracker.Models;

namespace TrainingTracker.Controllers
{
    public class RemindersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RemindersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reminders
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reminder.OrderBy(r => r.DateDue).ToListAsync());
        }


        // GET: Reminders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reminders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReminderId,ReminderContent,DateDue")] Reminder reminder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reminder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reminder);
        }

        // GET: Reminders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reminder = await _context.Reminder.FindAsync(id);
            if (reminder == null)
            {
                return NotFound();
            }
            return View(reminder);
        }

        // POST: Reminders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReminderId,ReminderContent,DateDue")] Reminder reminder)
        {
            if (id != reminder.ReminderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reminder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReminderExists(reminder.ReminderId))
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
            return View(reminder);
        }

        // GET: Reminders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reminder = await _context.Reminder
                .FirstOrDefaultAsync(m => m.ReminderId == id);
            if (reminder == null)
            {
                return NotFound();
            }

            return View(reminder);
        }

        // POST: Reminders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reminder = await _context.Reminder.FindAsync(id);
            _context.Reminder.Remove(reminder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReminderExists(int id)
        {
            return _context.Reminder.Any(e => e.ReminderId == id);
        }
    }
}
