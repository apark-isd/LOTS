using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LOTS3.Models
{
    public class PayType
    {
        public PayType()
        {
            Permits = new List<Permit>();
            //DepartmentId = 0;
            //Department = new Department();
        }

        [Key]
        public int Id { get; set; }

        //[Required]
        [Display(Name = "Pay Type")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string PayTypeName { get; set; }
                
        [Display(Name = "Pay Type Amount")]
        [DataType(DataType.Currency)]
        [DisplayFormat(NullDisplayText = "n/a", ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal PayTypeAmount { get; set; }

        public virtual ICollection<Permit> Permits { get; set; }

        //[Display(Name = "Department ID")]
        //public int DepartmentId { get; set; }
        //public virtual Department Department { get; set; }  


    }
}
