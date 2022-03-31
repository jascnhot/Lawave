using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Lawave.ViewModel
{
    /// <summary>
    /// forms驗證登入
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// 帳號
        /// </summary>
        [Required]
        public string UserName { get; set; }
        /// <summary>
        /// 密碼
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}