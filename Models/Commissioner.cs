using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LOTS3.Models
{
    public class Commissioner
    {
        public Commissioner()
        {
            //DepartmentId = 0;
            //LotId = 0;
            //StartDate = new DateTime();
            //EndDate = new DateTime();
        }
        [Key]
        public int Id { get; set; }
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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Card#")]
        public string CardNumber { get; set; }
        [ForeignKey("Department")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        [ForeignKey("Lot")]
        [Display(Name = "Lot")]
        public int LotId { get; set; }
        [ForeignKey("StatusType")]
        [Display(Name ="Status")]
        public int? StatusTypeId { get; set; }
        public string Comments { get; set; }
        public virtual Department Department { get; set; }
        public virtual Lot Lot { get; set; }
        public virtual StatusType? StatusType { get; set; }
    }
}