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
    public class PermitteeTypesController : Controller
    {
        private readonly AppDbContext _context;

        public PermitteeTypesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PermitteeTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.PermitteeType.ToListAsync());
        }

        // GET: PermitteeTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permitteeType = await _context.PermitteeType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (permitteeType == null)
            {
                return NotFound();
            }

            return View(permitteeType);
        }

        // GET: PermitteeTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PermitteeTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PermitteeTypeNo,PermitteeTypeName")] PermitteeType permitteeType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(permitteeType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(permitteeType);
        }

        // GET: PermitteeTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permitteeType = await _context.PermitteeType.FindAsync(id);
            if (permitteeType == null)
            {
                return NotFound();
            }
            return View(permitteeType);
        }

        // POST: PermitteeTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PermitteeTypeNo,PermitteeTypeName")] PermitteeType permitteeType)
        {
            if (id != permitteeType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(permitteeType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PermitteeTypeExists(permitteeType.Id))
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
            return View(permitteeType);
        }

        // GET: PermitteeTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permitteeType = await _context.PermitteeType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (permitteeType == null)
            {
                return NotFound();
            }

            return View(permitteeType);
        }

        // POST: PermitteeTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var permitteeType = await _context.PermitteeType.FindAsync(id);
            _context.PermitteeType.Remove(permitteeType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PermitteeTypeExists(int id)
        {
            return _context.PermitteeType.Any(e => e.Id == id);
        }
    }
}
