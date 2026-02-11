using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LOTS3.Models
{
    public class Lot
    {
        public Lot()
        {
            Permits = new List<Permit>();
        }

        [Key]
        public int Id { get; set; }

        [Display(Name = "Lot No")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string LotNo { get; set; }
        
        //[Required]
        [Display(Name = "Lot Name")]
        public string LotName { get; set; }

        public virtual ICollection<Permit> Permits { get; set; }
    }
}
