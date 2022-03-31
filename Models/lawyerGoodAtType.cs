using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Lawave.Models
{
    public class lawyerGoodAtType
    {
        [Key]//主鍵 PK
        [Display(Name = "律師擅長領域類別編號")]//顯示名稱
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自動生成編號
        public int id { get; set; }

        [Display(Name = "律師id")]//顯示名稱
        public int lawyerAccountId { get; set; }

        [Display(Name = "擅長領域id")]//顯示名稱
        public goodAtInfoEnum goodAtInfoId { get; set; }

        [ForeignKey("lawyerAccountId")]
        public virtual lawyerAccount lawyerAccount { get; set; }

        //[ForeignKey("goodAtInfoId")]
        //public virtual goodAtInfo goodAtInfo { get; set; }
    }
}