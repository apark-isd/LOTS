using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LOTS3.Models;
using LOTS3.ViewModels;

namespace LOTS3.Controllers
{
    public class PermitHistoriesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPermitteeRepository _permitteeRepository;
        //private readonly AppDbContext db;

        public PermitHistoriesController(IPermitteeRepository permitteeRepository, 
            AppDbContext context)
        {
            _permitteeRepository = permitteeRepository;
            _context = context;
        }

        // GET: PermitHistories
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.PermitHistory
                            //.Include(p => p.Permittee)
                            //    .ThenInclude(p => p.Department)
                            //.Include(p => p.Permittee)
                            //    .ThenInclude(p => p.StatusType)
                            .Include(p => p.Lot)
                            .Include(p => p.PermitteeType)
                            .Include(p => p.PermitType)
                            .Include(p => p.PayType)
                            .Include(p => p.StatusType)
                            .Include(p => p.StatusType1)
                            .Include(p => p.Department);
            return View(await appDbContext.ToListAsync());
        }

        // GET: PermitHistories/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    var permitHistory = await _context.PermitHistory
        //                    .Include(p => p.Lot)
        //                    .Include(p => p.PayType)
        //                    .Include(p => p.PermitType)
        //                    .Include(p => p.Permittee)
        //                    .Include(p => p.PermitteeType)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (permitHistory == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(permitHistory);
        //}

        // GET: PermitHistories/Create
        public ViewResult Details(int id)
        {
            Permittee permittee = _permitteeRepository.GetPermittee(id);
            if (permittee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id);
            }
            var permitteeViewModel = new PermitteeCreateViewModel();
            permitteeViewModel.Id = permittee.Id;
            permitteeViewModel.EmployeeNo = permittee.EmployeeNo;
            permitteeViewModel.FirstName = permittee.FirstName;
            permitteeViewModel.LastName = permittee.LastName;
            permitteeViewModel.Email = permittee.Email;
            permitteeViewModel.PhoneNumber = permittee.PhoneNumber;
            permitteeViewModel.Division = permittee.Division;
            permitteeViewModel.DepartmentId = permittee.DepartmentId;
            permitteeViewModel.StatusTypeId = permittee.StatusTypeId;
            permitteeViewModel.RequestedDate = permittee.RequestedDate;
            permitteeViewModel.UpdatedStatusDate = permittee.UpdatedStatusDate;

            var Results = from permitHistory in _context.PermitHistory
                          where permitHistory.PermitteeId == id
                          select new PermitHistory()
                          {
                              Id = permitHistory.Id,
                              PermitteeId = permitHistory.PermitteeId,
                              LotId = permitHistory.LotId,
                              StartDate = permitHistory.StartDate,
                              EndDate = permitHistory.EndDate,
                              PermitTypeId = permitHistory.PermitTypeId,
                              PermitteeTypeId = permitHistory.PermitteeTypeId,
                              Lot = permitHistory.Lot,
                              PermitType = permitHistory.PermitType,
                              PermitteeType = permitHistory.PermitteeType,
                              Comments = permitHistory.Comments,
                              PermitNo = permitHistory.PermitNo,
                              PayTypeId = permitHistory.PayTypeId,
                              PayType = permitHistory.PayType
                          };

            var multipleLotViewModel = new List<SingleLotViewModel>();
            foreach (var item in Results)
            {
                multipleLotViewModel.Add(new SingleLotViewModel
                {
                    LotId = item.LotId,
                    PermitteeId = item.PermitteeId,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    PermitTypeId = item.PermitTypeId,
                    PermitteeTypeId = item.PermitteeTypeId,
                    Lot = item.Lot,
                    PermitType = item.PermitType,
                    PermitteeType = item.PermitteeType,
                    Comments = item.Comments,
                    PermitNo = item.PermitNo,
                    PayTypeId = item.PayTypeId,
                    PayType = item.PayType
                });
            }
            permitteeViewModel.MultipleLots = multipleLotViewModel;
            ViewBag.Lots = _context.Lot;
            ViewBag.Departments = _context.Department;
            ViewBag.PermitTypes = _context.PermitType;
            ViewBag.PermitteeTypes = _context.PermitteeType;
            ViewBag.PayTypes = _context.PayType;
            ViewBag.StatusTypes = _context.StatusType;
            return View(permitteeViewModel);
        }

        //public IActionResult Create()
        //{
        //    ViewData["LotId"] = new SelectList(_context.Lot, "Id", "Id");
        //    ViewData["PayTypeId"] = new SelectList(_context.PayType, "Id", "Id");
        //    ViewData["PermitTypeId"] = new SelectList(_context.PermitType, "Id", "Id");
        //    ViewData["PermitteeId"] = new SelectList(_context.Permittee, "Id", "Id");
        //    ViewData["PermitteeTypeId"] = new SelectList(_context.PermitteeType, "Id", "Id");
        //    return View();
        //}

        // POST: PermitHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,PermitteeId,LotId,PermitNo,PermitTypeId,PermitteeTypeId,PayTypeId,StartDate,EndDate,Comments")] PermitHistory permitHistory)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(permitHistory);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["LotId"] = new SelectList(_context.Lot, "Id", "Id", permitHistory.LotId);
        //    ViewData["PayTypeId"] = new SelectList(_context.PayType, "Id", "Id", permitHistory.PayTypeId);
        //    ViewData["PermitTypeId"] = new SelectList(_context.PermitType, "Id", "Id", permitHistory.PermitTypeId);
        //    ViewData["PermitteeId"] = new SelectList(_context.Permittee, "Id", "Id", permitHistory.PermitteeId);
        //    ViewData["PermitteeTypeId"] = new SelectList(_context.PermitteeType, "Id", "Id", permitHistory.PermitteeTypeId);
        //    return View(permitHistory);
        //}

        // GET: PermitHistories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permitHistory = await _context.PermitHistory.FindAsync(id);
            if (permitHistory == null)
            {
                return NotFound();
            }
            ViewData["LotId"] = new SelectList(_context.Lot, "Id", "Id", permitHistory.LotId);
            ViewData["PayTypeId"] = new SelectList(_context.PayType, "Id", "Id", permitHistory.PayTypeId);
            ViewData["PermitTypeId"] = new SelectList(_context.PermitType, "Id", "Id", permitHistory.PermitTypeId);
            ViewData["PermitteeId"] = new SelectList(_context.Permittee, "Id", "Id", permitHistory.PermitteeId);
            ViewData["PermitteeTypeId"] = new SelectList(_context.PermitteeType, "Id", "Id", permitHistory.PermitteeTypeId);
            return View(permitHistory);
        }

        // POST: PermitHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PermitteeId,LotId,PermitNo,PermitTypeId,PermitteeTypeId,PayTypeId,StartDate,EndDate,Comments")] PermitHistory permitHistory)
        {
            if (id != permitHistory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(permitHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PermitHistoryExists(permitHistory.Id))
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
            ViewData["LotId"] = new SelectList(_context.Lot, "Id", "Id", permitHistory.LotId);
            ViewData["PayTypeId"] = new SelectList(_context.PayType, "Id", "Id", permitHistory.PayTypeId);
            ViewData["PermitTypeId"] = new SelectList(_context.PermitType, "Id", "Id", permitHistory.PermitTypeId);
            ViewData["PermitteeId"] = new SelectList(_context.Permittee, "Id", "Id", permitHistory.PermitteeId);
            ViewData["PermitteeTypeId"] = new SelectList(_context.PermitteeType, "Id", "Id", permitHistory.PermitteeTypeId);
            return View(permitHistory);
        }

        // GET: PermitHistories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permitHistory = await _context.PermitHistory
                .Include(p => p.Lot)
                .Include(p => p.PayType)
                .Include(p => p.PermitType)
                .Include(p => p.Permittee)
                .Include(p => p.PermitteeType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (permitHistory == null)
            {
                return NotFound();
            }

            return View(permitHistory);
        }

        [HttpGet]
        //public ViewResult Delete(int id)
        public async Task<IActionResult> Delete(int id)
        {
            Permittee permittee = _permitteeRepository.GetPermittee(id);
            if (permittee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id);
            }
            var permitteeViewModel = new PermitteeCreateViewModel();
            permitteeViewModel.EmployeeNo = permittee.EmployeeNo;
            permitteeViewModel.FirstName = permittee.FirstName;
            permitteeViewModel.LastName = permittee.LastName;
            permitteeViewModel.PhoneNumber = permittee.PhoneNumber;
            permitteeViewModel.Email = permittee.Email;
            permitteeViewModel.Division = permittee.Division;
            permitteeViewModel.DepartmentId = permittee.DepartmentId;
            permitteeViewModel.StatusTypeId = permittee.StatusTypeId;
            permitteeViewModel.RequestedDate = permittee.RequestedDate;
            permitteeViewModel.UpdatedStatusDate = permittee.UpdatedStatusDate;

            var multipleLotViewModel = new List<SingleLotViewModel>();
            var Results = from permitHistory in _context.PermitHistory
                          where permitHistory.PermitteeId == id
                          select new PermitHistory()
                          {
                              Id = permitHistory.Id,
                              LotId = permitHistory.LotId,
                              StartDate = permitHistory.StartDate,
                              EndDate = permitHistory.EndDate,
                              PermitTypeId = permitHistory.PermitTypeId,
                              PermitteeTypeId = permitHistory.PermitteeTypeId,
                              Comments = permitHistory.Comments,
                              Lot = permitHistory.Lot,
                              PermitType = permitHistory.PermitType,
                              PermitteeType = permitHistory.PermitteeType,
                              PermitNo = permitHistory.PermitNo,
                              PayTypeId = permitHistory.PayTypeId,
                              PayType = permitHistory.PayType
                          };

            foreach (var item in Results)
            {
                //foreach (var item in _context.PermitHistory)
                //{
                //    if (item.Id == result.Id)
                //    {
                        multipleLotViewModel.Add(new SingleLotViewModel
                        {
                            LotId = item.LotId,
                            StartDate = item.StartDate,
                            EndDate = item.EndDate,
                            PermitTypeId = item.PermitTypeId,
                            PermitteeTypeId = item.PermitteeTypeId,
                            Comments = item.Comments,
                            Lot = item.Lot,
                            PermitType = item.PermitType,
                            PermitteeType = item.PermitteeType,
                            PermitNo = item.PermitNo,
                            PayTypeId = item.PayTypeId,
                            PayType = item.PayType
                        });
                //    }
                //}
            }

            ViewBag.Lots = _context.Lot;
            ViewBag.Departments = _context.Department;
            ViewBag.PermitTypes = _context.PermitType;
            ViewBag.PermitteeTypes = _context.PermitteeType;
            ViewBag.PayTypes = _context.PayType;
            ViewBag.StatusTypes = _context.StatusType;
            permitteeViewModel.MultipleLots = multipleLotViewModel;
            await _context.SaveChangesAsync();
            return View(permitteeViewModel);
        }

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var permitHistory = await _context.PermitHistory.FindAsync(id);
        //    _context.PermitHistory.Remove(permitHistory);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //Permittee permittee = _permitteeRepository.GetPermittee(id);
            //if (permittee == null)
            //{
            //    Response.StatusCode = 404;
            //    return View("EmployeeNotFound", id);
            //}
            
            //var permitteeViewModel = new PermitteeCreateViewModel();
            //permitteeViewModel.EmployeeNo = permittee.EmployeeNo;
            //permitteeViewModel.FirstName = permittee.FirstName;
            //permitteeViewModel.LastName = permittee.LastName;
            //permitteeViewModel.PhoneNumber = permittee.PhoneNumber;
            //permitteeViewModel.Email = permittee.Email;
            //permitteeViewModel.Division = permittee.Division;
            //permitteeViewModel.DepartmentId = permittee.DepartmentId;
            //permitteeViewModel.StatusTypeId = permittee.StatusTypeId;
            //permitteeViewModel.RequestedDate = permittee.RequestedDate;
            //permitteeViewModel.UpdatedStatusDate = permittee.UpdatedStatusDate;
            
            var multipleLotViewModel = new List<SingleLotViewModel>();
            var Results = from permitHistory in _context.PermitHistory
                          where permitHistory.PermitteeId == id
                          select new PermitHistory()
                          {
                              Id = permitHistory.Id,
                              LotId = permitHistory.LotId,
                              StartDate = permitHistory.StartDate,
                              EndDate = permitHistory.EndDate,
                              PermitTypeId = permitHistory.PermitTypeId,
                              PermitteeTypeId = permitHistory.PermitteeTypeId,
                              Comments = permitHistory.Comments,
                              Lot = permitHistory.Lot,
                              PermitType = permitHistory.PermitType,
                              PermitteeType = permitHistory.PermitteeType,
                              PermitNo = permitHistory.PermitNo,
                              PayTypeId = permitHistory.PayTypeId,
                              PayType = permitHistory.PayType
                          };

            foreach (var result in Results)
            {
                foreach (var item in _context.PermitHistory.ToList())
                {
                    if (item.Id == result.Id)
                    {
                        _context.Entry(item).State = EntityState.Deleted;
                    }
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PermitHistoryExists(int id)
        {
            return _context.PermitHistory.Any(e => e.Id == id);
        }
    }
}
