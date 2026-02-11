using LOTS3.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LOTS3.ViewModels
{
    public class PermitteeCreateViewModel
    {
        public PermitteeCreateViewModel()
        {
            //StartDate = new DateTime();
            //EndDate = new DateTime();
            //RequestedPermitDate = new DateTime();
            //PickedPermitDate = new DateTime();
        }
        
        public int Id { get; set; }
        public int tempPermitTypeId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //[Remote(action:"IsEmployeeInUse", controller:"Permittee")]
        [Display(Name = "Employee No")]
        public string EmployeeNo { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First Name cannot exceed 50 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Last Name cannot exceed 50 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Office Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        public int DepartmentNo { get; set; }

        public string Division { get; set; }

        [Display(Name = "Department")]
        public string DepartmentAbrv { get; set; }

        [Display(Name = "PermitNo")]
        public string PermitNo { get; set; }

        [Display(Name = "Status")]
        public int StatusTypeId { get; set; }

        [Required]
        [Display(Name = "Start")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [Display(Name = "End")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Keycard No")]
        public string KeycardNo { get; set; }

        public virtual Department Department { get; set; }

        public virtual StatusType StatusType { get; set; }

        public virtual List<SingleLotViewModel> MultipleLots { get; set; }

        [Display(Name = "Requested Permit Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime? RequestedPermitDate { get; set; }
        public DateTime? RequestedDate { get; set; }

        [Display(Name = "Picked Permit Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime? PickedPermitDate { get; set; }
        public DateTime? UpdatedStatusDate { get; set; }
       
    }
}

