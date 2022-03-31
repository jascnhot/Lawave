using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Lawave.Models;
using Lawave.Security;
using Newtonsoft.Json;

namespace Lawave.Controllers
{
    [EnableCors("*", "*", "*")]
    public class UtilsController : ApiController
    {

        //mail驗證時間：30分鐘
        private const int VeriTime = 3;
        private const bool debug = true;
        public class Signup
        {
            public bool isLawyer { get; set; }
            public bool isCommunity { get; set; }
            public string mail { get; set; }
            public string password { get; set; }
            public string phone { get; set; }
            public string veriCode { get; set; }

            public string uid { get; set; }

        }
        public class sendVeriCode
        {
            public string toaddr { get; set; }
        }
        public class Login
        {
            public bool isLawyer { get; set; }
            public string mail { get; set; }
            public string password { get; set; }
        }
        public class ResetPwd
        {
            public bool isLawyer { get; set; }
            public string mail { get; set; }
            public string password { get; set; }
            public string rePassword { get; set; }
            public string veriCode { get; set; }

        }

        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: api/Utils
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// 簡訊驗證
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/snsVeriCode")]
        public IHttpActionResult sendSNSveriCode(sendVeriCode phone)
        {
            bool isNew = false;
            int phoneNumber = Convert.ToInt32(phone.toaddr);
            string strSMSVeriCode = VeriCode.GetVeriCode(4);
            if (!debug) VeriCode.VericodeSMSSend(phone.toaddr, strSMSVeriCode);
            var sns = _db.SnsVerification.Where(item => item.phone == phoneNumber).FirstOrDefault();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (sns == null)
            {
                isNew = true;
                sns = new snsVerification();
            }
            sns.phone = phoneNumber;
            sns.veriCode = strSMSVeriCode;
            sns.initDate = DateTime.Now;
            if (isNew) _db.SnsVerification.Add(sns);
            else _db.Entry(sns).State = EntityState.Modified;
            _db.SaveChanges();
            return Ok(strSMSVeriCode);
        }

        /// <summary>
        /// 驗證信
        /// </summary>
        /// <param name="sendMail"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/mailVeriCode")]
        public IHttpActionResult sendMailveriCode(sendVeriCode sendMail)
        {
            bool isNew = false;
            string strMailVeriCode = VeriCode.GetVeriCode(4);
            if (!debug) VeriCode.SendMailVeriCode(sendMail.toaddr, strMailVeriCode);
            var mail = _db.MailVerification.Where(item => item.mail == sendMail.toaddr).FirstOrDefault();
            if (mail == null)
            {
                isNew = true;
                mail = new mailVerification();
            }
            mail.mail = sendMail.toaddr;
            mail.veriCode = strMailVeriCode;
            mail.initDate = DateTime.Now;
            if (isNew) _db.MailVerification.Add(mail);
            else _db.Entry(mail).State = EntityState.Modified;
            _db.SaveChanges();
            return Ok(strMailVeriCode);
        }

        /// <summary>
        /// 註冊
        /// </summary>
        /// <param name="signup"></param>
        /// <returns></returns>
        //POST: api/signUp
        [HttpPost]
        [Route("api/signUp")]
        public IHttpActionResult SignUp([FromBody] Signup signup)
        {
            int id;
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (signup.isLawyer)
            {
                int phone = Convert.ToInt32(signup.phone);
                if (!(_db.lawyerAccounts.FirstOrDefault(user => user.mail == signup.mail) == null))
                    return BadRequest("帳號已經重複");
                var sns = _db.SnsVerification.Where(item => item.phone == phone).FirstOrDefault();
                if (String.IsNullOrEmpty(signup.veriCode) || sns == null || signup.veriCode != sns.veriCode) return BadRequest("驗證碼錯誤");
                if (Convert.ToInt32((DateTime.Now - sns.initDate).TotalMinutes) > VeriTime) return BadRequest("超過時間");
                lawyerAccount account = new lawyerAccount();
                account.mail = signup.mail;
                account.isCommunity = signup.isCommunity;
                if (!signup.isCommunity)
                {
                    account.PasswordSalt = salt.CreateSalt();
                    account.password = salt.GenerateHashWithSalt(signup.password, account.PasswordSalt);
                }
                account.phone = signup.phone;
                account.isCertification = false;
                account.isPublic = false;
                account.rule = new AppointmentRule();
                account.initDate = DateTime.Now;

                _db.lawyerAccounts.Add(account);
                _db.SaveChanges();

                id = _db.lawyerAccounts.FirstOrDefault(user => user.mail == signup.mail).id;

            }
            else
            {
                if (!(_db.publicAccounts.FirstOrDefault(user => user.mail == signup.mail) == null))
                    return BadRequest("帳號已經重複");
                publicAccount account = new publicAccount();
                account.mail = signup.mail;
                account.password = salt.GenerateHashWithSalt(signup.password, salt.CreateSalt());
                account.isCommunity = signup.isCommunity;
                if (!signup.isCommunity)
                {
                    account.PasswordSalt = salt.CreateSalt();
                    account.password = salt.GenerateHashWithSalt(signup.password, account.PasswordSalt);
                }
                account.initDate = DateTime.Now;
                _db.publicAccounts.Add(account);
                _db.SaveChanges();

                id = _db.publicAccounts.FirstOrDefault(user => user.mail == signup.mail).id;
            }
            var jwtAuth = new JwtToken();
            var token = jwtAuth.GenerateToken(id, signup.isLawyer);
            return Ok(new { Message = signup.isLawyer ? "律師註冊成功" : "民眾註冊成功", token = token });
        }

        /// <summary>
        /// 登入功能
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/login")]
        public IHttpActionResult login([FromBody] Login login)
        {
            int id;
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (login.isLawyer)
            {
                var user = _db.lawyerAccounts.FirstOrDefault(x => x.mail == login.mail);
                if (user == null) return BadRequest("無此帳號");
                if (!user.isCommunity)
                    if (user.password != salt.GenerateHashWithSalt(login.password, user.PasswordSalt)) return BadRequest("密碼錯誤");

                id = user.id;
            }
            else
            {
                var user = _db.publicAccounts.FirstOrDefault(x => x.mail == login.mail);
                if (user == null) return BadRequest("無此帳號");
                if (!user.isCommunity)
                    if (user.password != salt.GenerateHashWithSalt(login.password, user.PasswordSalt)) return BadRequest("密碼錯誤");
                id = user.id;
            }
            var jwtAuth = new JwtToken();
            var token = jwtAuth.GenerateToken(id, login.isLawyer);
            return Ok(new { Message = login.isLawyer ? "律師登入成功" : "民眾登入成功", token = token });
        }

        /// <summary>
        /// 變更密碼
        /// </summary>
        /// <param name="reset"></param>
        /// <returns></returns>
        //POST: api/resetPwd
        [HttpPost]
        [Route("api/resetPwd")]
        public IHttpActionResult resetPwd([FromBody] ResetPwd reset)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            mailVerification verification = _db.MailVerification.Where(item => item.mail == reset.mail).FirstOrDefault();
            if (reset.isLawyer)
            {
                var user = _db.lawyerAccounts.FirstOrDefault(x => x.mail == reset.mail);
                if (user == null) return BadRequest("無此帳號");
                if (String.IsNullOrEmpty(reset.veriCode) || verification == null || reset.veriCode != verification.veriCode) return BadRequest("驗證碼錯誤");
                if (Convert.ToInt32((DateTime.Now - verification.initDate).TotalMinutes) > VeriTime) return BadRequest("超過時間");
                if (reset.password != reset.rePassword) return BadRequest("密碼不一致");
                user.password = salt.GenerateHashWithSalt(reset.password, user.PasswordSalt);

                _db.Entry(user).State = EntityState.Modified;
                _db.SaveChanges();
            }
            else
            {
                var user = _db.publicAccounts.FirstOrDefault(x => x.mail == reset.mail);

                if (user == null) return BadRequest("無此帳號");
                if (String.IsNullOrEmpty(reset.veriCode) || verification == null || reset.veriCode != verification.veriCode) return BadRequest("驗證碼錯誤");
                if (Convert.ToInt32((DateTime.Now - verification.initDate).TotalMinutes) > VeriTime) return BadRequest("超過時間");
                if (reset.password != reset.rePassword) return BadRequest("密碼不一致");
                user.password = salt.GenerateHashWithSalt(reset.password, user.PasswordSalt);

                _db.Entry(user).State = EntityState.Modified;
                _db.SaveChanges();
            }
            return Ok(new { status = true, message = "修改成功" });
        }
      
    }
}
