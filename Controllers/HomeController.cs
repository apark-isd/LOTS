using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LOTS3.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System.Net.Http;
using Microsoft.Identity.Web;
using System.Net;
using Newtonsoft.Json;

namespace LOTS3.Controllers
{
    public class HomeController : Controller
    {
        /*  
        //Test error handleing with RayGun on Andrew's master 4-5-22
        public ViewResult Index(int? id)
        {
            throw new Exception("This is a test exception!");
        }
        */
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
                //val = "Signed by " + User.Identity.Name + " at " + DateTime.Now.ToString();
                val = "Signed by " + employee.name.ToUpper() + ", " + employee.title.ToLower() + ", " + employee.department + " at " + DateTime.Now.ToString();
                return Json(new {value = val});
            }
            else
            {
                return Json(new { value = "Error: Invalid, please enter username and password." });
            }
        }
        public ActionResult VehicleLiabilityWaiver()
        {
            return View();
        }

        public ActionResult MainView()
        {
            return View();
        }


        public IActionResult Index()
        {
            return View();
        }

        public ViewResult About()
        {
            return View();
        }

        public ViewResult Contact()
        {
            return View();
        }

        //[Authorize("PARCS_LOTS")]
        public ViewResult Privacy()
        {
            return View();
        }
    }
}