using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LOTS3.Models
{
    public class Permit
    {
        public Permit()
        {
            PermitTypeId = 0;
            PermitteeTypeId = 0;
            PayTypeId = 0;
            //StatusTypeId = 0;
            StartDate = new DateTime();
            EndDate = new DateTime();
        }
        [Key]
        public int Id { get; set; }
        [ForeignKey("Permittee")]
        public int PermitteeId { get; set; }
        [ForeignKey("Lot")]
        [Display(Name = "Lot")]
        public int LotId { get; set; }
        [Display(Name = "Permit#")]
        public string PermitNo { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Keycard#")]
        public string KeycardNo { get; set; }
        [ForeignKey("PermitType")]
        [Display(Name = "Permit")]
        public int PermitTypeId { get; set; }
        [ForeignKey("PermitteeType")]
        [Display(Name = "Permittee")]
        public int PermitteeTypeId { get; set; }
        [ForeignKey("PayType")]
        [Display(Name = "Pay")]
        public int PayTypeId { get; set; }
        [ForeignKey("Department")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        [Display(Name = "From")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }
        [Display(Name = "To")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Comments { get; set; }
        [ForeignKey("StatusType")]
        [Display(Name = "Status")]
        public int StatusTypeId { get; set; }
        public int DepartmentNo { get; set; }
        public virtual StatusType StatusType { get; set; }
        public virtual PermitType PermitType { get; set; }
        public virtual PermitteeType PermitteeType { get; set; }
        public virtual Permittee Permittee { get; set; }
        public virtual Lot Lot { get; set; }
        public virtual PayType PayType { get; set; }
        public virtual Department Department { get; set; }  
    }
}

