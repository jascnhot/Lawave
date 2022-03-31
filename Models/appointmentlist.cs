using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Lawave.Models
{
    public class appointmentlist
    {
        [Key]//主鍵 PK
        [Display(Name = "預約諮詢編號")]//顯示名稱
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自動生成編號
        public int id { get; set; }

        [Display(Name = "律師id")]//顯示名稱
        public int lawyerAccountId { get; set; }

        [Display(Name = "民眾id")]//顯示名稱
        public int? publicAccountId { get; set; }

        [Display(Name = "狀態")]//顯示名稱
        public appointmentStatus status { get; set; }
        

        //[MaxLength(200)]//限制最大字數，未設定為Max
        [Display(Name = "案子敘訴")]//顯示名稱
        public string caseInfo { get; set; }

        [Display(Name = "案子類型")]//顯示名稱
        public string caseType { get; set; }

        [Display(Name = "拒絕理由")]//顯示名稱
        public string rejection { get; set; }

        [Display(Name = "律師評價")]//顯示名稱
        public string lawyerOpinion { get; set; }

        [Display(Name = "律師星星")]//顯示名稱
        public int? lawyerStar { get; set; }

        [Display(Name = "平台評價")]//顯示名稱
        public string lawaveOpinion { get; set; }

        [Display(Name = "平台星星")]//顯示名稱
        public int? lawaveStar { get; set; }

        [Display(Name = "婉拒時間")]
        public DateTime? rejectionTime { get; set; }

        [Display(Name = "諮詢時間")]
        public DateTime startTime { get; set; }

        [Display(Name = "評價時間")]
        public DateTime? evalTime { get; set; }

        [Display(Name = "建立時間")]
        public DateTime InitDate { get; set; }
             
        [ForeignKey("lawyerAccountId")]
        public virtual lawyerAccount lawyerAccount { get; set; }

        [ForeignKey("publicAccountId")]
        public virtual publicAccount publicAccount { get; set; }

        [JsonIgnore]
        public virtual ICollection<Chatlog> Chatlogs { get; set; }
    }
}