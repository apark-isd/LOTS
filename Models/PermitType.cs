using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LOTS3.Models
{
    public class PermitType
    {
        [Key]
        public int Id { get; set; }
        
        //[Required]
        [Display(Name = "Permit Type No")]
        public int PermitTypeNo { get; set; }

        //[Required]
        [Display(Name = "Permit Type Name")]
        public string PermitTypeName { get; set; }

        public virtual ICollection<Permit> Permits { get; set; }
    }
}
