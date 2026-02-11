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
    public class CheckTimesController : Controller
    {
        private readonly AppDbContext _context;

        public CheckTimesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: CheckTimes
        public async Task<IActionResult> Index()
        {
            return View(await _context.CheckTime.ToListAsync());
        }

        // GET: CheckTimes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkTime = await _context.CheckTime
                .FirstOrDefaultAsync(m => m.Id == id);
            if (checkTime == null)
            {
                return NotFound();
            }

            return View(checkTime);
        }

        // GET: CheckTimes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CheckTimes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CheckTimeNo,CheckTimeName")] CheckTime checkTime)
        {
            if (ModelState.IsValid)
            {
                _context.Add(checkTime);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(checkTime);
        }

        // GET: CheckTimes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkTime = await _context.CheckTime.FindAsync(id);
            if (checkTime == null)
            {
                return NotFound();
            }
            return View(checkTime);
        }

        // POST: CheckTimes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CheckTimeNo,CheckTimeName")] CheckTime checkTime)
        {
            if (id != checkTime.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(checkTime);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CheckTimeExists(checkTime.Id))
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
            return View(checkTime);
        }

        // GET: CheckTimes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkTime = await _context.CheckTime
                .FirstOrDefaultAsync(m => m.Id == id);
            if (checkTime == null)
            {
                return NotFound();
            }

            return View(checkTime);
        }

        // POST: CheckTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var checkTime = await _context.CheckTime.FindAsync(id);
            _context.CheckTime.Remove(checkTime);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CheckTimeExists(int id)
        {
            return _context.CheckTime.Any(e => e.Id == id);
        }
    }
}
