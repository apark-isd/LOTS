using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LOTS3.Models
{
    public class CheckTime
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Check Time No")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string CheckTimeNo { get; set; }

        [Display(Name = "Check Time Name")]
        public string CheckTimeName { get; set; }

    }
}
