﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
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
            //await getReminderCountWhereDateIsDue();

            int dueReminderCount = 0;
            var applicationDbContext = await _context.Reminders.ToListAsync();
            DateTime today = DateTime.Today;

            try
            {
                foreach (var item in applicationDbContext)
                {
                    if (item.DateDue < today)
                    {
                        dueReminderCount++;
                    }
                }
                //return Ok();
            }
            catch (DbUpdateException ex)
            {
                //Log error
                ModelState.AddModelError(ex.ToString(), "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
                return BadRequest();
            }

            ViewData["dueReminderCount"] = dueReminderCount;
            return View();
        }
        //public async Task<IActionResult> getReminderCountWhereDateIsDue()
        //{

        //}



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
