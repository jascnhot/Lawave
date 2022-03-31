using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Lawave.Models
{
    public class lawyerArea
    {
        [Key]//主鍵 PK
        [Display(Name = "律師地區編號")]//顯示名稱
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自動生成編號
        public int id { get; set; }

        [Display(Name = "律師id")]//顯示名稱
        public int lawyerAccountId { get; set; }

        [Display(Name = "地區")]//顯示名稱
        public areaEnum area { get; set; }

        [ForeignKey("lawyerAccountId")]
        public virtual lawyerAccount lawyerAccount { get; set; }
    }
}