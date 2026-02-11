using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LOTS3.Models;
using System.Net;
using Newtonsoft.Json;

namespace LOTS3.Controllers
{
    public class CoordinatorsController : Controller
    {
        private readonly AppDbContext _context;

        public CoordinatorsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Coordinators
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Coordinator.Include(c => c.Department);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Coordinators/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coordinator = await _context.Coordinator
                .Include(c => c.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coordinator == null)
            {
                return NotFound();
            }
            
            ViewBag.Departments = _context.Department;
            
            return View(coordinator);
        }

        // GET: Coordinators/Create
        public IActionResult Create()
        {
            //ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Id");
            ViewBag.Departments = _context.Department;
            return View();
        }

        // POST: Coordinators/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, EmployeeNo,FirstName,LastName,PhoneNumber,Email,Division,DepartmentId")] Coordinator coordinator)
        {
            if (ModelState.IsValid)
            {
                _context.Add(coordinator);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Id", coordinator.DepartmentId);
            return View(coordinator);
        }

        // GET: Coordinators/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coordinator = await _context.Coordinator.FindAsync(id);
            if (coordinator == null)
            {
                return NotFound();
            }

            //ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Id", coordinator.DepartmentId);
            ViewBag.Departments = _context.Department;

            return View(coordinator);
        }

        // POST: Coordinators/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeNo,FirstName,LastName,PhoneNumber,Email,Division,DepartmentId")] Coordinator coordinator)
        {
            if (id != coordinator.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coordinator);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoordinatorExists(coordinator.Id))
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
            ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Id", coordinator.DepartmentId);
            return View(coordinator);
        }

        // GET: Coordinators/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coordinator = await _context.Coordinator
                .Include(c => c.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coordinator == null)
            {
                return NotFound();
            }

            ViewBag.Departments = _context.Department;

            return View(coordinator);
        }

        // POST: Coordinators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coordinator = await _context.Coordinator.FindAsync(id);
            _context.Coordinator.Remove(coordinator);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoordinatorExists(int id)
        {
            return _context.Coordinator.Any(e => e.Id == id);
        }

        //For Test Emp#/202147/445395/467348/521949/534072/661714
        [HttpGet]
        public IActionResult GetEmployeeInfo(string employeeId)
        {
            string url = $"https://fleetapi.isd.lacounty.gov/parking/ge?ei={employeeId}";
            var response = new WebClient().DownloadString(url);

            var definition = new
            {
                name = "",
                givenName = "",
                sn = "",
                telephoneNumber = "",
                email = "",
                department = "",
                division = ""
            };
            var employee = JsonConvert.DeserializeAnonymousType(response, definition);
            string dept = "";
            int departmentId = 0;

            if (employee.department.Contains("Affirmative"))
            {
                dept = "AA";
                departmentId = (from department in _context.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Alternate Public Defender"))
            {
                dept = "AD";
                departmentId = (from department in _context.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Animal"))
            {
                dept = "AN";
                departmentId = (from department in _context.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Chief Executive"))
            {
                dept = "AO";
                departmentId = (from department in _context.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Museum"))
            {
                dept = "AR";
                departmentId = (from department in _context.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Assessor"))
            {
                dept = "AS";
                departmentId = (from department in _context.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Auditor"))
            {
                dept = "AU";
                departmentId = (from department in _context.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Agricultural"))
            {
                dept = "AW";
                departmentId = (from department in _context.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Beaches"))
            {
                dept = "BH";
                departmentId = (from department in _context.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Board of Supervisors"))
            {
                dept = "BS";
                departmentId = (from department in _context.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Consumer"))
            {
                dept = "CA";
                departmentId = (from department in _context.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("County Counsel"))
            {
                dept = "CC";
                departmentId = (from department in _context.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Child Support"))
            {
                dept = "CD";
                departmentId = (from department in _context.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Children"))
            {
                dept = "CH";
                departmentId = (from department in _context.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Community"))
            {
                dept = "CS";
                departmentId = (from department in _context.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Attorney"))
            {
                dept = "DA";
                departmentId = (from department in _context.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Fire"))
            {
                dept = "FR";
                departmentId = (from department in _context.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Grand Jury"))
            {
                dept = "GJ";
                departmentId = (from department in _context.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("LAC/USC"))
            {
                dept = "HG";
                departmentId = (from department in _context.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Internal Services"))
            {
                dept = "IS";
                departmentId = (from department in _context.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Public Health"))
            {
                dept = "PH";
                departmentId = (from department in _context.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Arts"))
            {
                dept = "RT";
                departmentId = (from department in _context.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }

            return Json(new
            {
                id = employeeId,
                firstName = employee.givenName,
                lastName = employee.sn,
                employee.telephoneNumber,
                employee.email,
                department = dept,
                employee.division,
                departmentId = departmentId.ToString()
            });
        }
    }
}
