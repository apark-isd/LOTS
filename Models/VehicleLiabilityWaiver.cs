using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LOTS3.Models
{
    public class VehicleLiabilityWaiver
    {
        [Key]
        public int Id { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Submit")]
        public DateTime? DateRequested { get; set; }
        public string? ReasonForRequest { get; set; }
        public string? Name { get; set; }
        public bool YesEmployee { get; set; }
        public string? EmployeeNumber { get; set; } 
        public bool NoCompany { get; set; }
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        [ForeignKey("Lot")]
        public int LotId { get; set; }
        public virtual Lot Lot { get; set; }
        [Display(Name="Permit No")]
        public string? PermitNo { get; set; }
        public string? Year { get; set; }
        [Display(Name = "Make/Model")]
        public string? MakeModel { get; set;  }
        public string? Color { get; set; }
        public string? LicensePlateNumber { get; set; }
        public string? Signature { get; set; }
        public string? ApprovedBy { get; set; }
        public string? AcknowledgedBy { get; set; }
        [ForeignKey("StatusType")]
        public int? StatusTypeId { get; set; }
        public virtual StatusType StatusType { get; set; }
    }
}
