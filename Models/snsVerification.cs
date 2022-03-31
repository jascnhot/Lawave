using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Lawave.Models
{
    public class snsVerification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Display(Name = "phone")]
        public int phone { get; set; }

        [Required]
        [Display(Name = "Verification Code")]
        public string veriCode { get; set; }

        [Display(Name = "建立時間")]
        public DateTime initDate { get; set; }
    }
}