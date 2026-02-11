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
    public class LotsController : Controller
    {
        private readonly AppDbContext _context;

        public LotsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Lots
        public async Task<IActionResult> Index()
        {
            return View(await _context.Lot.ToListAsync());
        }

        // GET: Lots/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id.ToString() == null)
            {
                return NotFound();
            }

            var lot = await _context.Lot
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lot == null)
            {
                return NotFound();
            }

            return View(lot);
        }

        // GET: Lots/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lots/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LotNo,LotName")] Lot lot)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lot);
        }

        // GET: Lots/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var lot = await _context.Lot.FindAsync(id);
            if (lot == null)
            {
                return NotFound();
            }
            return View(lot);
        }

        // POST: Lots/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LotNo,LotName")] Lot lot)
        {
            if (id != lot.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LotExists(lot.Id))
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
            return View(lot);
        }

        // GET: Lots/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id.ToString() == null)
            {
                return NotFound();
            }

            var lot = await _context.Lot
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lot == null)
            {
                return NotFound();
            }

            return View(lot);
        }

        // POST: Lots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lot = await _context.Lot.FindAsync(id);
            _context.Lot.Remove(lot);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LotExists(int id)
        {
            return _context.Lot.Any(e => e.Id == id);
        }
    }
}
