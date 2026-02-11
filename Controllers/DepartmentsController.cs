using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LOTS3.Models;
using LOTS3.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace LOTS3.Controllers
{
    //[Authorize("PARCS_LOTS")]
    //[Authorize]
    //[Authorize(Roles = "Administrator, Parking Services, Super Administrator")]
    [Authorize(Roles = "Administrator, ISD Parking Services")]
    public class DepartmentsController : Controller
    {
        private readonly AppDbContext _context;
        
        public DepartmentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            return View(await _context.Department.ToListAsync());
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id.ToString() == null)
            {
                return NotFound();
            }

            var department = await _context.Department
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (department == null)
            {
                return NotFound();
            }

            //var singleDepartmentViewModel = new SingleDepartmentViewModel();
            //singleDepartmentViewModel.Id = id;
            //singleDepartmentViewModel.DepartmentNo = department.DepartmentNo;
            //singleDepartmentViewModel.FundOrg = department.FundOrg;
            //singleDepartmentViewModel.DepartmentAbrv = department.DepartmentAbrv;
            //singleDepartmentViewModel.DepartmentName = department.DepartmentName;

            var Results = from coordinator in _context.Coordinator
                          where coordinator.DepartmentId == id
                          select new Coordinator()
                          {
                              Id = coordinator.Id,
                              EmployeeNo = coordinator.EmployeeNo,
                              FirstName = coordinator.FirstName,
                              LastName = coordinator.LastName,
                              Division = coordinator.Division,
                              Email = coordinator.Email,
                              PhoneNumber = coordinator.PhoneNumber,
                              DepartmentId = id
                          };

            var departmentCoordinators = new List<Coordinator>();


            foreach (var coordinator in Results)
            {
                departmentCoordinators.Add(new Coordinator() 
                {   
                    EmployeeNo = coordinator.EmployeeNo, 
                    FirstName = coordinator.FirstName, 
                    LastName = coordinator.LastName, 
                    Division = coordinator.Division, 
                    Email = coordinator.Email, 
                    PhoneNumber = coordinator.PhoneNumber 
                });
            }

            department.CoordinatorList = departmentCoordinators;

            return View(department);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartmentNo,DepartmentAbrv,FundOrg,DepartmentName")] Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id.ToString() == null)
            {
                return NotFound();
            }

            var department = await _context.Department.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, DepartmentNo,DepartmentAbrv,FundOrg,DepartmentName")] Department department)
        {
            if (id != department.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.Id))
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
            return View(department);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id.ToString() == null)
            {
                return NotFound();
            }

            var department = await _context.Department
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Department.FindAsync(id);
            _context.Department.Remove(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
            return _context.Department.Any(e => e.Id == id);
        }
    }
}
