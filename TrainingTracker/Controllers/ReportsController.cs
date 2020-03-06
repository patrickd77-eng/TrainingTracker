using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainingTracker.Data;

namespace TrainingTracker.Controllers
{

    public class ReportsController : Controller
    {

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        private readonly ApplicationDbContext _context;

        public async Task<IActionResult> Index()
        {

            //Where complete and category is xxx
            var ssowNewCompleted = await _context.Progresses
                .Where(p => p.Completed == true)
                .Where(p => p.Training.CategoryName.Contains("SSOW New"))
                .ToListAsync();
            var ssowRefreshQ1Completed = await _context.Progresses
               .Where(p => p.Completed == true)
               .Where(p => p.Training.CategoryName.Contains("Q1"))
               .ToListAsync();
            var ssowRefreshQ2Completed = await _context.Progresses
               .Where(p => p.Completed == true)
               .Where(p => p.Training.CategoryName.Contains("Q2"))
               .ToListAsync();
            var ssowRefreshQ3Completed = await _context.Progresses
               .Where(p => p.Completed == true)
               .Where(p => p.Training.CategoryName.Contains("Q3"))
               .ToListAsync();
            var ssowRefreshQ4Completed = await _context.Progresses
               .Where(p => p.Completed == true)
               .Where(p => p.Training.CategoryName.Contains("Q4"))
               .ToListAsync();
            var jollydeckEssentialCompleted = await _context.Progresses
               .Where(p => p.Completed == true)
               .Where(p => p.Training.CategoryName.Contains("Essential"))
               .ToListAsync();
            var jollydeckOptionalCompleted = await _context.Progresses
               .Where(p => p.Completed == true)
               .Where(p => p.Training.CategoryName.Contains("Optional"))
               .ToListAsync();

            //Get numbers for view
            ViewData["SSOWNEW"] = ssowNewCompleted.Count();
            ViewData["SSOWRQ1"] = ssowRefreshQ1Completed.Count();
            ViewData["SSOWRQ2"] = ssowRefreshQ2Completed.Count();
            ViewData["SSOWRQ3"] = ssowRefreshQ3Completed.Count();
            ViewData["SSOWRQ4"] = ssowRefreshQ4Completed.Count();
            ViewData["JollyDeckEssential"] = jollydeckEssentialCompleted.Count();
            ViewData["JollyDeckOptional"] = jollydeckOptionalCompleted.Count();

            //Total category stats
            var ssowNewCount = await _context.Progresses
              .Where(p => p.Training.CategoryName.Contains("SSOW New"))
              .ToListAsync();
            var ssowRefreshQ1Count = await _context.Progresses
               .Where(p => p.Training.CategoryName.Contains("Q1"))
               .ToListAsync();
            var ssowRefreshQ2Count = await _context.Progresses
               .Where(p => p.Training.CategoryName.Contains("Q2"))
               .ToListAsync();
            var ssowRefreshQ3Count = await _context.Progresses
               .Where(p => p.Training.CategoryName.Contains("Q3"))
               .ToListAsync();
            var ssowRefreshQ4Count = await _context.Progresses
               .Where(p => p.Training.CategoryName.Contains("Q4"))
               .ToListAsync();
            var jollydeckEssentialCount = await _context.Progresses
               .Where(p => p.Training.CategoryName.Contains("Essential"))
               .ToListAsync();
            var jollydeckOptionalCount = await _context.Progresses
               .Where(p => p.Training.CategoryName.Contains("Optional"))
               .ToListAsync();

            //Get numbers for view
            ViewData["SSOWNEWC"] = ssowNewCount.Count();
            ViewData["SSOWRQ1C"] = ssowRefreshQ1Count.Count();
            ViewData["SSOWRQ2C"] = ssowRefreshQ2Count.Count();
            ViewData["SSOWRQ3C"] = ssowRefreshQ3Count.Count();
            ViewData["SSOWRQ4C"] = ssowRefreshQ4Count.Count();
            ViewData["JollyDeckEssentialC"] = jollydeckEssentialCount.Count();
            ViewData["JollyDeckOptionalC"] = jollydeckOptionalCount.Count();

            //Get Employee Ranked Progress
            var progressCount = 0;
            var employeeProgress = await _context.Employees
             .Include(p => p.Progresses)
              .ToListAsync();

            employeeProgress.GroupBy(p => p.Equals(true));
            var trainingCount = _context.Trainings.Count();


            List<string> rankingList = new List<string>();

            foreach (var item in employeeProgress)
            {
                progressCount++;

                rankingList.Add(
                    item.FirstName + " " + item.LastName + " " +
                    item.Progresses.Where(p => p.Completed == true).Count() +
                    "/" + trainingCount
                    );
            }

            ViewBag.List = rankingList;

            //Get training count
            var trainingContent = await _context.Trainings
              .ToListAsync();

            List<string> trainingList = new List<string>();

            foreach (var item in trainingContent)
            {
                trainingList.Add(
                        item.ModuleName
                        );
            }

            var trainingCategories = await _context.Trainings
                .Distinct()
                .ToListAsync();

            List<string> trainingCategoriesList = new List<string>();

            foreach (var item in trainingCategories)
            {
                trainingCategoriesList.Add(
                    item.CategoryName);
            }


            ViewBag.TrainingList = trainingList;
            ViewData["TrainingCount"] = trainingContent.Count();
            ViewBag.TrainingCategoriesList = trainingCategoriesList;

            return View();
        }
    }
}
