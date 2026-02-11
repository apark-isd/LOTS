using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LOTS3.Models;
using Microsoft.AspNetCore.Authorization;

namespace LOTS3.Controllers
{
    //[Authorize("PARCS_LOTS")]
    //[Authorize]
    //[Authorize(Roles = "Administrator, Parking Services, Super Administrator")]
    [Authorize(Roles = "Administrator, ISD Parking Services")]
    public class PayTypesController : Controller
    {
        private readonly AppDbContext _context;
        
        public PayTypesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PayTypes
        public async Task<IActionResult> Index()
        {
            //return View(await _context.PayType.ToListAsync());
            var payType = _context.PayType
                            //.Include(p => p.Department)
                            //    .ThenInclude(p => p.Department)
                            //.Include(p => p.Permittee)
                            //    .ThenInclude(p => p.StatusType)
                            //.Include(p => p.Lot)
                            //.Include(p => p.PermitteeType)
                            //.Include(p => p.PermitType)
                            //.Include(p => p.PayType)
                            .ToListAsync();

            return View(await payType);
        }

        // GET: PayTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payType = await _context.PayType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payType == null)
            {
                return NotFound();
            }

            return View(payType);
        }

        // GET: PayTypes/Create
        public IActionResult Create()
        {
            //ViewBag.Departments = new SelectList(_context.Department, "Id", "DepartmentName");
            ViewBag.Departments = _context.Department;
            return View();
        }

        // POST: PayTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,PayTypeName,PayTypeAmount")] PayType payType)
        public async Task<IActionResult> Create(PayType payType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(payType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(payType);
        }

        // GET: PayTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payType = await _context.PayType.FindAsync(id);
            if (payType == null)
            {
                return NotFound();
            }
            return View(payType);
        }

        // POST: PayTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PayTypeName,PayTypeAmount")] PayType payType)
        {
            if (id != payType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PayTypeExists(payType.Id))
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
            return View(payType);
        }

        // GET: PayTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payType = await _context.PayType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payType == null)
            {
                return NotFound();
            }

            return View(payType);
        }

        // POST: PayTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payType = await _context.PayType.FindAsync(id);
            _context.PayType.Remove(payType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PayTypeExists(int id)
        {
            return _context.PayType.Any(e => e.Id == id);
        }
    }
}
