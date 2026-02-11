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
    public class PermitTypesController : Controller
    {
        private readonly AppDbContext _context;

        public PermitTypesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PermitTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.PermitType.ToListAsync());
        }

        // GET: PermitTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permitType = await _context.PermitType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (permitType == null)
            {
                return NotFound();
            }

            return View(permitType);
        }

        // GET: PermitTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PermitTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PermitTypeNo,PermitTypeName")] PermitType permitType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(permitType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(permitType);
        }

        // GET: PermitTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permitType = await _context.PermitType.FindAsync(id);
            if (permitType == null)
            {
                return NotFound();
            }
            return View(permitType);
        }

        // POST: PermitTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PermitTypeNo,PermitTypeName")] PermitType permitType)
        {
            if (id != permitType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(permitType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PermitTypeExists(permitType.Id))
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
            return View(permitType);
        }

        // GET: PermitTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permitType = await _context.PermitType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (permitType == null)
            {
                return NotFound();
            }

            return View(permitType);
        }

        // POST: PermitTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var permitType = await _context.PermitType.FindAsync(id);
            _context.PermitType.Remove(permitType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PermitTypeExists(int id)
        {
            return _context.PermitType.Any(e => e.Id == id);
        }
    }
}
