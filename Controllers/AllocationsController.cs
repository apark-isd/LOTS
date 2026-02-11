using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LOTS3.Models;

namespace LOTS3.Controllers
{
    public class AllocationsController : Controller
    {
        private readonly AppDbContext _context;

        public AllocationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Allocations
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Allocation.Include(a => a.Department).Include(a => a.Lot);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Allocations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allocation = await _context.Allocation
                .Include(a => a.Department)
                .Include(a => a.Lot)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (allocation == null)
            {
                return NotFound();
            }

            ViewBag.Departments = allocation.Department;
            ViewBag.Lots = allocation.Lot;

            return View(allocation);
        }

        // GET: Allocations/Create
        public IActionResult Create()
        {
            //ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "DepartmentName");
            //ViewData["LotId"] = new SelectList(_context.Lot, "Id", "LotName");
            ViewBag.Departments = _context.Department;
            ViewBag.Lots = _context.Lot;
            return View();
        }

        // POST: Allocations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AllocationNo,IssuedNo,PlusMinusAllocation,DepartmentId,LotId")] Allocation allocation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(allocation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Id", allocation.DepartmentId);
            ViewData["LotId"] = new SelectList(_context.Lot, "Id", "Id", allocation.LotId);
            return View(allocation);
        }

        // GET: Allocations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allocation = await _context.Allocation.FindAsync(id);
            if (allocation == null)
            {
                return NotFound();
            }
            //ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Id", allocation.DepartmentId);
            //ViewData["LotId"] = new SelectList(_context.Lot, "Id", "Id", allocation.LotId);
            ViewBag.Departments = _context.Department;
            ViewBag.Lots = _context.Lot;
            return View(allocation);
        }

        // POST: Allocations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AllocationNo,IssuedNo,PlusMinusAllocation,DepartmentId,LotId")] Allocation allocation)
        {
            if (id != allocation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(allocation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AllocationExists(allocation.Id))
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
            ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Id", allocation.DepartmentId);
            ViewData["LotId"] = new SelectList(_context.Lot, "Id", "Id", allocation.LotId);
            return View(allocation);
        }

        // GET: Allocations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allocation = await _context.Allocation
                .Include(a => a.Department)
                .Include(a => a.Lot)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (allocation == null)
            {
                return NotFound();
            }

            return View(allocation);
        }

        // POST: Allocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var allocation = await _context.Allocation.FindAsync(id);
            _context.Allocation.Remove(allocation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AllocationExists(int id)
        {
            return _context.Allocation.Any(e => e.Id == id);
        }

        [HttpGet]
        public IActionResult GetIssuedCount(int departmentId, int lotId)
        {
            int IssuedCount = (from permit in _context.Permit
                               join permittee in _context.Permittee on
                               permit.PermitteeId equals permittee.Id
                               where permittee.DepartmentId == departmentId
                               && permit.LotId == lotId && (permit.EndDate <= DateTime.Today || permit.EndDate == null)
                               select permit).Count();

            return Json(new
            {
                issuedCount = IssuedCount
            }); 
        }
    }
}
