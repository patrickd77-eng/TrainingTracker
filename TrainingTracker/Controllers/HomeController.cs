using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TrainingTracker.Data;
using TrainingTracker.Models;

namespace TrainingTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //Get current month, initialise empty variable
            DateTime today = DateTime.Today;
            int currentMonth = today.Month;

            //Generate empty quarter var
            var quarter = "";


            if (currentMonth.Equals(2) || currentMonth.Equals(3) || currentMonth.Equals(4))
            {
                quarter = "Q1";
            }
            else if (currentMonth.Equals(5) || currentMonth.Equals(6) || currentMonth.Equals(7))
            {
                quarter = "Q2";
            }
            else if (currentMonth.Equals(8) || currentMonth.Equals(9) || currentMonth.Equals(10))
            {
                quarter = "Q3";
            }
            else if (currentMonth.Equals(11) || currentMonth.Equals(12) || currentMonth.Equals(1))
            {
                quarter = "Q4";
            }

            var targetCategory = "SSOW " + quarter + " Refresh";


            var quarterlyProgressContext = await _context.Progresses
              .Include(e => e.Employee)
              .Include(e => e.Training)
              .Where(e => e.Training.CategoryName.Contains(quarter)
              &&
              e.Completed == false)
              .ToListAsync();

            //Pass data to view - first up is category that is due.
            ViewData["dueCategory"] = targetCategory;
            ViewData["dueQuarter"] = quarter;

            //Count number of employees with incomplete quarterly records

            ViewData["incompleteQuarterlyRecords"] = quarterlyProgressContext.GroupBy(e => e.Employee).Distinct().Count();


            //Reminders
            int dueReminderCount = 0;
            var applicationDbContext = await _context.Reminders.ToListAsync();

            try
            {
                foreach (var item in applicationDbContext)
                {
                    if (item.DateDue <= today)
                    {
                        dueReminderCount++;
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                //Log error
                ModelState.AddModelError(ex.ToString(), "Unable to reach database and retrieve reminders. Contact your system admin.");
                return View();
            }

            //pass reminder count to view
            ViewData["dueReminderCount"] = dueReminderCount;


            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
