using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LOTS3.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Location No")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string LocationNo { get; set; }

        [Display(Name = "Location Name")]
        public string LocationName { get; set; }
    }
}
