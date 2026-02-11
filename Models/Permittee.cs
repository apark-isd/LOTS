using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LOTS3.Models
{
    public class Permittee
    {
        public Permittee()
        {
            Permits = new List<Permit>();
            PermitHistories = new List<PermitHistory>();
            DepartmentId = 0;
            StatusTypeId = 0;
        }

        public int Id { get; set; }
       
        [NotMapped]
        public string EncryptedId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Emp No")]
        public string EmployeeNo { get; set; }
        
        //[Required]
        [StringLength(50, ErrorMessage = "First Name Cannot Exceed 50 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        //[Required]
        [StringLength(50, ErrorMessage = "Last Name Cannot Exceed 50 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        //[Required]
        [Display(Name = "Office Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Division")]
        public string Division { get; set; }

        [ForeignKey("Department")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [ForeignKey("StatusType")]
        [Display(Name = "Status Type")]
        public int StatusTypeId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Keycard")]
        public string KeycardNo { get; set; }

        public virtual StatusType StatusType { get; set; }

        public virtual Department Department { get; set; }

        public virtual ICollection<Permit> Permits { get; set; }

        public virtual ICollection<PermitHistory> PermitHistories { get; set; }

        [Display(Name = "Requested Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? RequestedDate { get; set; }

        [Display(Name = "Updated Status Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? UpdatedStatusDate { get; set; }

    }
}
