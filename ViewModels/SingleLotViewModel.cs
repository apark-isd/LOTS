using LOTS3.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LOTS3.ViewModels
{
    public class SingleLotViewModel
    {
        public SingleLotViewModel()
        {
            PermitTypeId = 0;
            PermitteeTypeId = 0;
            PayTypeId = 0;
            StatusTypeId = 0;
            StartDate = null;
            EndDate = null;
        }

        [Display(Name = "Permittee")]
        public int? PermitteeId { get; set; }

        [Required]
        [Display(Name = "Lot")]
        public int? LotId { get; set; }

        [Display(Name = "Permit No")]
        public string PermitNo { get; set; }

        [Display(Name = "Keycard No")]
        public string KeycardNo { get; set; }   

        [Required]
        [Display(Name = "Permit Type")]
        public int? PermitTypeId { get; set; }

        [Required]
        [Display(Name = "Permittee Type")]
        public int? PermitteeTypeId { get; set; }

        [Required]
        [Display(Name = "Pay Type")]
        public int? PayTypeId { get; set; }

        [Display(Name = "Status Type")]
        public int? StatusTypeId { get; set; }

        [Required]
        [Display(Name = "Start")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }
        
        public string Comments { get; set; }

        public virtual Lot Lot { get; set; }

        public virtual PermitType PermitType { get; set; }

        public virtual PermitteeType PermitteeType { get; set; }

        public virtual PayType PayType { get; set; }

        public virtual StatusType StatusType { get; set; }
    }
}
