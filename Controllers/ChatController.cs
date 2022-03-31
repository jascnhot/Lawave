using Lawave.Models;
using Lawave.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace Lawave.Controllers
{
    [EnableCors("*", "*", "*")]
    public class ChatController : ApiController
    {

        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        #region 取得進入聊天室的使用者資訊(姓名大頭貼
        //[JwtAuthUtil]
        [HttpGet]
        [Route("api/chatuserinfo/{id}")]
        public IHttpActionResult GetChatUserinfo(int id)
        {
            var wbpath = "https://" + HttpContext.Current.Request.Url.Host + @"/img/shot/";
            //int lawyerId = JwtAuthUtil.GetTokenId(Request.Headers.Authorization.Parameter); 
            bool isLawyer = JwtAuthUtil.GetTokenisLawyer(Request.Headers.Authorization.Parameter);

            var app = _db.appointmentlists.FirstOrDefault(y => y.id == id);
            var lawyerinfo = _db.lawyerAccounts.Where(x => x.id == app.lawyerAccountId).Select(x => new
            {
                x.firstName,
                x.lastName,
                shot = x.shot != null && x.shot != "" ? wbpath + x.shot : null,
            });
            var publicinfo = _db.publicAccounts.Where(x => x.id == app.publicAccountId).Select(x => new
            {
                x.firstName,
                x.lastName,
                shot = x.shot != null && x.shot != "" ? wbpath + x.shot : null,
            });

            if (isLawyer)
            {
                return Ok(new
                {
                    Senderinfo = lawyerinfo,
                    Recipientinfo = publicinfo,
                });
            }
            else
            {
                return Ok(new
                {
                    Senderinfo = publicinfo,
                    Recipientinfo = lawyerinfo
                });
            }
        }
        #endregion


        #region 歷史訊息
        //[JwtAuthUtil]
        [HttpGet]
        [Route("api/chatHistory/{id}")]
        public IHttpActionResult GetChatHistory(int id)
        {
            var wbpath = "https://" + HttpContext.Current.Request.Url.Host + @"/img/shot/";
            //int lawyerId = JwtAuthUtil.GetTokenId(Request.Headers.Authorization.Parameter); 
            bool isLawyer = JwtAuthUtil.GetTokenisLawyer(Request.Headers.Authorization.Parameter);

            var app = _db.appointmentlists.FirstOrDefault(y => y.id == id);

            var lawyerinfo = _db.lawyerAccounts.Where(x => x.id == app.lawyerAccountId).Select(x => new
            {
                x.firstName,
                x.lastName,
                shot = x.shot != null && x.shot != "" ? wbpath + x.shot : null,
            });
            var publicinfo = _db.publicAccounts.Where(x => x.id == app.publicAccountId).Select(x => new
            {
                x.firstName,
                x.lastName,
                shot = x.shot != null && x.shot != "" ? wbpath + x.shot : null,
            });

            var lawyersendername = _db.lawyerAccounts.Where(x => x.id == app.lawyerAccountId).Select(x => x.lastName + x.firstName);
            var publicsendername = _db.publicAccounts.Where(x => x.id == app.publicAccountId).Select(x => x.lastName + x.firstName);

            var chatHistory = _db.Chatlog;
            var history = _db.Chatlog.FirstOrDefault(x => x.AppointmentId == app.id);
           
            //if (isLawyer)
            //{

                return Ok(new
                {
                    Senderinfo = isLawyer?lawyerinfo:publicinfo,
                    Recipientinfo = isLawyer ? publicinfo:lawyerinfo,
                    
                    message = chatHistory.Where(x => (x.SenderId == app.lawyerAccountId && x.RecipientId == app.publicAccountId && x.AppointmentId == app.id) || (x.SenderId == app.publicAccountId && x.RecipientId == app.lawyerAccountId && x.AppointmentId == app.id)).OrderByDescending(x => x.MsgTime).Select(x => new
                    {
                        name = x.SenderId == app.lawyerAccountId && x.RecipientId == app.publicAccountId ? lawyersendername.FirstOrDefault() : publicsendername.FirstOrDefault(),
                        x.Message,
                        x.MsgTime,
                    })
                });
            //}
            //else 
            //{
            //    return Ok(new
            //    {
            //        Senderinfo = publicinfo,
            //        Recipientinfo = lawyerinfo,
            //        message = chatHistory.Where(x => (x.SenderId == app.lawyerAccountId && x.RecipientId == app.publicAccountId && x.AppointmentId == app.id) || (x.SenderId == app.publicAccountId && x.RecipientId == app.lawyerAccountId && x.AppointmentId == app.id)).OrderByDescending(x => x.MsgTime).Select(x => new {
            //            name = x.SenderId == app.publicAccountId && x.RecipientId == app.lawyerAccountId ? lawyersendername.FirstOrDefault() : publicsendername.FirstOrDefault(),
            //            x.Message,
            //            x.MsgTime,
            //        })
            //    });
            //}
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
