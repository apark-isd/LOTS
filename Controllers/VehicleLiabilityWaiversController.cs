using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LOTS3.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using MimeKit;
//using System.Net.Mail;
using MailKit.Net.Smtp;

namespace LOTS3.Controllers
{
    public class VehicleLiabilityWaiversController : Controller
    {
        private readonly AppDbContext _context;

        public VehicleLiabilityWaiversController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetSignatureAsync(string employeeNo, string password)
        {
            string url = $"https://fleetapi.isd.lacounty.gov/inventory/ValidateEmployee?ei={employeeNo}&p={password}";
            string val = string.Empty;

            var client = new HttpClient();
            var response = await client.GetStringAsync(url);
            var response1 = new WebClient().DownloadString(url);

            //{ "name":"Andrew Park","department":"Internal Services #300","division":"Management Info Sys Div ","email":"APark@isd.lacounty.gov","title":"SENIOR APPLICATION DEVELOPER"}

            var definition = new
            {
                name = "",
                department = "",
                division = "",
                email = "",
                title = ""
            };

            var employee = JsonConvert.DeserializeAnonymousType(response, definition);

            if (!string.IsNullOrEmpty(response))
            {
                val = "Signed by " + employee.name.ToUpper() + ", " + employee.title.ToLower() + ", " + employee.department + " at " + DateTime.Now.ToString();
                return Json(new { value = val });
            }
            else
            {
                return Json(new { value = "Error: Invalid, please enter username and password." });
            }
        }

        public async Task<IActionResult> GetAcknowledgementAsync(string employeeNo, string password)
        {
            string url = $"https://fleetapi.isd.lacounty.gov/inventory/ValidateEmployee?ei={employeeNo}&p={password}";
            string val = string.Empty;

            var client = new HttpClient();
            var response = await client.GetStringAsync(url);
            var response1 = new WebClient().DownloadString(url);

            //{ "name":"Andrew Park","department":"Internal Services #300","division":"Management Info Sys Div ","email":"APark@isd.lacounty.gov","title":"SENIOR APPLICATION DEVELOPER"}

            var definition = new
            {
                name = "",
                department = "",
                division = "",
                email = "",
                title = ""
            };

            var employee = JsonConvert.DeserializeAnonymousType(response, definition);

            if (!string.IsNullOrEmpty(response))
            {
                val = "Validated by " + employee.name.ToUpper() + ", " + employee.title.ToLower() + ", " + employee.department + " at " + DateTime.Now.ToString();
                return Json(new { value = val });
            }
            else
            {
                return Json(new { value = "Error: Invalid, please enter username and password." });
            }
        }

        public async Task<IActionResult> GetApprovalAsync(string employeeNo, string password, string statusTypeId)
        {
            string url = $"https://fleetapi.isd.lacounty.gov/inventory/ValidateEmployee?ei={employeeNo}&p={password}";
            string val = string.Empty;

            var client = new HttpClient();
            var response = await client.GetStringAsync(url);
            var response1 = new WebClient().DownloadString(url);

            //{ "name":"Andrew Park","department":"Internal Services #300","division":"Management Info Sys Div ","email":"APark@isd.lacounty.gov","title":"SENIOR APPLICATION DEVELOPER"}

            var definition = new
            {
                name = "",
                department = "",
                division = "",
                email = "",
                title = ""
            };

            var employee = JsonConvert.DeserializeAnonymousType(response, definition);

            var statusType="";
            if (statusTypeId == "1")
                statusType = "Requested by ";
            else if (statusTypeId == "2")
                statusType = "Approved by ";
            else if (statusTypeId == "3")
                statusType = "Rejected by ";

            if (!string.IsNullOrEmpty(response))
            {
                val = statusType + employee.name.ToUpper() + ",ISD Parking Services," + employee.department + " at " + DateTime.Now.ToString();
                return Json(new { value = val });
            }
            else
            {
                return Json(new { value = "Error: Invalid, please enter username and password." });
            }
        }
        public async Task<ActionResult> VehicleLiabilityWaiversRequested()
        {
            var model = new List<VehicleLiabilityWaiver>();
            var query = _context.VehicleLiabilityWaiver
                        .Include(v => v.Department)
                        .Include(v => v.Lot)
                        .Include(v => v.StatusType);
            model = await query.Where(v => v.StatusTypeId == 1)
                        .ToListAsync();    
            return View(model);
        }
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.VehicleLiabilityWaiver
                                .Include(v => v.Department)
                                .Include(v => v.Lot)
                                .Include(v => v.StatusType);
            return View(await appDbContext.ToListAsync());
        }

        // GET: VehicleLiabilityWaivers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleLiabilityWaiver = await _context.VehicleLiabilityWaiver
                .Include(v => v.Department)
                .Include(v => v.Lot)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicleLiabilityWaiver == null)
            {
                return NotFound();
            }

            return View(vehicleLiabilityWaiver);
        }

        // GET: VehicleLiabilityWaivers/Create
        public IActionResult Create()
        {
            //ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Id");
            //ViewData["LotId"] = new SelectList(_context.Lot, "Id", "Id");
            ViewBag.Department = _context.Department;
            ViewBag.Lot = _context.Lot;
            return View();
        }

        // POST: VehicleLiabilityWaivers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateRequested,ReasonForRequest,Name,YesEmployee,EmployeeNumber,NoCompany,DepartmentId,LotId,PermitNo,Year,MakeModel,Color,LicensePlateNumber,Signature")] VehicleLiabilityWaiver vehicleLiabilityWaiver)
        {
            if (ModelState.IsValid)
            {
                vehicleLiabilityWaiver.StatusTypeId = 1;
                _context.Add(vehicleLiabilityWaiver);
                await _context.SaveChangesAsync();

                var userEmail = HttpContext.User.Identity.Name;
                int startIndex = userEmail.IndexOf("@") + "@".Length;
                int endIndex = userEmail.IndexOf(".");
                string deptAb = userEmail.Substring(startIndex, endIndex - startIndex);
                var coordinatorDept = (from d in _context.Department
                                          where d.DepartmentAbrv == deptAb
                                          select new {d.Id, d.DepartmentName}).FirstOrDefault();
                IList<string> emailLists = new List<string>();
                emailLists = (from c in _context.Coordinator
                                    where c.DepartmentId == coordinatorDept.Id
                                    select c.Email).ToList();
                string NotificationBody = coordinatorDept.DepartmentName + ", Vehicle Liability Waiver"
                        + " was submitted by Employee, " + vehicleLiabilityWaiver.EmployeeNumber + ", " + vehicleLiabilityWaiver.Name
                        + ", details follow this link: " + "https://lots.lacounty.gov/VehicleLiabilityWaivers/Edit/" + vehicleLiabilityWaiver.Id;
                var task = SendEmailAsync("ISD Parking Services", emailLists, "Vehicle Liability Waiver", NotificationBody, null);

                return RedirectToAction(nameof(Index));
            }
            //ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "DepartmentName", vehicleLiabilityWaiver.DepartmentId);
            //ViewData["LotId"] = new SelectList(_context.Lot, "Id", "LotName", vehicleLiabilityWaiver.LotId);
            ViewBag.Department = _context.Department;
            ViewBag.Lot = _context.Lot;
            return View(vehicleLiabilityWaiver);
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
        // GET: VehicleLiabilityWaivers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleLiabilityWaiver = await _context.VehicleLiabilityWaiver.FindAsync(id);
            if (vehicleLiabilityWaiver == null)
            {
                return NotFound();
            }
            //ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Id", vehicleLiabilityWaiver.DepartmentId);
            //ViewData["LotId"] = new SelectList(_context.Lot, "Id", "Id", vehicleLiabilityWaiver.LotId);
            ViewBag.Department = _context.Department;
            ViewBag.Lot = _context.Lot;
            ViewBag.StatusTypes = _context.StatusType;
            return View(vehicleLiabilityWaiver);
        }

        // POST: VehicleLiabilityWaivers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateRequested,ReasonForRequest,Name,YesEmployee,EmployeeNumber, NoCompany,DepartmentId,LotId,PermitNo,Year,MakeModel,Color,LicensePlateNumber,Signature,AcknowledgedBy,StatusTypeId,ApprovedBy")] VehicleLiabilityWaiver vehicleLiabilityWaiver)
        {
            if (id != vehicleLiabilityWaiver.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicleLiabilityWaiver);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleLiabilityWaiverExists(vehicleLiabilityWaiver.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                var userEmail = HttpContext.User.Identity.Name;
                int startIndex = userEmail.IndexOf("@") + "@".Length;
                int endIndex = userEmail.IndexOf(".");
                string deptAb = userEmail.Substring(startIndex, endIndex - startIndex);
                var coordinatorDept = (from d in _context.Department
                                       where d.DepartmentAbrv == deptAb
                                       select new { d.Id, d.DepartmentName }).FirstOrDefault();
                IList<string> emailLists = new List<string>();
                emailLists = (from c in _context.Coordinator
                              where c.DepartmentId == coordinatorDept.Id
                              select c.Email).ToList();
                emailLists.Add("PSun@isd.lacounty.gov");
                emailLists.Add("eshahverdian@isd.lacounty.gov");
                string NotificationBody;
                if (vehicleLiabilityWaiver.ApprovedBy == null)
                {
                    NotificationBody = coordinatorDept.DepartmentName + ", Vehicle Liability Waiver"
                        + " was Acknowledged by Department Coordinator,"
                        + " details follow this link: " + "https://lots.lacounty.gov/VehicleLiabilityWaivers/Edit/" + vehicleLiabilityWaiver.Id;
                }
                else
                {
                    NotificationBody = coordinatorDept.DepartmentName + ", Vehicle Liability Waiver"
                        + " was Updated by ISD Parking Services,"
                        + " details follow this link: " + "https://lots.lacounty.gov/VehicleLiabilityWaivers/Edit/" + vehicleLiabilityWaiver.Id;
                }
                var task = SendEmailAsync("ISD Parking Services", emailLists, "Vehicle Liability Waiver", NotificationBody, null);
                return RedirectToAction(nameof(Index));
            }
            //ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Id", vehicleLiabilityWaiver.DepartmentId);
            //ViewData["LotId"] = new SelectList(_context.Lot, "Id", "Id", vehicleLiabilityWaiver.LotId);
            ViewBag.Department = _context.Department;
            ViewBag.Lot = _context.Lot;
            return View(vehicleLiabilityWaiver);
        }

        // GET: VehicleLiabilityWaivers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vehicleLiabilityWaiver = await _context.VehicleLiabilityWaiver
                .Include(v => v.Department)
                .Include(v => v.Lot)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicleLiabilityWaiver == null)
            {
                return NotFound();
            }
            ViewBag.Department = _context.Department;
            ViewBag.Lot = _context.Lot;
            return View(vehicleLiabilityWaiver);
        }

        // POST: VehicleLiabilityWaivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicleLiabilityWaiver = await _context.VehicleLiabilityWaiver.FindAsync(id);
            _context.VehicleLiabilityWaiver.Remove(vehicleLiabilityWaiver);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleLiabilityWaiverExists(int id)
        {
            return _context.VehicleLiabilityWaiver.Any(e => e.Id == id);
        }
    }
}
