using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LOTS3.Models
{
    public class Coordinator
    {
        
        [Key]
        public int Id { get; set; }

        //[Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Emp No")]
        public string EmployeeNo { get; set; }

        //[Required]
        [StringLength(50, ErrorMessage = "First Name cannot exceed 50 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        //[Required]
        [StringLength(50, ErrorMessage = "Last Name cannot exceed 50 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Phone")]
        //[DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email")]
        //[DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Division")]
        public string Division { get; set; }

        [ForeignKey("Department")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        //[Required]
        [Display(Name = "Department")]
        public Department Department { get; set; }
    }

}
