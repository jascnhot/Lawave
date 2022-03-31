using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Lawave.Models
{
    public class lawyerAccount
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

        [Display(Name = "驗證圖片-律師證")]//顯示名稱
        public string verifyPhotolawyer { get; set; }

        [Display(Name = "驗證圖片-身分證")]//顯示名稱
        public string verifyPhotoFir { get; set; }

        [Display(Name = "驗證圖片-第二證明")]//顯示名稱
        public string verifyPhotoSec { get; set; }

        [Display(Name = "是否公開個人資訊")]//顯示名稱
        public bool isPublic { get; set; }

        [MaxLength(30)]//限制最大字數，未設定為Max
        [Display(Name = "phone")]//顯示名稱
        public string phone { get; set; }

        //[MaxLength()]//限制最大字數，未設定為Max
        [Display(Name = "名言")]//顯示名稱
        public string saying { get; set; }

        //[MaxLength()]//限制最大字數，未設定為Max
        [Display(Name = "律師介紹")]//顯示名稱
        public string introduction { get; set; }

        //[MaxLength()]//限制最大字數，未設定為Max
        //[Display(Name = "工作經歷")]//顯示名稱
        //public string experience { get; set; }

        //[MaxLength()]//限制最大字數，未設定為Max
        //[Display(Name = "學歷")]//顯示名稱
        //public string education { get; set; }

        //[MaxLength()]//限制最大字數，未設定為Max
        //[Display(Name = "學位")]//顯示名稱
        //public string bachelor { get; set; }

        //[MaxLength()]//限制最大字數，未設定為Max
        [Display(Name = "專業")]//顯示名稱
        public string professional { get; set; }

        //[MaxLength()]//限制最大字數，未設定為Max
        [Display(Name = "費用")]//顯示名稱
        public string phoneCost { get; set; }

        //[MaxLength()]//限制最大字數，未設定為Max
        [Display(Name = "費用")]//顯示名稱
        public string faceCost { get; set; }

        [Display(Name = "驗證狀態")]//顯示名稱
        public bool isCertification { get; set; }

        [Display(Name = "預約時間規則")]//顯示名稱
        public AppointmentRule rule { get; set; }

        [Display(Name = "總評分")]//顯示名稱
        public int? totalScore { get; set; }

        [Display(Name = "建立時間")]
        public DateTime initDate { get; set; }


        [JsonIgnore]
        public virtual ICollection<lawyerGoodAtType> lawyerGoodAtType { get; set; }

        [JsonIgnore]
        public virtual ICollection<lawyerArea> lawyerArea { get; set; }

        [JsonIgnore]
        public virtual ICollection<lawyerBlacklist> lawyerBlacklist { get; set; }

        [JsonIgnore]
        public virtual ICollection<publicCollection> publicCollection { get; set; }

        [JsonIgnore]
        public virtual ICollection<appointmentlist> appointmentlist { get; set; }

        [JsonIgnore]
        public virtual ICollection<LawyerEducation> lawyerEducations { get; set; }

        [JsonIgnore]
        public virtual ICollection<LawyerExperience> LawyerExperiences { get; set; }
    }

    public class AppointmentRule
    {
        public string sum { get; set; }
        public string mon { get; set; }
        public string tues { get; set; }
        public string wed { get; set; }
        public string thur { get; set; }
        public string fri { get; set; }
        public string sat { get; set; }
    }


}