using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainingTracker.Data;

namespace TrainingTracker.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        private readonly ApplicationDbContext _context;

        public IActionResult Index()
        {

            return View();
        }


        public async Task<IActionResult> ModuleProgress()
        {
            //Get training modules including their category.
            var trainingContent = await _context.Trainings
                .Include(p => p.Progresses)
              .ToListAsync();

            List<string> trainingList = new List<string>();

            foreach (var item in trainingContent)
            {
                trainingList.Add(
                    item.CategoryName + ": " +
                        item.ModuleName + ": " + item.Progresses.Where(p => p.Completed == true).Count()
                        );
            }
            ViewBag.TrainingList = trainingList;
            ViewData["TrainingCount"] = trainingContent.Count();
            return View();
        }

        public async Task<IActionResult> CategoryProgress()
        {
            //Get completion count per category
            var categoriesAndCompletion = await _context.Progresses
                .Include(p => p.Training)
                .Where(p => p.Completed == true)
                .ToListAsync();


            ViewData["ssowNew"] = categoriesAndCompletion.Where(p => p.Training.CategoryName.Contains("New")).Count();
            ViewData["ssowRQ1"] = categoriesAndCompletion.Where(p => p.Training.CategoryName.Contains("Q1")).Count();
            ViewData["ssowRQ2"] = categoriesAndCompletion.Where(p => p.Training.CategoryName.Contains("Q2")).Count();
            ViewData["ssowRQ3"] = categoriesAndCompletion.Where(p => p.Training.CategoryName.Contains("Q3")).Count();
            ViewData["ssowRQ4"] = categoriesAndCompletion.Where(p => p.Training.CategoryName.Contains("Q4")).Count();
            ViewData["jollydeckE"] = categoriesAndCompletion.Where(p => p.Training.CategoryName.Contains("Essential")).Count();
            ViewData["jollydeckO"] = categoriesAndCompletion.Where(p => p.Training.CategoryName.Contains("Optional")).Count();

            return View();
        }
        public async Task<IActionResult> EmployeeProgress()
        {
            //Get Employee "Completed" Count.
            var progressCount = 0;
            var employeeProgress = await _context.Employees
             .Include(p => p.Progresses)
              .ToListAsync();


            var trainingCount = _context.Trainings.Count();


            List<string> progressList = new List<string>();

            foreach (var item in employeeProgress)
            {
                progressCount++;

                progressList.Add(
                     item.FirstName + " " + item.LastName + ": " +
                    item.Progresses.Where(p => p.Completed == true).Count() +
                    "/" + trainingCount
                    );
            }

            ViewBag.EmployeeList = progressList;

            return View();
        }
    }
}
