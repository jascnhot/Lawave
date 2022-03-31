using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Lawave.Models
{
    public class goodAtInfo
    {
        [Key]//主鍵 PK
        [Display(Name = "擅長領域編號")]//顯示名稱
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自動生成編號
        public int id { get; set; }

        [Required]//必填
        [MaxLength(20)]//限制最大字數，未設定為Max
        [Display(Name = "擅長領域名稱")]//顯示名稱
        public string name { get; set; }

        [Display(Name = "固定")]//顯示名稱
        public bool isFix { get; set; }

        //[JsonIgnore]
        //public virtual ICollection<lawyerGoodAtType> lawyerGoodAtType { get; set; }
    }
}