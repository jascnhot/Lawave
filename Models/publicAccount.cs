using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Lawave.Models
{
    public class publicAccount
    {
        [Key]//主鍵 PK
        [Display(Name = "編號")]//顯示名稱
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自動生成編號
        public int id { get; set; }
          
        [Required]//必填
        [MaxLength(200)]//限制最大字數，未設定為Max
        [Display(Name = "Mail")]//顯示名稱
        public string mail { get; set; }

        [Display(Name = "密碼")]//顯示名稱
        public string password { get; set; }

        [Display(Name = "密碼salt")]//顯示名稱
        public string PasswordSalt { get; set; }

        [Display(Name = "是否為社群登入")]//顯示名稱
        public bool isCommunity { get; set; }

        [Display(Name = "社群ID")]//顯示名稱
        public string googleID { get; set; }

        [MaxLength(200)]//限制最大字數，未設定為Max
        [Display(Name = "姓")]//顯示名稱
        public string firstName { get; set; }

        [MaxLength(200)]//限制最大字數，未設定為Max
        [Display(Name = "名字")]//顯示名稱
        public string lastName { get; set; }

        [Display(Name = "大頭貼")]//顯示名稱
        public string shot { get; set; }

        [MaxLength(30)]//限制最大字數，未設定為Max
        [Display(Name = "phone")]//顯示名稱
        public string phone { get; set; }

        //[MaxLength()]//限制最大字數，未設定為Max
        [Display(Name = "個人介紹")]//顯示名稱
        public string introduction { get; set; }

        [Display(Name = "建立時間")]
        public DateTime initDate { get; set; }

        [JsonIgnore]
        public virtual ICollection<lawyerBlacklist> lawyerBlacklist { get; set; }

        [JsonIgnore]
        public virtual ICollection<publicCollection> publicCollection { get; set; }

        [JsonIgnore]
        public virtual ICollection<appointmentlist> appointmentlist { get; set; }
    }
}