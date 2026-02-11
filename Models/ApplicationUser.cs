using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOTS3.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string EmployeeNo { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
    }
}
