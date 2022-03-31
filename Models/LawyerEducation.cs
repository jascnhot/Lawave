using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Lawave.Models
{
    public class LawyerEducation
    {
        [Key]//主鍵 PK
        [Display(Name = "編號")]//顯示名稱
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自動生成編號
        public int id { get; set; }

        [Display(Name = "律師id")]//顯示名稱
        public int lawyerAccountId { get; set; }

        [Display(Name = "學校名稱")]
        public string schoolName { get; set; }

        [Display(Name = "學系名稱")]
        public string departmentName { get; set; }

        [Display(Name = "學位名稱")]
        public string degree { get; set; }

        [ForeignKey("lawyerAccountId")]
        public virtual lawyerAccount lawyerAccount { get; set; }
    }
}