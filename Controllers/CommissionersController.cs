using LOTS3.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
namespace LOTS3.Controllers
{
    public class CommissionersController : Controller
    {
        private readonly AppDbContext _context;
        public CommissionersController(AppDbContext context)
        {
            _context = context;
        }
       
        public async Task<ActionResult> CommissionerRequested()
        {
            var model = new List<Commissioner>();
            var query = _context.Commissioner
                        .Include(c => c.Department)
                        .Include(c => c.Lot)
                        .Include(c => c.StatusType);
            model = await query.Where(c => c.StatusTypeId== 1 || c.StatusTypeId == 4)
                        .ToListAsync();

            return View(model);
        }

        public async Task<IActionResult> Index()
        {
            string UserName = HttpContext.User.Identity.Name;
            int startIndex = UserName.IndexOf("@") + "@".Length;
            int endIndex = UserName.IndexOf(".");

            if (startIndex > endIndex)
            {
                UserName = UserName.Substring(startIndex, UserName.Length - startIndex);
                startIndex = 0;
                endIndex = UserName.IndexOf(".");
            }

            string deptAb = UserName.Substring(startIndex, endIndex - startIndex);
            var deptId = (from p in _context.Department
                          where p.DepartmentAbrv == deptAb
                          select p.Id).FirstOrDefault();
            if (User.IsInRole("Coordinator"))
            {
                var appDbContext = _context.Commissioner
                        .Include(c => c.Department)
                        .Include(c => c.Lot)
                        .Include(c => c.StatusType)
                        .Where(c => c.DepartmentId == deptId);
                return View(await appDbContext.ToListAsync());
            }
            else
            {
                var appDbContext = _context.Commissioner
                        .Include(c => c.Department)
                        .Include(c => c.Lot)
                        .Include(c => c.StatusType);
                return View(await appDbContext.ToListAsync());
            }
        }
        // GET: Commissioners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();
            var commissioner = await _context.Commissioner
                .Include(c => c.Department)
                .Include(c => c.Lot)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commissioner == null)
            {
                return NotFound();
            }
            return View(commissioner);
        }
        // GET: Commissioners/Create
        public IActionResult Create()
        {
            string UserName = HttpContext.User.Identity.Name;
            int startIndex = UserName.IndexOf("@") + "@".Length;
            int endIndex = UserName.IndexOf(".");
            string deptAb = UserName.Substring(startIndex, endIndex - startIndex);
            var deptId = (from p in _context.Department
                          where p.DepartmentAbrv == deptAb
                          select p.Id).FirstOrDefault();
            TempData["deptId"] = deptId;
            ViewBag.Departments = _context.Department;
            ViewBag.Lots = _context.Lot;
            return View();
        }
        // POST: Commissioners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LastName,FirstName,StartDate,EndDate,CardNumber,DepartmentId,LotId")] Commissioner commissioner)
        {
            if (User.IsInRole("Coordinator"))
            {
                string userName = HttpContext.User.Identity.Name;
                int startIndex = userName.IndexOf("@") + "@".Length;
                int endIndex = userName.IndexOf(".");
                string deptAb = userName.Substring(startIndex, endIndex - startIndex);
                var coordinatordDeptId = (from p in _context.Department
                                          where p.DepartmentAbrv == deptAb
                                          select p.Id).FirstOrDefault();
                var permitteeDeptId = commissioner.DepartmentId;
                if (coordinatordDeptId != permitteeDeptId)
                {
                    ViewBag.Lots = _context.Lot;
                    ViewBag.Departments = _context.Department;
                    ViewBag.PermitTypes = _context.PermitType;
                    ViewBag.PermitteeTypes = _context.PermitteeType;
                    ViewBag.PayTypes = _context.PayType;
                    ModelState.AddModelError("DepartmentId", "You cannot add other department permit");
                    return View(commissioner);
                }
            }
            if (ModelState.IsValid)
            {
                var statusTypeId = (from statusType in _context.StatusType
                                    where statusType.StatusTypeName == "Requested"
                                    select statusType.Id).FirstOrDefault();
                commissioner.StatusTypeId= statusTypeId;
                _context.Add(commissioner);
                await _context.SaveChangesAsync();

                string deptName = (from dept in _context.Department
                                   where dept.Id == commissioner.DepartmentId
                                   select dept.DepartmentName).FirstOrDefault();
                string lastName = commissioner.LastName;
                string firstName = commissioner.FirstName;
                string userName = HttpContext.User.Identity.Name;
                string NotificationBody = deptName + ", Permit Type: Commissioner" + ", "
                                        + lastName + ", " + firstName + " was created by " + userName
                                        + " details follow this link: " + "https://lots.lacounty.gov/Commissioners/Details/" + commissioner.Id; ;
                IList<string> emailLists = new List<string>();
                emailLists.Add("JAquino@isd.lacounty.gov");
                emailLists.Add(User.Identity.Name);
                var task = SendEmailAsync("ISD Parking Services", emailLists, "New Commissioner", NotificationBody, null);
                return RedirectToAction(nameof(Index));
            }
            return View(commissioner);
        }
        public async Task SendEmailAsync(string name, IList<string> toAddress, string subject, string body, IList<string> ccAddresses = null)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("ISD Parking Services", "apark@isd.lacounty.gov"));
            IEnumerable<string> enumerable = toAddress.AsEnumerable();
            foreach (var e in enumerable)
            {
                mimeMessage.To.Add(new MailboxAddress(name, e));
            }
            if (ccAddresses != null)
            {
                foreach (var cc in ccAddresses)
                    mimeMessage.Cc.Add(new MailboxAddress("", cc));
            }
            mimeMessage.Subject = subject;
            mimeMessage.Body = new TextPart("html")
            {
                Text = body
            };
            try
            {
                using (var client = new SmtpClient())
                {
                    //client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    //string emailHost = "mx1.hc5957-98.iphmx.com";
                    //await client.ConnectAsync(emailHost, 587, useSsl: false);
                    //string userId = "svcx187";
                    //string password = "Hosted4me!";
                    //client.Authenticate(userId, password);
                    //await client.SendAsync(mimeMessage);
                    //await client.DisconnectAsync(true);
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    string emailHost = "smtp-us.ser.proofpoint.com";
                    await client.ConnectAsync(emailHost, 587, useSsl: false);
                    string userId = "ISDParking_Permit";
                    string password = "W]s4h5]>QyU>qBB]L!Un";
                    client.Authenticate(userId, password);
                    await client.SendAsync(mimeMessage);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
            }
        }
        // GET: Commissioners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var commissioner = await _context.Commissioner.FindAsync(id);
            if (commissioner == null)
            {
                return NotFound();
            }
            ViewBag.Departments = _context.Department;
            ViewBag.Lots = _context.Lot;
            ViewBag.StatusTypes = _context.StatusType;
            return View(commissioner);
        }
        // POST: Commissioners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LastName,FirstName,StartDate,EndDate,CardNumber,DepartmentId,LotId,StatusTypeId,Comments")] Commissioner commissioner)
        {
            if (id != commissioner.Id)
                return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(commissioner);
                    await _context.SaveChangesAsync();
                    string deptName = (from dept in _context.Department
                                       where dept.Id == commissioner.DepartmentId
                                       select dept.DepartmentName).FirstOrDefault();
                    string lastName = commissioner.LastName;
                    string firstName = commissioner.FirstName;
                    string userName = HttpContext.User.Identity.Name;
                    string NotificationBody = deptName + ", Permit Type: Commissioner" + ", "
                                            + lastName + ", " + firstName + " was updated by " + userName
                                            + " details follow this link: " + "https://lots.lacounty.gov/Commissioners/Details/" + commissioner.Id; ;
                    IList<string> emailLists = new List<string>();
                    emailLists.Add("JAquino@isd.lacounty.gov");
                    emailLists.Add(User.Identity.Name);
                    var task = SendEmailAsync("ISD Parking Services", emailLists, "Commissioner updated", NotificationBody, null);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommissionerExists(commissioner.Id))
                    return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Departments = _context.Department;
            ViewBag.Lots = _context.Lot;
            return View(commissioner);
        }
        [Authorize(Roles = "Administrator, ISD Parking Services")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            var commissioner = await _context.Commissioner
                .Include(c => c.Department)
                .Include(c => c.Lot)
                .Include(c => c.StatusType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commissioner == null)
                return NotFound();
            ViewBag.Departments = _context.Department;
            ViewBag.Lots = _context.Lot;
            ViewBag.StatusTypes = _context.StatusType;
            return View(commissioner);
        }

        [Authorize(Roles = "Administrator, ISD Parking Services")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var commissioner = await _context.Commissioner.FindAsync(id);
            _context.Commissioner.Remove(commissioner);
            await _context.SaveChangesAsync();
            string deptName = (from dept in _context.Department
                               where dept.Id == commissioner.DepartmentId
                               select dept.DepartmentName).FirstOrDefault();
            string lastName = commissioner.LastName;
            string firstName = commissioner.FirstName;
            string userName = HttpContext.User.Identity.Name;
            string NotificationBody = deptName + ", Permit Type: Commissioner" + ", "
                                    + lastName + ", " + firstName + " was updated by " + userName;
            IList<string> emailLists = new List<string>();
            emailLists.Add("JAquino@isd.lacounty.gov");
            emailLists.Add(User.Identity.Name);
            var task = SendEmailAsync("ISD Parking Services", emailLists, "Commissioner deleted", NotificationBody, null);
            return RedirectToAction(nameof(Index));
        }
        private bool CommissionerExists(int id)
        {
            return _context.Commissioner.Any(e => e.Id == id);
        }
    }
}
