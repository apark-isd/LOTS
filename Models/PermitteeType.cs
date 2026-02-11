using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LOTS3.Models
{
    public class PermitteeType
    {
        [Key]
        public int Id { get; set; }

        //[Required]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        //[Display(Name = "Permittee Type No")]
        //public int PermitteeTypeNo { get; set; }

        //[Required]
        [Display(Name = "Permittee Type Name")]
        public string PermitteeTypeName { get; set; }

        public virtual ICollection<Permit> Permits { get; set; }
    }
}
