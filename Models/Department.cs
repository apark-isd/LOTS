using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LOTS3.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        //[Required]
        [Display(Name="Department No")]
        public int DepartmentNo { get; set; }

        //[Required]
        [Display(Name = "Department Abrv")]
        public string DepartmentAbrv { get; set; }

        [Display(Name = "Fund Org")]
        public int FundOrg { get; set; }
        
        [Display(Name = "Department")]
        public string DepartmentName { get; set; }

        //public virtual ICollection<Coordinator> Coordinators { get; set; }
        public virtual List<Coordinator> CoordinatorList { get; set; }
        
    }
}
