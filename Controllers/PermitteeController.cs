using LOTS3.Models;
using LOTS3.Security;
using LOTS3.ViewModels;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MimeKit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using LOTS3.Factories;
using Microsoft.AspNetCore.Mvc.Razor;
using FastMail.Web.Controllers;
using Microsoft.Extensions.Azure;
using System.Web.WebPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LOTS3.Controllers
{
    [Authorize(Roles = "Administrator, ISD Parking Services, Coordinator")]
    public class PermitteeController : BaseController
    {
        private readonly IPermitteeRepository _permitteeRepository;
        private readonly ILogger logger;
        private readonly AppDbContext db;
        private readonly IDataProtector protector;
        private readonly IConfiguration configuration;
        public PermitteeController(IPermitteeRepository permitteeRepository,
                              ILogger<PermitteeController> logger,
                              IDataProtectionProvider dataProtectionProvider,
                              DataProtectionPurposeStrings dataProtectionPurposeStrings,
                              IConfiguration configuration,
                              AppDbContext db,
                              IRazorViewEngine razorViewEngine): base(db,razorViewEngine)
        {
            _permitteeRepository = permitteeRepository;
            this.logger = logger;
            this.db = db;
            protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.EmployeeIdRouteValue);
            this.configuration = configuration;
        }

        public JsonResult GetEmployeeInfo1(string employeeNumber)
        {
            string html = "";
            if (string.IsNullOrEmpty(employeeNumber))
                return null;
            //var model = db.PermitHistory.FirstOrDefault(x => x.EmployeeNo == employeeNumber);
            var model = db.PermitHistory
                            .Include(p => p.Department)
                            .Include(p => p.PermitType)
                            .Include(p => p.PermitteeType)
                            .Include(p => p.PayType)
                            .Where(x => x.EmployeeNo == employeeNumber)
                            .OrderByDescending(x => x.UpdatedDate)
                            .ToList();
            html = RenderPartialViewToString("HistoryPermitInfo", model);
            return Json(new { html });
        }
        public JsonResult GetPermitteeInfo(string permitNumber)
        {
            string html = "";
            if (string.IsNullOrEmpty(permitNumber))
                return null;
            var model = db.PermitHistory.FirstOrDefault(x => x.PermitNo == permitNumber);

            return Json(new { html });
        }

        public IActionResult Tracking()
        {
            return View();
        }

        public JsonResult SearchEmployee(string employeeNo)
        {
            if (string.IsNullOrEmpty(employeeNo) || employeeNo.Length < 6)
                return null;

            var employees = new List<string>();
            int count0 = 0;
            string cnnStr = configuration.GetConnectionString("PermitteeDBConnection");
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                string sql = $"Select Distinct EmployeeNo ";
                sql += $" From PermitHistory Where EmployeeNo Like '%{employeeNo}%' Order by EmployeeNo";
                var cmd = cnn.CreateCommand();
                cmd.CommandText = sql;
                cnn.Open();
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (count0 >= 40)
                        break;
                    string no = dr[0].ToString();
                    employees.Add(no);
                    count0++;
                }
            }
            string html = "<div class=\"font-weight-bold mb-2\"> Select Employee Number:</div>";
            int count1 = 0;
            html += "<ul>";
            foreach (var p in employees)
            {
                if (count1 > 20)
                {
                    html += "<div class\"mt-4 text-center fw-bold\" style=\"margin-left: -20px;\">";
                    html += "To display more, enter more digits.</div>";
                    break;
                }
                html += $"<li><a href=\"#\" onclick=\"showEmployee({p})\" class=\"package-link\">{p}</a></li>";
                count1++;
            }
            html += "</ul>";
            return Json(new { html });
        }
        public JsonResult SearchPermit(string permitNo)
        {
            if (string.IsNullOrEmpty(permitNo) || permitNo.Length < 6)
                return null;

            var employees = new List<string>();
            int count0 = 0;
            string cnnStr = configuration.GetConnectionString("PermitteeDBConnection");
            using(SqlConnection cnn = new SqlConnection(cnnStr))
            {
                string sql = $"Select Distinct PermitNo ";
                sql += $" From PermitHistory Where PermitNo Like '%{permitNo}%' Order by PermitNo";
                var cmd = cnn.CreateCommand();
                cmd.CommandText = sql;
                cnn.Open();
                var dr = cmd.ExecuteReader();
                while(dr.Read())
                {
                    if (count0 >= 40)
                        break;
                    string no = dr[0].ToString();
                    employees.Add(no);
                    count0++;
                }
            }
            string html = "<div class=\"font-weight-bold mb-2\"> Select Permit Number:</div>";
            int count1 = 0;
            html += "<ul>";
            foreach (var p in  employees)
            {
                if(count1 > 20)
                {
                    html += "<div class\"mt-4 text-center fw-bold\" style=\"margin-left: -20px;\">";
                    html += "To display more, enter more digits.</div>";
                    break;
                }
                html += $"<li><a href=\"#\" onclick=\"showPermit({p})\" class=\"package-link\">{p}</a></li>";
                count1++;
            }
            html += "</ul>";
            return Json(new { html });
        }
        public async Task<ActionResult> PermitRequested(IFormCollection collection)
        {
            var model = new List<Permit>();
            var query = db.Permit
                        .Include(p => p.Permittee)
                        .Include(p => p.StatusType)
                        .Include(p => p.Department);
            model = await query.Where(p => p.StatusTypeId == 1 || p.StatusTypeId == 4)
                        .ToListAsync();
            ViewBag.Departments = db.Department;
            return View(model);
        }
        public async Task<IActionResult> Index(IFormCollection collection, int Id, int DeptId)
        {
            var model = new List<Permit>();
            var query = db.Permit
                        .Include(p => p.Permittee)
                        .Include(p => p.StatusType)
                        .Include(p => p.Department)
                        .Include(p => p.Lot)
                        .Include(p => p.PayType);
            if (User.IsInRole("Coordinator"))
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

                int departmentId;
                int departmentNo;
                
                departmentId = (from p in db.Department
                                    where p.DepartmentAbrv == deptAb
                                    select p.Id).FirstOrDefault();

                departmentNo = (from p in db.Department
                                where p.DepartmentAbrv == deptAb
                                select p.DepartmentNo).FirstOrDefault();

                model = await query.Where(p => p.DepartmentNo == departmentNo).ToListAsync();

                ViewBag.DepartmentId = departmentId;
            }
            else
            {
                if (Id != 0)
                {
                    Permittee permittee = _permitteeRepository.GetPermittee(Id);
                    var departmentId = permittee.DepartmentId;
                    model = await query.Where(p => p.DepartmentId == departmentId)
                                .ToListAsync();
                }
                else if (DeptId != 0)
                {
                    model = await query.Where(p => p.DepartmentId == DeptId).ToListAsync();
                }
                else if (string.IsNullOrEmpty(collection["DepartmentId"]) && string.IsNullOrEmpty(collection["multipleDepartments"]))
                {
                    model = await query.Take(0).ToListAsync();
                }
                else
                {
                    //var departmentId = int.Parse(collection["DepartmentId"]);
                    var multipleDepartmentId = collection["multipleDepartments"];
                    if (multipleDepartmentId.Any(a => a == 67.ToString()))
                        model = await query.ToListAsync();
                    else
                    {
                        model = await query.Where(p => multipleDepartmentId.Any(y => y == p.DepartmentId.ToString())).ToListAsync();
                    }
                    //ViewBag.DepartmentId = departmentId;
                    ViewBag.MultipleDepartments = multipleDepartmentId;
                }
            }
            ViewBag.Departments = db.Department.OrderBy(p => p.DepartmentName);
            return View(model);
        }
        //202147/445395/467348/602810/521949/534072/661714
        [HttpGet]
        public IActionResult Create()
        {
            if (User.IsInRole("Coordinator"))
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
                var deptId = (from p in db.Department
                              where p.DepartmentAbrv == deptAb
                              select p.Id).FirstOrDefault();
                TempData["deptId"] = deptId;
            }
            ViewBag.Lots = db.Lot;
            ViewBag.Departments = db.Department;
            ViewBag.PermitTypes = db.PermitType;
            ViewBag.PermitteeTypes = db.PermitteeType;
            ViewBag.PayTypes = db.PayType;
            return View();
        }
        [AcceptVerbs("Get", "Post")]
        //[AllowAnonymous]
        public IActionResult IsEmployeeInUse(string employeeNo)
        {
            var permitteeId = (from p in db.Permittee
                               where p.EmployeeNo == employeeNo
                               select p.Id).FirstOrDefault();
            var user = _permitteeRepository.GetPermittee(permitteeId);
            if (user == null)
                return Json(true);
            else
                return Json($"Email {employeeNo} is already in use");
        }
        [HttpPost]
        public IActionResult Create(IFormCollection collection, PermitteeCreateViewModel model)
        {
            if(User.IsInRole("Coordinator"))
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
                var coordinatordDeptId = (from p in db.Department
                                          where p.DepartmentAbrv == deptAb
                                          select p.Id).FirstOrDefault();
                var permitteeDeptId = model.DepartmentId;
                if (coordinatordDeptId != permitteeDeptId)
                {
                    ViewBag.Lots = db.Lot;
                    ViewBag.Departments = db.Department;
                    ViewBag.PermitTypes = db.PermitType;
                    ViewBag.PermitteeTypes = db.PermitteeType;
                    ViewBag.PayTypes = db.PayType;
                    ModelState.AddModelError("DepartmentId", "You cannot add other department permit");
                    return View(model);
                }
            }
            if (ModelState.IsValid)
            {
                var statusTypeId = (from statusType in db.StatusType
                                    where statusType.StatusTypeName == "Requested"
                                    select statusType.Id).FirstOrDefault();
                TimeZoneInfo pst = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                DateTime pstDateTime = TimeZoneInfo.ConvertTime(DateTime.Now, pst);
                Permittee newPermittee = new Permittee
                {
                    EmployeeNo = model.EmployeeNo,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    Division = model.Division,
                    DepartmentId = model.DepartmentId,
                    StatusTypeId = statusTypeId,
                    KeycardNo = model.KeycardNo,
                    RequestedDate = pstDateTime,
                    UpdatedStatusDate = model.UpdatedStatusDate
                };
                var MultipleLots = JsonConvert.DeserializeObject<List<SingleLotViewModel>>(HttpContext.Request.Form["hidden-Lots"]);
                if (MultipleLots == null)
                {
                    Response.StatusCode = 404;
                    ViewBag.Lots = db.Lot;
                    ViewBag.Departments = db.Department;
                    ViewBag.PermitTypes = db.PermitType;
                    ViewBag.PermitteeTypes = db.PermitteeType;
                    ViewBag.PayTypes = db.PayType;
                    ViewBag.JavaScriptFunction = string.Format("addLotRequired('{0}');", "Add LOT Required!");
                    ModelState.AddModelError("EmployeeNo","Add LOT Required!");
                    TempData["AddLot"] = "Add LOT Required!";
                    return View(model);
                }
                else
                {
                    var permittee = _permitteeRepository.Add(newPermittee);
                    var permitteeId = (from p in db.Permittee
                                       where p.EmployeeNo == model.EmployeeNo
                                       select p.Id).FirstOrDefault();

                    var departmentNo = (from dept in db.Department
                                         where dept.Id == model.DepartmentId
                                         select dept.DepartmentNo).FirstOrDefault();

                    foreach (var singleLot in MultipleLots)
                    {
                        var lotId = (from lot in db.Lot
                                     where lot.Id == singleLot.LotId
                                     select lot.Id).FirstOrDefault();
                        var lotno = (from lot in db.Lot
                                     where lot.Id == singleLot.LotId
                                     select lot.LotNo).FirstOrDefault();
                        var permitTypeId = (from permitType in db.PermitType
                                            where permitType.Id == singleLot.PermitTypeId
                                            select permitType.Id).FirstOrDefault();
                        var permitteeTypeId = (from permitteeType in db.PermitteeType
                                               where permitteeType.Id == singleLot.PermitteeTypeId
                                               select permitteeType.Id).FirstOrDefault();
                        var payTypeId = (from payType in db.PayType
                                         where payType.Id == singleLot.PayTypeId
                                         select payType.Id).FirstOrDefault();

                        if (lotId == 0 || permitTypeId == 0 || singleLot.StartDate == null || payTypeId == 0 || permitteeTypeId == 0 )
                        {
                            //return View("Permittee must have Info", model.EmployeeNo);
                            Response.StatusCode = 404;
                            ViewBag.Lots = db.Lot;
                            ViewBag.Departments = db.Department;
                            ViewBag.PermitTypes = db.PermitType;
                            ViewBag.PermitteeTypes = db.PermitteeType;
                            ViewBag.PayTypes = db.PayType;
                            ViewBag.JavaScriptFunction = string.Format("addLotRequired('{0}');", "Lot,Permit,Start Date,Pay,Permittee type are Required!");
                            ModelState.AddModelError("EmployeeNo", "Lot,Permit,Start Date,Pay,Permittee type are Required!");
                            TempData["AddLot"] = "Lot,Permit,Start Date,Pay,Permittee type are Required!";
                            return View(model);
                        }
                        permittee.Permits.Add(new Permit()
                        {
                            PermitteeId = permitteeId,
                            LotId = lotId,
                            StartDate = singleLot.StartDate,
                            EndDate = singleLot.EndDate,
                            PermitTypeId = permitTypeId,
                            PermitteeTypeId = permitteeTypeId,
                            Comments = singleLot.Comments,
                            PermitNo = singleLot.PermitNo,
                            PayTypeId = payTypeId,
                            DepartmentId = model.DepartmentId,

                            DepartmentNo = departmentNo,

                            StatusTypeId = statusTypeId,
                            KeycardNo = singleLot.KeycardNo,
                            UpdatedDate = pstDateTime
                        });
                        permittee.PermitHistories.Add(new PermitHistory()
                        {
                            PermitteeId = permitteeId,
                            LotId = lotId,
                            StartDate = singleLot.StartDate,
                            EndDate = singleLot.EndDate,
                            PermitTypeId = permitTypeId,
                            PermitteeTypeId = permitteeTypeId,
                            Comments = singleLot.Comments,
                            PermitNo = singleLot.PermitNo,
                            PayTypeId = payTypeId,
                            StatusTypeId = statusTypeId,
                            StatusTypeId1 = 1,
                            UpdatedDate = pstDateTime,
                            DepartmentId = model.DepartmentId,
                            EmployeeNo = model.EmployeeNo,
                            LastName = model.LastName,
                            FirstName = model.FirstName
                        });
                    }
                    db.SaveChanges();
                }
                string deptName = (from dept in db.Department
                                   where dept.Id == model.DepartmentId
                                   select dept.DepartmentName).FirstOrDefault();
                string empNo = model.EmployeeNo;
                string empLName = model.LastName;
                string empFName = model.FirstName;
                string userName = HttpContext.User.Identity.Name;
                string NotificationBody = deptName + ", Permit Type: New" + ", EmpNo : " + empNo + ", " 
                                        + empLName + ", " + empFName + " was Requested by " + userName
                                        + ". Follow this link " + "https://lots.lacounty.gov/Permittee/Details/" + newPermittee.Id
                                        + " for status update; LOTS will also email you when the Request is processed. If the request is rejected,"
                                        + " ISD Parking Services team (ISDParkingRequest@isd.lacounty.gov) will contact you to discuss details. "; 



                IList<string> isdParkingServicesEmailList = new List<string>
                {
                    "JAquino@isd.lacounty.gov",
                    "apark@isd.lacounty.gov",
                    User.Identity.Name
                };
                
                IList<string> deptCoordinatorEmailList = new List<string>();

                deptCoordinatorEmailList = (from c in db.Coordinator
                                            where c.DepartmentId == model.DepartmentId
                                            select c.Email).ToList();

                IList<string> emailLists = new List<string>();
                foreach (string isdparkingService in isdParkingServicesEmailList)
                    emailLists.Add(isdparkingService);
                foreach (string coordinator in deptCoordinatorEmailList)
                    emailLists.Add(coordinator);
                var task = SendEmailAsync("ISD Parking Services", emailLists, "New Permit", NotificationBody, null);
                return RedirectToAction("details", new { id = newPermittee.Id });
            }
            return View();
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
        //[AllowAnonymous]
        public ViewResult Details(int id)
        {
            logger.LogTrace("Trace Log");
            logger.LogDebug("Debug Log");
            logger.LogInformation("Information Log");
            logger.LogWarning("Warning Log");
            logger.LogError("Error Log");
            logger.LogCritical("Critical Log");
            Permittee permittee = _permitteeRepository.GetPermittee(id);
            if (permittee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id);
            }
            var permitteeViewModel = new PermitteeCreateViewModel();
            permitteeViewModel.Id = id;
            permitteeViewModel.EmployeeNo = permittee.EmployeeNo;
            permitteeViewModel.FirstName = permittee.FirstName;
            permitteeViewModel.LastName = permittee.LastName;
            permitteeViewModel.Email = permittee.Email;
            permitteeViewModel.PhoneNumber = permittee.PhoneNumber;
            permitteeViewModel.Division = permittee.Division;
            permitteeViewModel.DepartmentId = permittee.DepartmentId;
            permitteeViewModel.StatusTypeId = permittee.StatusTypeId;
            permitteeViewModel.RequestedDate  = permittee.RequestedDate;
            permitteeViewModel.UpdatedStatusDate = permittee.UpdatedStatusDate;

            TimeSpan cycleTime;
            if (permittee.UpdatedStatusDate != null && permittee.RequestedDate != null)
            {
                cycleTime = (TimeSpan)(permittee.UpdatedStatusDate - permittee.RequestedDate);
                ViewBag.cycleDays = cycleTime.Days;
                
                DateTime startDate = (DateTime)permittee.RequestedDate;
                DateTime endDate = (DateTime)permittee.UpdatedStatusDate;
                int businessDays = GetBusinessDays(startDate, endDate);
                ViewBag.businessDays = businessDays;
            }
            else
                ViewBag.businessDays = "";
                
            permitteeViewModel.KeycardNo = permittee.KeycardNo;
            var Results = from permit in db.Permit
                          where permit.PermitteeId == id
                          select new Permit()
                          {
                              LotId = permit.LotId, 
                              StartDate = permit.StartDate, 
                              EndDate = permit.EndDate,
                              PermitTypeId = permit.PermitTypeId, 
                              PermitteeTypeId = permit.PermitteeTypeId, 
                              Lot = permit.Lot, 
                              PermitType=permit.PermitType, 
                              PermitteeType=permit.PermitteeType,
                              Comments = permit.Comments, 
                              PermitNo = permit.PermitNo,
                              KeycardNo = permit.KeycardNo,
                              PayTypeId = permit.PayTypeId,
                              PayType = permit.PayType,
                              StatusTypeId= permit.StatusTypeId
                          };
            var multipleLotViewModel = new List<SingleLotViewModel>();
            foreach (var item in Results)
            {
                multipleLotViewModel.Add(new SingleLotViewModel
                {
                    LotId = item.LotId, 
                    StartDate = item.StartDate, 
                    EndDate = item.EndDate,
                    PermitTypeId = item.PermitTypeId,
                    PermitteeTypeId = item.PermitteeTypeId,
                    Lot = item.Lot, 
                    PermitType = item.PermitType, 
                    PermitteeType = item.PermitteeType,
                    Comments = item.Comments, 
                    PermitNo = item.PermitNo,
                    KeycardNo = item.KeycardNo,
                    PayTypeId = item.PayTypeId, 
                    PayType = item.PayType,
                    StatusTypeId = item.StatusTypeId
                });
            }
            permitteeViewModel.MultipleLots = multipleLotViewModel;
            ViewBag.Lots = db.Lot;
            ViewBag.Departments = db.Department;
            ViewBag.PermitTypes = db.PermitType;
            ViewBag.PermitteeTypes = db.PermitteeType;
            ViewBag.PayTypes = db.PayType;
            ViewBag.StatusTypes = db.StatusType;
            return View(permitteeViewModel);
        }

        public static int GetBusinessDays(DateTime startDate, DateTime endDate)
        {
            int businessDays = 0;
            DateTime currentDate = startDate;

            while (currentDate <= endDate)
            {
                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    businessDays++;
                }
                currentDate = currentDate.AddDays(1);
            }
            return businessDays;
        }

        [HttpGet]
        public ViewResult Delete(int id)
        {
            Permittee permittee = _permitteeRepository.GetPermittee(id);
            if (permittee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id);
            }
            var permitteeViewModel = new PermitteeCreateViewModel();
            permitteeViewModel.Id = id;
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
            permitteeViewModel.KeycardNo = permittee.KeycardNo;
            var multipleLotViewModel = new List<SingleLotViewModel>();
            var Results = from permit in db.Permit
                          where permit.PermitteeId == id
                          select new Permit()
                          {
                              LotId = permit.LotId,
                              StartDate = permit.StartDate,
                              EndDate = permit.EndDate,
                              PermitTypeId = permit.PermitTypeId,
                              PermitteeTypeId = permit.PermitteeTypeId,
                              Comments = permit.Comments,
                              Lot = permit.Lot,
                              PermitType = permit.PermitType,
                              PermitteeType = permit.PermitteeType,
                              PermitNo = permit.PermitNo,
                              KeycardNo = permit.KeycardNo,
                              PayTypeId = permit.PayTypeId,
                              PayType = permit.PayType
                          };
            foreach (var item in Results)
            {
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
                    KeycardNo = item.KeycardNo,
                    PayTypeId = item.PayTypeId,
                    PayType = item.PayType
                });
            }
            ViewBag.Lots = db.Lot;
            ViewBag.Departments = db.Department;
            ViewBag.PermitTypes = db.PermitType;
            ViewBag.PermitteeTypes = db.PermitteeType;
            ViewBag.PayTypes = db.PayType;
            ViewBag.StatusTypes = db.StatusType;
            permitteeViewModel.MultipleLots = multipleLotViewModel;
            return View(permitteeViewModel);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
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
            permitteeViewModel.KeycardNo = permittee.KeycardNo;
            var multipleLotViewModel = new List<SingleLotViewModel>();
            var Results = from permit in db.Permit
                          where permit.PermitteeId == id
                          select new Permit()
                          {
                              LotId = permit.LotId,
                              StartDate = permit.StartDate,
                              EndDate = permit.EndDate,
                              PermitTypeId = permit.PermitTypeId,
                              PermitteeTypeId = permit.PermitteeTypeId,
                              Comments = permit.Comments,
                              Lot = permit.Lot,
                              PermitType = permit.PermitType,
                              PermitteeType = permit.PermitteeType,
                              PermitNo = permit.PermitNo,
                              KeycardNo = permit.KeycardNo,
                              PayTypeId = permit.PayTypeId,
                              PayType = permit.PayType
                          };
            foreach (var item in Results)
            {
                permittee.PermitHistories.Add(new PermitHistory()
                {
                    PermitteeId = id,
                    LotId = item.LotId,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    PermitTypeId = item.PermitTypeId,
                    PermitteeTypeId = item.PermitteeTypeId,
                    Comments = item.Comments,
                    PermitNo = item.PermitNo,
                    PayTypeId = item.PayTypeId,
                    StatusTypeId = permittee.StatusTypeId,
                    StatusTypeId1 = 3,
                    UpdatedDate = DateTime.Now,
                    DepartmentId = permittee.DepartmentId,
                    EmployeeNo = permittee.EmployeeNo,
                    LastName = permittee.LastName,
                    FirstName = permittee.FirstName
                });
            }
            var permitTypeName = "";
            var permitNo = "";
            foreach (var item in db.Permit)
            {
                if (item.PermitteeId == id)
                {
                    permitTypeName = (from permitType in db.PermitType
                                      where permitType.Id == item.PermitTypeId
                                      select permitType.PermitTypeName).FirstOrDefault();
                    permitNo = (from permit in db.Permit
                                where permit.PermitNo == item.PermitNo
                                select permit.PermitNo).FirstOrDefault();
                    db.Entry(item).State = EntityState.Deleted;
                }
            }
            db.Permittee.Remove(permittee);
            //db.Entry(permittee).State = EntityState.Deleted;
            await db.SaveChangesAsync();
            string deptName = (from dept in db.Department
                               where dept.Id == permittee.DepartmentId
                               select dept.DepartmentName).FirstOrDefault();
            string empNo = permittee.EmployeeNo;
            string empLName = permittee.LastName;
            string empFName = permittee.FirstName;
            string userName = HttpContext.User.Identity.Name;
            string NotificationBody = deptName + ", Permit Type: " + permitTypeName + ", EmpNo: " + empNo + ", " + empLName + ", " + empFName + " was deleted by " + userName;
            //IList<string> emailLists = new List<string>();
            //emailLists.Add("eshahverdian@isd.lacounty.gov");
            //emailLists.Add(User.Identity.Name);
            IList<string> isdParkingServicesEmailList = new List<string>
            {
                "JAquino@isd.lacounty.gov" , "apark@isd.lacounty.gov",
                User.Identity.Name 
            };
            var task = SendEmailAsync("ISD Parking Services", isdParkingServicesEmailList, "Permit No: " + permitNo, NotificationBody, null);
            return RedirectToAction("Index",new {@DeptId = permittee.DepartmentId});
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Permittee permittee = _permitteeRepository.GetPermittee(id);
            var permitteeViewModel = new PermitteeCreateViewModel();
            permitteeViewModel.Id = id;
            permitteeViewModel.EmployeeNo = permittee.EmployeeNo;
            permitteeViewModel.FirstName = permittee.FirstName;
            permitteeViewModel.LastName = permittee.LastName;
            permitteeViewModel.Email = permittee.Email;
            permitteeViewModel.PhoneNumber = permittee.PhoneNumber;
            permitteeViewModel.Division = permittee.Division;
            permitteeViewModel.DepartmentId = permittee.DepartmentId;
            if (User.IsInRole("Coordinator"))
                permitteeViewModel.StatusTypeId = 1;
            else
                permitteeViewModel.StatusTypeId = permittee.StatusTypeId;
            permitteeViewModel.RequestedDate = permittee.RequestedDate;
            permitteeViewModel.UpdatedStatusDate = permittee.UpdatedStatusDate;
            //TimeSpan cycleTime;
            //if (permittee.UpdatedStatusDate != null && permittee.RequestedDate != null)
            //{
            //    cycleTime = (TimeSpan)(permittee.UpdatedStatusDate - permittee.RequestedDate);
            //    ViewBag.cycleDays = cycleTime.Days;
            //}
            //else
            //    ViewBag.cycleDays = "";
            TimeSpan cycleTime;
            if (permittee.UpdatedStatusDate != null && permittee.RequestedDate != null)
            {
                cycleTime = (TimeSpan)(permittee.UpdatedStatusDate - permittee.RequestedDate);
                ViewBag.cycleDays = cycleTime.Days;

                DateTime startDate = (DateTime)permittee.RequestedDate;
                DateTime endDate = (DateTime)permittee.UpdatedStatusDate;
                int businessDays = GetBusinessDays(startDate, endDate);
                ViewBag.businessDays = businessDays;
            }
            else
                ViewBag.businessDays = "";
            permitteeViewModel.KeycardNo = permittee.KeycardNo;
            var Results = from permit in db.Permit
                          where permit.PermitteeId == id
                          select new Permit()
                          {
                              LotId = permit.LotId, 
                              StartDate = permit.StartDate, 
                              EndDate = permit.EndDate,
                              PermitTypeId = permit.PermitTypeId, 
                              PermitteeTypeId = permit.PermitteeTypeId, 
                              Lot = permit.Lot, 
                              PermitType = permit.PermitType, 
                              PermitteeType = permit.PermitteeType,
                              Comments = permit.Comments, 
                              PermitNo = permit.PermitNo,
                              KeycardNo= permit.KeycardNo,
                              PayType = permit.PayType, 
                              PayTypeId = permit.PayTypeId,
                          };
            var multipleLotViewModel = new List<SingleLotViewModel>();
            foreach (var item in Results)
            {
                multipleLotViewModel.Add(new SingleLotViewModel
                {
                    LotId = item.LotId, 
                    StartDate = item.StartDate, 
                    EndDate = item.EndDate,
                    PermitTypeId = item.PermitTypeId, 
                    PermitteeTypeId = item.PermitteeTypeId, 
                    Lot = item.Lot, 
                    PermitType = item.PermitType, 
                    PermitteeType = item.PermitteeType,
                    Comments = item.Comments, 
                    PermitNo = item.PermitNo, 
                    KeycardNo = item.KeycardNo,
                    PayType = item.PayType, 
                    PayTypeId = item.PayTypeId,
                });
                TempData["PermitTypeId"] = item.PermitTypeId;
                permitteeViewModel.tempPermitTypeId = item.PermitTypeId;
            }
            permitteeViewModel.MultipleLots = multipleLotViewModel;
            ViewBag.MultipleLots = JsonConvert.SerializeObject(multipleLotViewModel);
            ViewBag.Lots = db.Lot; 
            ViewBag.Departments = db.Department; 
            ViewBag.PermitTypes = db.PermitType; 
            ViewBag.PermitteeTypes = db.PermitteeType;
            ViewBag.PayTypes = db.PayType;
            ViewBag.StatusTypes = db.StatusType;
            return View(permitteeViewModel);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Edit(PermitteeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Permittee permittee = _permitteeRepository.GetPermittee(model.Id);
                permittee.EmployeeNo = model.EmployeeNo;
                permittee.FirstName = model.FirstName; 
                permittee.LastName = model.LastName;
                permittee.Email = model.Email; 
                permittee.PhoneNumber = model.PhoneNumber;
                permittee.Division = model.Division; 
                permittee.DepartmentId = model.DepartmentId;
                if (User.IsInRole("Coordinator"))
                    permittee.StatusTypeId = 1;
                else
                    permittee.StatusTypeId = model.StatusTypeId;

                TimeZoneInfo pst = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                DateTime pstDateTime = TimeZoneInfo.ConvertTime(DateTime.Now, pst);

                permittee.RequestedDate = model.RequestedDate;
                //permittee.RequestedDate = pstDateTime;
                permittee.UpdatedStatusDate = pstDateTime;
                permittee.KeycardNo = model.KeycardNo;
                foreach (var item in db.Permit)
                {
                    if (item.PermitteeId == model.Id)
                    {
                        db.Entry(item).State = EntityState.Deleted;
                    }
                }
                var MultipleLots = JsonConvert.DeserializeObject<List<SingleLotViewModel>>(HttpContext.Request.Form["hidden-Lots"]);
                var deptName = (from dept in db.Department
                               where dept.Id == model.DepartmentId
                               select dept.DepartmentName).FirstOrDefault();

                var departmentNo = (from dept in db.Department
                                    where dept.Id == model.DepartmentId
                                    select dept.DepartmentNo).FirstOrDefault();

                var lotName = "";
                var permitStatus = "";
                var fromDate = "";
                var permitTypeName = "";
                var permitNo = "";
                
                foreach (var singleLot in MultipleLots)
                {
                    permitNo = singleLot.PermitNo;
                    var lotId = (from lot in db.Lot
                                 where lot.Id == singleLot.LotId
                                 select lot.Id).FirstOrDefault();
                    lotName = (from lot in db.Lot
                                 where lot.Id == singleLot.LotId
                                 select lot.LotName).FirstOrDefault();
                    var permitTypeId = (from permitType in db.PermitType
                                        where permitType.Id == singleLot.PermitTypeId
                                        select permitType.Id).FirstOrDefault();

                    //if(Convert.ToInt32(TempData["PermitTypeId"]) != permitTypeId)
                    if ( model.tempPermitTypeId != permitTypeId)
                        permittee.RequestedDate = DateTime.Now;

                    permitTypeName = (from permitType in db.PermitType
                                          where permitType.Id == singleLot.PermitTypeId
                                          select permitType.PermitTypeName).FirstOrDefault();
                    var permitteeTypeId = (from permitteeType in db.PermitteeType
                                           where permitteeType.Id == singleLot.PermitteeTypeId
                                           select permitteeType.Id).FirstOrDefault();
                    var payTypeId = (from payType in db.PayType
                                     where payType.Id == singleLot.PayTypeId
                                     select payType.Id).FirstOrDefault();
                    
                    if (lotId == 0 || permitTypeId == 0 || singleLot.StartDate == null || payTypeId == 0 || permitteeTypeId == 0)
                    {
                        Response.StatusCode = 404;
                        ViewBag.Lots = db.Lot;
                        ViewBag.Departments = db.Department;
                        ViewBag.PermitTypes = db.PermitType;
                        ViewBag.PermitteeTypes = db.PermitteeType;
                        ViewBag.PayTypes = db.PayType;
                        ViewBag.StatusTypes = db.StatusType;
                        ViewBag.MultipleLots = JsonConvert.SerializeObject(MultipleLots);
                        ViewBag.JavaScriptFunction = string.Format("addLotRequired('{0}');", "Lot,Permit,Start Date,Pay,Permittee type are Required!");
                        ModelState.AddModelError("EmployeeNo", "Lot,Permit,Start Date,Pay,Permittee type are Required!");
                        TempData["AddLot"] = "Lot,Permit,Start Date,Pay,Permittee type are Required!";
                        model.MultipleLots = MultipleLots;
                        TempData["PermitTypeId"] = singleLot.PermitTypeId;
                        return View(model);
                    }
                    
                    int statusTypeId = 0;
                    if (User.IsInRole("Coordinator"))
                        statusTypeId = 1;
                    else
                        statusTypeId = model.StatusTypeId;
                    permitStatus = (from statusType in db.StatusType
                                  where statusType.Id == model.StatusTypeId
                                  select statusType.StatusTypeName).FirstOrDefault();
                    fromDate = singleLot.StartDate.ToString();
                    var permit = new Permit() 
                    {
                        PermitteeId = model.Id,
                        LotId = lotId,
                        StartDate = singleLot.StartDate,
                        EndDate = singleLot.EndDate,
                        PermitTypeId = permitTypeId,
                        PermitteeTypeId = permitteeTypeId,
                        Comments = singleLot.Comments,
                        PermitNo = singleLot.PermitNo,
                        KeycardNo = singleLot.KeycardNo,
                        PayTypeId = payTypeId,
                        DepartmentId = model.DepartmentId,
                        DepartmentNo = departmentNo,
                        StatusTypeId = statusTypeId
                    };
                    permittee.Permits.Add(permit);
                    permittee.PermitHistories.Add(new PermitHistory()
                    {
                        PermitteeId = model.Id,
                        LotId = lotId,
                        StartDate = singleLot.StartDate,
                        EndDate = singleLot.EndDate,
                        PermitTypeId = permitTypeId,
                        PermitteeTypeId = permitteeTypeId,
                        Comments = singleLot.Comments,
                        PermitNo = singleLot.PermitNo,
                        PayTypeId = payTypeId,
                        StatusTypeId = statusTypeId,
                        StatusTypeId1 = 2,
                        UpdatedDate = pstDateTime,
                        KeycardNo = model.KeycardNo,
                        DepartmentId = model.DepartmentId,
                        EmployeeNo = model.EmployeeNo,
                        LastName = model.LastName,
                        FirstName = model.FirstName
                    });
                }
                db.SaveChanges();
                string empNo = permittee.EmployeeNo;
                string empLName = permittee.LastName;
                string empFName = permittee.FirstName;
                string userName = HttpContext.User.Identity.Name;
                string empEmail = permittee.Email;
                IList<string> isdParkingServicesEmailList = new List<string>
                {
                    "JAquino@isd.lacounty.gov",
                    "apark@isd.lacounty.gov",
                    User.Identity.Name
                };
                
                IList<string> deptCoordinatorEmailList = new List<string>();

                deptCoordinatorEmailList = (from c in db.Coordinator
                                            where c.DepartmentId == model.DepartmentId
                                            select c.Email).ToList();

                IList<string> emailLists = new List<string>();
                foreach (string isdparkingService in isdParkingServicesEmailList)
                    emailLists.Add(isdparkingService);
                foreach (string coordinator in deptCoordinatorEmailList)
                    emailLists.Add(coordinator);

                string NotificationBody = "";
              
                if(permitStatus == "Requested")
                {
                    NotificationBody = deptName + ", Permit Type: " + permitTypeName + ", EmpNo: " + empNo + ", "
                                       + empLName + ", " + empFName + " was " + permitStatus + " by " + userName
                                       + ". Follow this link " + "https://lots.lacounty.gov/Permittee/Details/" + permittee.Id
                                       + " for status update; LOTS will also email you when the Request is processed. If the request is rejected, ISD Parking Services team (ISDParkingRequest@isd.lacounty.gov) will contact you to discuss details.";
                }
                else if(permitStatus == "Approved")
                {
                    if(permitTypeName == "New" || permitTypeName == "Replace")
                    {
                        NotificationBody = deptName + ", Permit Type: " + permitTypeName + ", EmpNo: " + empNo + ", "
                                        + empLName + ", " + empFName + " was " + permitStatus + " by " + userName
                                        + ". Follow this link " + "https://lots.lacounty.gov/Permittee/Details/" + permittee.Id
                                        + " for details. ISD Parking Services team (ISDParkingRequest@isd.lacounty.gov) will notify you via email as a courtesy.";
                    }
                    else
                    {
                        NotificationBody = deptName + ", Permit Type: " + permitTypeName + ", EmpNo: " + empNo + ", "
                                        + empLName + ", " + empFName + " was " + permitStatus + " by " + userName
                                        + ". Follow this link " + "https://lots.lacounty.gov/Permittee/Details/" + permittee.Id
                                        + " for details.";
                    }
                }
                else
                {
                    NotificationBody = deptName + ", Permit Type: " + permitTypeName + ", EmpNo: " + empNo + ", "
                                       + empLName + ", " + empFName + " was " + permitStatus + " by " + userName
                                       + ". Follow this link " + "https://lots.lacounty.gov/Permittee/Details/" + permittee.Id
                                       + " for details. ISD Parking Services team (ISDParkingRequest@isd.lacounty.gov) will contact you to discuss details.";
                }
                
                //var task = SendEmailAsync("ISD Parking Services", emailLists, "Permit No: " + permitNo, NotificationBody, null);

                return RedirectToAction("details", new { id = permittee.Id });
            }
            return View(model);
        }
        public IActionResult AddLots(string lotId, string permitTypeId, string permitteeTypeId,
                string startDate, string endDate, string comments, string permitNo, string keycardNo, string payTypeId, string hiddenLots)
        {
            string val = "";
            List<SingleLotViewModel> list;
            string html = "";
            html += $"<TABLE class='table'><thead>" +
                    $"<tr align='center'>" +
                    $"<th>Lot</th>" +
                    $"<th>Permit No</th>" +
                    $"<th>Keycard No</th>" +
                    $"<th>Permit Type</th>" +
                    $"<th>Permittee Type</th>" +
                    $"<th>Start Date</th>" +
                    $"<th>End Date</th>" +
                    $"<th>Pay Type</th>" +
                    $"<th>Notes</th><th></th>" +
                    $"</tr></thead>";
            if (string.IsNullOrEmpty(hiddenLots))
                list = new List<SingleLotViewModel>();
            else
                list = JsonConvert.DeserializeObject<List<SingleLotViewModel>>(hiddenLots);
            if (lotId == null)
                lotId = "0";
            if (permitTypeId == null)
                permitTypeId = "0";
            if (permitteeTypeId == null)
                permitteeTypeId = "0";
            if (payTypeId == null)
                payTypeId = "0";
            DateTime? startdate = null;
            if (!string.IsNullOrEmpty(startDate))
                startdate = DateTime.Parse(startDate);
            DateTime? enddate = null;
            if (!string.IsNullOrEmpty(endDate)) 
                enddate = DateTime.Parse(endDate);
            var LotNo = (from Lot in db.Lot
                         where Lot.Id == Int32.Parse(lotId)
                         select Lot.LotNo).FirstOrDefault();
            list.Add(new SingleLotViewModel
            {
                LotId = Int32.Parse(lotId),
                PermitTypeId = Int32.Parse(permitTypeId),
                PermitteeTypeId = Int32.Parse(permitteeTypeId),
                //StartDate = DateTime.Parse(startDate),
                StartDate = startdate,
                //EndDate = DateTime.Parse(endDate),
                EndDate = enddate,
                Comments = comments,
                PermitNo = LotNo+"-"+permitNo,
                KeycardNo = keycardNo,
                PayTypeId = Int32.Parse(payTypeId)
            });
            val = JsonConvert.SerializeObject(list);
            html += $"<tbody>";
            int seq = 0;
            foreach (var lot in list)
            {
                var LotName = (from Lot in db.Lot
                               where Lot.Id == lot.LotId
                               select Lot.LotName).FirstOrDefault();
                var PermitTypeName = (from permitType in db.PermitType
                                      where permitType.Id == lot.PermitTypeId
                                      select permitType.PermitTypeName).FirstOrDefault();
                var PermitteeTypeName = (from permitteeType in db.PermitteeType
                                         where permitteeType.Id == lot.PermitteeTypeId
                                         select permitteeType.PermitteeTypeName).FirstOrDefault();
                var PayTypeName = (from payType in db.PayType
                                   where payType.Id == lot.PayTypeId
                                   select payType.PayTypeName).FirstOrDefault();
                var StartDate = lot.StartDate?.ToString("MM/dd/yyyy");
                var EndDate = lot.EndDate?.ToString("MM/dd/yyyy");
                var PermitNo = lot.PermitNo;
                var KeycardNo = lot.KeycardNo;
                html += $"<tr>" +
                    $"<td id='lotid-{seq}' data-id=\"{lot.LotId}\">{LotName}</td>" +
                    $"<td id='permitno-{seq}' data-id=\"{lot.PermitNo}\">{PermitNo}</td>" +
                    $"<td id='keycardno-{seq}' data-id=\"{lot.KeycardNo}\">{keycardNo}</td>" +
                    $"<td id='permittypeid-{seq}' data-id=\"{lot.PermitTypeId}\">{PermitTypeName}</td>" +
                    $"<td id='permitteetypeid-{seq}' data-id=\"{lot.PermitteeTypeId}\">{PermitteeTypeName}</td>" +
                    $"<td id='startdate-{seq}'>{StartDate}</td>" +
                    $"<td id='enddate-{seq}'>{EndDate}</td>" +
                    $"<td id='paytypeid-{seq}' data-id=\"{lot.PayTypeId}\">{PayTypeName}</td>" +
                    $"<td id='comments-{seq}'>{lot.Comments}</td>" +
                    $"<td><a href='#' class='edit' id='edit-{seq}' data-id=\"{seq}\" onclick=\"EditModal({seq})\">Edit</a></td>" +
                    $"<td><a href='#' class='delete' id='delete-{seq}' data-id=\"{seq}\" onclick=\"DeleteModal({seq})\">Delete</a></td>" +
                    "</tr>";
                seq++;
            }
            html += "</tbody></table></div>";
            return Json(
                new
                {
                    value = val,
                    html = html
                }
            );
        }
        public IActionResult UpdateLots(string seqLineNo, string lotId, string permitTypeId, string permitteeTypeId,
                        string startDate, string endDate, string comments, string permitNo, string keycardNo, string payTypeId, string hiddenLots)
        {
            string val = "";
            List<SingleLotViewModel> list;
            string html = "";
            html +=
                    $"<TABLE class='table'><thead>" +
                    $"<tr align='center'>" +
                    $"<th>Lot</th>" +
                    $"<th>Permit No</th>" +
                    $"<th>Keycard No</th>" +
                    $"<th>Permit Type</th>" +
                    $"<th>Permittee Type</th>" +
                    $"<th>Start Date</th>" +
                    $"<th>End Date</th>" +
                    $"<th>Pay Type</th>" +
                    $"<th>Notes</th>" +
                    $"<th></th>" +
                    $"</tr></thead>";

            list = JsonConvert.DeserializeObject<List<SingleLotViewModel>>(hiddenLots);
            //var item = list.FirstOrDefault(x => x.LotId == Int32.Parse(lotId)
            var item = list[Int32.Parse(seqLineNo)];
            if (lotId != null)
                item.LotId = Int32.Parse(lotId);
            else
                item.LotId = 0;
            if (permitTypeId != null)
                item.PermitTypeId = Int32.Parse(permitTypeId);
            else
                item.PermitTypeId = 0;
            if (permitteeTypeId != null)
                item.PermitteeTypeId = Int32.Parse(permitteeTypeId);
            else
                item.PermitteeTypeId = 0;
            if (payTypeId != null)
                item.PayTypeId = Int32.Parse(payTypeId);
            else
                item.PayTypeId = 0;
            DateTime? startdate = null;
            if(!string.IsNullOrEmpty(startDate))
                item.StartDate = DateTime.Parse(startDate);
            else
                item.StartDate = startdate;
            DateTime? enddate = null;
            if (!string.IsNullOrEmpty(endDate))
                item.EndDate = DateTime.Parse(endDate);
            else
                item.EndDate = enddate;
            item.Comments = comments;
            item.PermitNo = permitNo;
            item.KeycardNo= keycardNo;
            val = JsonConvert.SerializeObject(list);
            html += $"<tbody>";
            //string html = "<div>"
            int seq = 0;
            foreach (var lot in list)
            {
                var LotName = (from Lot in db.Lot
                               where Lot.Id == lot.LotId
                               select Lot.LotName).FirstOrDefault();
                var LotNo = (from Lot in db.Lot
                             where Lot.Id == lot.LotId
                             select Lot.LotNo).FirstOrDefault();
                var PermitTypeName = (from permitType in db.PermitType
                                      where permitType.Id == lot.PermitTypeId
                                      select permitType.PermitTypeName).FirstOrDefault();
                var PermitteeTypeName = (from permitteeType in db.PermitteeType
                                         where permitteeType.Id == lot.PermitteeTypeId
                                         select permitteeType.PermitteeTypeName).FirstOrDefault();
                var PayTypeName = (from payType in db.PayType
                                   where payType.Id == lot.PayTypeId
                                   select payType.PayTypeName).FirstOrDefault();
                var StartDate = lot.StartDate?.ToString("MM/dd/yyyy");
                var EndDate = lot.EndDate?.ToString("MM/dd/yyyy");
                var PermitNo = lot.PermitNo;
                var KeycardNo = lot.KeycardNo;
                html += $"<tr>" +
                    $"<td id='lotid-{seq}' data-id=\"{lot.LotId}\">{LotName}</td>" +
                    $"<td id='permitno-{seq}' data-id=\"{lot.PermitNo}\">{PermitNo}</td>" +
                    $"<td id='keycardno-{seq}' data-id=\"{lot.KeycardNo}\">{KeycardNo}</td>" +
                    $"<td id='permittypeid-{seq}' data-id=\"{lot.PermitTypeId}\">{PermitTypeName}</td>" +
                    $"<td id='permitteetypeid-{seq}' data-id=\"{lot.PermitteeTypeId}\">{PermitteeTypeName}</td>" +
                    $"<td id='startdate-{seq}'>{StartDate}</td>" +
                    $"<td id='enddate-{seq}'>{EndDate}</td>" +
                    $"<td id='paytypeid-{seq}' data-id=\"{lot.PayTypeId}\">{PayTypeName}</td>" +
                    $"<td id='comments-{seq}'>{lot.Comments}</td>" +
                    $"<td><a href='#' class='edit' id='edit-{seq}' data-id=\"{seq}\" onclick=\"EditModal({seq})\">Edit</a></td>" +
                    $"<td><a href='#' class='delete' id='delete-{seq}' data-id=\"{seq}\" onclick=\"DeleteModal({seq})\">Delete</a></td>" +
                    "</tr>";
                seq++;
            }
            html += "</tbody></table></div>";
            return Json(
                new
                {
                    value = val,
                    html = html
                }
            );
        }
        public IActionResult DeleteLots(string seqLineNo, string lotId, string permitTypeId, string permitteeTypeId,
                string startDate, string endDate, string comments, string permitNo,string keycardNo, string payTypeId, string statusTypeId, string hiddenLots)
        {
            string val = "";
            List<SingleLotViewModel> list;
            string html = "";
            html +=
                    $"<TABLE class='table'><thead>" +
                    $"<tr align='center'>" +
                    $"<th>Lot</th>" +
                    $"<th>Permit No</th>" +
                    $"<th>Keycard No</th>" +
                    $"<th>Permit Type</th>" +
                    $"<th>Permittee Type</th>" +
                    $"<th>Start Date</th>" +
                    $"<th>End Date</th>" +
                    $"<th>Pay Type</th>" +
                    $"<th>Notes</th>" +
                    $"<th></th>" +
                    $"</tr></thead>";
            list = JsonConvert.DeserializeObject<List<SingleLotViewModel>>(hiddenLots);
            //var item = list.FirstOrDefault(x => x.LotId == Int32.Parse(lotId)
            var item = list[Int32.Parse(seqLineNo)];
            list.Remove(item);
            val = JsonConvert.SerializeObject(list);
            html += $"<tbody>";
            int seq = 0;
            foreach (var lot in list)
            {
                var LotName = (from Lot in db.Lot
                               where Lot.Id == lot.LotId
                               select Lot.LotName).FirstOrDefault();
                var LotNo = (from Lot in db.Lot
                             where Lot.Id == lot.LotId
                             select Lot.LotNo).FirstOrDefault();
                var PermitTypeName = (from permitType in db.PermitType
                                      where permitType.Id == lot.PermitTypeId
                                      select permitType.PermitTypeName).FirstOrDefault();
                var PermitteeTypeName = (from permitteeType in db.PermitteeType
                                         where permitteeType.Id == lot.PermitteeTypeId
                                         select permitteeType.PermitteeTypeName).FirstOrDefault();
                var PayTypeName = (from payType in db.PayType
                                   where payType.Id == lot.PayTypeId
                                   select payType.PayTypeName).FirstOrDefault();
                var StartDate = lot.StartDate?.ToString("MM/dd/yyyy");
                var EndDate = lot.EndDate?.ToString("MM/dd/yyyy");
                var PermitNo = lot.PermitNo;
                var KeycardNo = lot.KeycardNo;
                html += $"<tr>" +
                    $"<td id='lotid-{seq}' data-id=\"{lot.LotId}\">{LotName}</td>" +
                    $"<td id='permitno-{seq}' data-id=\"{lot.PermitNo}\">{PermitNo}</td>" +
                    $"<td id='keycardno-{seq}' data-id=\"{lot.KeycardNo}\">{KeycardNo}</td>" +
                    $"<td id='permittypeid-{seq}' data-id=\"{lot.PermitTypeId}\">{PermitTypeName}</td>" +
                    $"<td id='permitteetypeid-{seq}' data-id=\"{lot.PermitteeTypeId}\">{PermitteeTypeName}</td>" +
                    $"<td id='startdate-{seq}'>{StartDate}</td>" +
                    $"<td id='enddate-{seq}'>{EndDate}</td>" +
                    $"<td id='paytypeid-{seq}' data-id=\"{lot.PayTypeId}\">{PayTypeName}</td>" +
                    $"<td id='comments-{seq}'>{lot.Comments}</td>" +
                    $"<td><a href='#' class='edit' id='edit-{seq}' data-id=\"{seq}\" onclick=\"EditModal({seq})\">Edit</a></td>" +
                    $"<td><a href='#' class='delete' id='delete-{seq}' data-id=\"{seq}\" onclick=\"DeleteModal({seq})\">Delete</a></td>" +
                    "</tr>";
                seq++;
            }
            html += "</tbody></table></div>";
            return Json(
                new
                {
                    value = val,
                    html = html
                }
            );
        }
        //For Test Emp#/202147/445395/467348/521949/534072/661714
        [HttpGet]
        public IActionResult GetEmployeeInfo(string employeeId)
        {
            string url = $"https://fleetapi.isd.lacounty.gov/parking/ge?ei={employeeId}";
            var response = new WebClient().DownloadString(url);
            var definition = new
            {
                name = "",
                givenName = "",
                sn = "",
                telephoneNumber = "",
                email = "",
                department = "",
                division = ""
            };
            var employee = JsonConvert.DeserializeAnonymousType(response, definition);
            string dept = "";
            int departmentId = 0;
            if (employee.department.Contains("Health Services"))
            {
                dept = "dhs";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Public Health"))
            {
                dept = "ph";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Public Defender"))
            {
                dept = "pubdef";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Justice"))
            {
                dept = "jcod";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Youth"))
            {
                dept = "dyd";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Superior Court"))
            {
                dept = "lacourt";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Affirmative"))
            {
                dept = "AA";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Alternate Public Defender"))
            {
                dept = "AD";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Animal"))
            {
                dept = "animalcare";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Chief Executive"))
            {
                dept = "AO";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Museum"))
            {
                dept = "AR";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Assessor"))
            {
                dept = "AS";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Auditor"))
            {
                dept = "AU";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Agricultural"))
            {
                dept = "AW";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Beaches"))
            {
                dept = "BH";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Board of Supervisors"))
            {
                dept = "BS";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Consumer"))
            {
                dept = "CA";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("County Counsel"))
            {
                dept = "CC";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Child Support"))
            {
                dept = "CD";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Children"))
            {
                dept = "CH";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Community"))
            {
                dept = "CS";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Attorney"))
            {
                dept = "DA";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Fire"))
            {
                dept = "FR";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Grand Jury"))
            {
                dept = "GJ";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("LAC/USC"))
            {
                dept = "HG";
                departmentId = (from department in db.Department where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Internal Services"))
            {
                dept = "isd";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Arts"))
            {
                dept = "RT";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Clerk"))
            {
                dept = "RR";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Park"))
            {
                dept = "PK";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Coroner"))
            {
                dept = "ME";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Public Works"))
            {
                dept = "PW";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Library"))
            {
                dept = "PL";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Human"))
            {
                dept = "HM";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Regional"))
            {
                dept = "RP";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Tax"))
            {
                dept = "TT";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Public Social Services"))
            {
                dept = "DPSS";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Probation"))
            {
                dept = "PB";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Aging"))
            {
                dept = "AG";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            else if (employee.department.Contains("Economic Opportunity"))
            {
                dept = "EW";
                departmentId = (from department in db.Department
                                where department.DepartmentAbrv == dept
                                select department.Id).FirstOrDefault();
            }
            return Json(new
            {
                id = employeeId,
                firstName = employee.givenName,
                lastName = employee.sn,
                employee.telephoneNumber,
                employee.email,
                department = dept,
                employee.division,
                departmentId = departmentId.ToString()
            });
        }
    }
}
