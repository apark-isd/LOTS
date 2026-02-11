using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LOTS3.ViewModels
{
    public class SingleDepartmentViewModel
    {
        
        [Required]
        public int Id { get; set; }

        [Display(Name = "Department No")]
        public int DepartmentNo { get; set; }

        [Display(Name = "Department Abrv")]
        public string DepartmentAbrv { get; set; }

        [Display(Name = "Fund Org")]
        public int FundOrg { get; set; }

        [Display(Name = "Department")]
        public string DepartmentName { get; set; }

        public virtual List<SingleCoordinatorViewModel> DepartmentCoordinators { get; set; }
    }
}
