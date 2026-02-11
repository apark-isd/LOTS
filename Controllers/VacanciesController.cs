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
    public class VacanciesController : Controller
    {
        private readonly AppDbContext _context;

        public VacanciesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Vacancies
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Vacancy.Include(v => v.CheckTime).Include(v => v.Location).Include(v => v.Lot);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Vacancies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacancy = await _context.Vacancy
                .Include(v => v.CheckTime)
                .Include(v => v.Location)
                .Include(v => v.Lot)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vacancy == null)
            {
                return NotFound();
            }

            return View(vacancy);
        }

        // GET: Vacancies/Create
        public IActionResult Create()
        {
            ViewData["CheckTimeId"] = new SelectList(_context.CheckTime, "Id", "CheckTimeName");
            ViewData["LocationId"] = new SelectList(_context.Location, "Id", "LocationName");
            ViewData["LotId"] = new SelectList(_context.Lot, "Id", "LotName");
            return View();
        }

        // POST: Vacancies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LotId,LocationId,CheckTimeId,Occupied")] Vacancy vacancy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vacancy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CheckTimeId"] = new SelectList(_context.CheckTime, "Id", "Id", vacancy.CheckTimeId);
            ViewData["LocationId"] = new SelectList(_context.Location, "Id", "Id", vacancy.LocationId);
            ViewData["LotId"] = new SelectList(_context.Lot, "Id", "Id", vacancy.LotId);
            return View(vacancy);
        }

        // GET: Vacancies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacancy = await _context.Vacancy.FindAsync(id);
            if (vacancy == null)
            {
                return NotFound();
            }
            ViewData["CheckTimeId"] = new SelectList(_context.CheckTime, "Id", "Id", vacancy.CheckTimeId);
            ViewData["LocationId"] = new SelectList(_context.Location, "Id", "Id", vacancy.LocationId);
            ViewData["LotId"] = new SelectList(_context.Lot, "Id", "Id", vacancy.LotId);
            return View(vacancy);
        }

        // POST: Vacancies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LotId,LocationId,CheckTimeId,Occupied")] Vacancy vacancy)
        {
            if (id != vacancy.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vacancy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VacancyExists(vacancy.Id))
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
            ViewData["CheckTimeId"] = new SelectList(_context.CheckTime, "Id", "Id", vacancy.CheckTimeId);
            ViewData["LocationId"] = new SelectList(_context.Location, "Id", "Id", vacancy.LocationId);
            ViewData["LotId"] = new SelectList(_context.Lot, "Id", "Id", vacancy.LotId);
            return View(vacancy);
        }

        // GET: Vacancies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacancy = await _context.Vacancy
                .Include(v => v.CheckTime)
                .Include(v => v.Location)
                .Include(v => v.Lot)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vacancy == null)
            {
                return NotFound();
            }

            return View(vacancy);
        }

        // POST: Vacancies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vacancy = await _context.Vacancy.FindAsync(id);
            _context.Vacancy.Remove(vacancy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VacancyExists(int id)
        {
            return _context.Vacancy.Any(e => e.Id == id);
        }
    }
}
