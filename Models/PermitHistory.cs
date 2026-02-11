using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LOTS3.Models
{
    public class PermitHistory
    {
        public PermitHistory()
        {
            PermitteeId = 0;
            LotId = 0;
            PermitTypeId = 0;
            PermitteeTypeId = 0;
            PayTypeId = 0;
            StatusTypeId = 0;
            StatusTypeId1 = 0;
            DepartmentId = 0;
            PermitNo = "";
            KeycardNo = "";
            FirstName = "";
            LastName = "";
            StartDate = new DateTime();
            EndDate = new DateTime();
            
        }
        [Key]
        public int Id { get; set; }
        //[ForeignKey("Permittee")]
        public int? PermitteeId { get; set; }
        //[ForeignKey("Lot")]
        [Display(Name = "Lot")]
        public int? LotId { get; set; }
        [Display(Name = "Permit")]
        public string PermitNo { get; set; }
        [Display(Name = "Keycard")]
        public string KeycardNo { get; set; }
        //[ForeignKey("PermitType")]
        [Display(Name = "Permit Type")]
        public int? PermitTypeId { get; set; }
        //[ForeignKey("PermitteeType")]
        [Display(Name = "Permittee Type")]
        public int? PermitteeTypeId { get; set; }
        //[ForeignKey("PayType")]
        [Display(Name = "Pay Type")]
        public int? PayTypeId { get; set; }
        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }
        [Display(Name = "Emp No")]
        public string EmployeeNo { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }    
        [Display(Name = "First Name")]
        public string FirstName { get; set; }   
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }
        public string Comments { get; set; }
        //[ForeignKey("StatusType")]
        [Display(Name = "Status Type")]
        public int? StatusTypeId { get; set; }
        [ForeignKey("StatusType1")]
        [Display(Name = "Status Type")]
        public int? StatusTypeId1 { get; set; }
        [Display(Name = "Updated Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? UpdatedDate { get; set; }
        public virtual StatusType StatusType { get; set; }
        public virtual StatusType1 StatusType1 { get; set; }    
        public virtual PermitType PermitType { get; set; }
        public virtual PermitteeType PermitteeType { get; set; }
        public virtual Permittee Permittee { get; set; }
        public virtual Lot Lot { get; set; }
        public virtual PayType PayType { get; set; }
        public virtual Department Department { get; set; }  
    }
}

