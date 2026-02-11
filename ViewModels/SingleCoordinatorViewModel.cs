using LOTS3.Models;
using System.ComponentModel.DataAnnotations;

namespace LOTS3.ViewModels
{
    public class SingleCoordinatorViewModel
    {
        
        [Display(Name = "Emp No")]
        public string EmployeeNo { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Division")]
        public string Division { get; set; }

        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        //[Required]
        [Display(Name = "Department")]
        public Department Department { get; set; }
    }
}
