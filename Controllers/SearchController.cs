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
    public class SearchController : Controller
    {
        private readonly AppDbContext _context;

        public SearchController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Search
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Permit
                                    .Include(p => p.Department)
                                    .Include(p => p.Lot).Include(p => p.PayType)
                                    .Include(p => p.PermitType)
                                    .Include(p => p.Permittee)
                                    .Include(p => p.PermitteeType)
                                    .Include(p => p.StatusType);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Search/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permit = await _context.Permit
                .Include(p => p.Department)
                .Include(p => p.Lot)
                .Include(p => p.PayType)
                .Include(p => p.PermitType)
                .Include(p => p.Permittee)
                .Include(p => p.PermitteeType)
                .Include(p => p.StatusType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (permit == null)
            {
                return NotFound();
            }

            return View(permit);
        }

        // GET: Search/Create
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Id");
            ViewData["LotId"] = new SelectList(_context.Lot, "Id", "Id");
            ViewData["PayTypeId"] = new SelectList(_context.PayType, "Id", "Id");
            ViewData["PermitTypeId"] = new SelectList(_context.PermitType, "Id", "Id");
            ViewData["PermitteeId"] = new SelectList(_context.Permittee, "Id", "Id");
            ViewData["PermitteeTypeId"] = new SelectList(_context.PermitteeType, "Id", "Id");
            ViewData["StatusTypeId"] = new SelectList(_context.StatusType, "Id", "Id");
            return View();
        }

        // POST: Search/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PermitteeId,LotId,PermitNo,KeycardNo,PermitTypeId,PermitteeTypeId,PayTypeId,DepartmentId,StartDate,EndDate,UpdatedDate,Comments,StatusTypeId")] Permit permit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(permit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Id", permit.DepartmentId);
            ViewData["LotId"] = new SelectList(_context.Lot, "Id", "Id", permit.LotId);
            ViewData["PayTypeId"] = new SelectList(_context.PayType, "Id", "Id", permit.PayTypeId);
            ViewData["PermitTypeId"] = new SelectList(_context.PermitType, "Id", "Id", permit.PermitTypeId);
            ViewData["PermitteeId"] = new SelectList(_context.Permittee, "Id", "Id", permit.PermitteeId);
            ViewData["PermitteeTypeId"] = new SelectList(_context.PermitteeType, "Id", "Id", permit.PermitteeTypeId);
            ViewData["StatusTypeId"] = new SelectList(_context.StatusType, "Id", "Id", permit.StatusTypeId);
            return View(permit);
        }

        // GET: Search/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permit = await _context.Permit.FindAsync(id);
            if (permit == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Id", permit.DepartmentId);
            ViewData["LotId"] = new SelectList(_context.Lot, "Id", "Id", permit.LotId);
            ViewData["PayTypeId"] = new SelectList(_context.PayType, "Id", "Id", permit.PayTypeId);
            ViewData["PermitTypeId"] = new SelectList(_context.PermitType, "Id", "Id", permit.PermitTypeId);
            ViewData["PermitteeId"] = new SelectList(_context.Permittee, "Id", "Id", permit.PermitteeId);
            ViewData["PermitteeTypeId"] = new SelectList(_context.PermitteeType, "Id", "Id", permit.PermitteeTypeId);
            ViewData["StatusTypeId"] = new SelectList(_context.StatusType, "Id", "Id", permit.StatusTypeId);
            return View(permit);
        }

        // POST: Search/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PermitteeId,LotId,PermitNo,KeycardNo,PermitTypeId,PermitteeTypeId,PayTypeId,DepartmentId,StartDate,EndDate,UpdatedDate,Comments,StatusTypeId")] Permit permit)
        {
            if (id != permit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(permit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PermitExists(permit.Id))
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
            ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Id", permit.DepartmentId);
            ViewData["LotId"] = new SelectList(_context.Lot, "Id", "Id", permit.LotId);
            ViewData["PayTypeId"] = new SelectList(_context.PayType, "Id", "Id", permit.PayTypeId);
            ViewData["PermitTypeId"] = new SelectList(_context.PermitType, "Id", "Id", permit.PermitTypeId);
            ViewData["PermitteeId"] = new SelectList(_context.Permittee, "Id", "Id", permit.PermitteeId);
            ViewData["PermitteeTypeId"] = new SelectList(_context.PermitteeType, "Id", "Id", permit.PermitteeTypeId);
            ViewData["StatusTypeId"] = new SelectList(_context.StatusType, "Id", "Id", permit.StatusTypeId);
            return View(permit);
        }

        // GET: Search/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permit = await _context.Permit
                .Include(p => p.Department)
                .Include(p => p.Lot)
                .Include(p => p.PayType)
                .Include(p => p.PermitType)
                .Include(p => p.Permittee)
                .Include(p => p.PermitteeType)
                .Include(p => p.StatusType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (permit == null)
            {
                return NotFound();
            }

            return View(permit);
        }

        // POST: Search/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var permit = await _context.Permit.FindAsync(id);
            _context.Permit.Remove(permit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PermitExists(int id)
        {
            return _context.Permit.Any(e => e.Id == id);
        }
    }
}
