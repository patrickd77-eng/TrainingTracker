using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingTracker.Data;
using TrainingTracker.Models;

namespace TrainingTracker.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        public EmployeesController(ApplicationDbContext db)
        {
            _db = db;
        }

        private readonly ApplicationDbContext _db;

        [BindProperty]
        public Employee Employee { get; set; }

        //View for employee list.
        public IActionResult Index()
        {
            return View();
        }

        
        //ID is nullable here (not required). Return an employee's info or a not found error based on ID presence.
        public IActionResult Update(int? id)
        {
            Employee = new Employee();
            if (id == null)
            {       
                return View(Employee);
            }
          
            Employee = _db.Employees.FirstOrDefault(u => u.Id == id);
            if (Employee == null)
            {
                return NotFound();
            }
            return View(Employee);
        }

        #region Database/API Calls
        //POST a new employee (If no ID given, this is a create request. If ID present, it's update.)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update()
        {
            if (ModelState.IsValid)
            {
                if (Employee.Id == 0)
                {           
                    _db.Employees.Add(Employee);
                }
                else
                {            
                    _db.Employees.Update(Employee);
                }
                _db.SaveChanges();
                //Redirect back to list view.
                return RedirectToAction("Index");
            }
            return View(Employee);
        }

        //GET a list of all Employees.
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Employees.ToListAsync() });
        }

        //Send a DELETE request for this particular Employee. The ID of said employee is passed from Ajax to this method.
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var employeeFromDb = await _db.Employees.FirstOrDefaultAsync(u => u.Id == id);
            if (employeeFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting the employee." });
            }
            _db.Employees.Remove(employeeFromDb);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Delete successful." });
        }
        #endregion
    }
}