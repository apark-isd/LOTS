using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LOTS3.Models
{
    public class StatusType
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Status Type No")]
        public int StatusTypeNo { get; set; }
        [Display(Name = "Status")]
        public string StatusTypeName { get; set; }
        public virtual ICollection<Permit> Permits { get; set; }
        public virtual ICollection<Permittee> Permittees { get; set; }  
    }
}
