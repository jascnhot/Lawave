using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Lawave.Models
{
    public class Chatlog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "傳送者Id")]
        public int SenderId { get; set; }

        [Display(Name = "傳送對象Id")]
        public int RecipientId { get; set; }

        [Display(Name = "訊息內容")]
        public string Message { get; set; }

        [Display(Name = "傳送時間")]
        public DateTime MsgTime { get; set; }

        [Display(Name = "預約Id")]
        public int AppointmentId { get; set; }

        [ForeignKey("AppointmentId")]
        public virtual appointmentlist Appointmentlist { get; set; }
    }
}