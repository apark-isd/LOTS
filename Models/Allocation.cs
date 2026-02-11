using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LOTS3.Models
{
    public class Allocation
    {
        [Key]
        public int Id { get; set; }
        
        [Display(Name = "Allocation")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Nullable<int> AllocationNo { get; set; }   
        
        [Display(Name = "Issued")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Nullable<int> IssuedNo { get; set; }   
        
        [Display(Name = "+-Allocation")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Nullable<int> PlusMinusAllocation { get; set; }    

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        
        [ForeignKey("Lot")]
        public int LotId { get; set; }  
        
        public virtual Department Department { get; set; }  
        
        public virtual Lot Lot { get; set; }    
    }
}
