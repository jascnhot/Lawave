using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Lawave.Models
{
    public class lawyerBlacklist
    {
        [Key]//主鍵 PK
        [Display(Name = "律師黑名單編號")]//顯示名稱
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自動生成編號
        public int id { get; set; }

        [Display(Name = "律師id")]//顯示名稱
        public int lawyerAccountId { get; set; }

        [Display(Name = "民眾id")]//顯示名稱
        public int publicAccountId { get; set; }

        [ForeignKey("lawyerAccountId")]
        public virtual lawyerAccount lawyerAccount { get; set; }

        [ForeignKey("publicAccountId")]
        public virtual publicAccount publicAccount { get; set; }
    }
}