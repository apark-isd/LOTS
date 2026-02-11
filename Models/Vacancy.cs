using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LOTS3.Models
{
    public class Vacancy
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Lot")]
        [Display(Name = "Lot")]
        public int LotId { get; set; }
        public virtual Lot Lot { get; set; }

        [ForeignKey("Location")]
        [Display(Name = "Location")]
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }

        [ForeignKey("CheckTime")]
        [Display(Name = "CheckTime")]
        public int CheckTimeId { get; set; }
        public virtual CheckTime CheckTime { get; set; }    

        public int Occupied { get; set; }

        public DateTime? DateInput { get; set; }
    }
}
