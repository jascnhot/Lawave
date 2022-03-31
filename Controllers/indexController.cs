using Lawave.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Lawave.Controllers
{
    [EnableCors("*", "*", "*")]
    public class indexController : ApiController
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// 首頁儀錶板
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/indexDashboard")]
        public IHttpActionResult getDashboard()
        {
            //媒合案件數
            var caseload = db.appointmentlists.Count();
            //諮詢總時數(分)
            var totaltime = db.appointmentlists.Count() * 60;
            //會員回饋數
            var OpinionCount = db.appointmentlists.Count(x => x.lawyerOpinion != null);
            //合作律師數量
            var cooperationLawyer = db.lawyerAccounts.Count();

            return Ok(new { caseload, totaltime, OpinionCount, cooperationLawyer });

        }


        /// <summary>
        /// 熱門律師推薦
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/indexhotlawyer")]
        public IHttpActionResult gethotlawaver()
        {
            var wbpath = "https://" + HttpContext.Current.Request.Url.Host + @"/img/shot/";
            var lawyerlist = db.lawyerAccounts.Where(user => user.isCertification == true).OrderByDescending(p => p.totalScore).ThenBy(p => p.id);
            var hotlawaver = lawyerlist.Select(x => new
            {
                name = x.lastName + x.firstName,
                office = x.LawyerExperiences.OrderByDescending(item => item.id).FirstOrDefault() != null ?
                    x.LawyerExperiences.OrderByDescending(item => item.id).FirstOrDefault().companyName : null,
                shot = x.shot != null && x.shot != "" ? wbpath + x.shot : null,

            });

            return Ok(hotlawaver);
        }

     
        /// <summary>
        /// 民眾好評
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/hotPublic")]
        public IHttpActionResult gethotPublic()
        {
            var wbpath = "https://" + HttpContext.Current.Request.Url.Host + @"/img/shot/";
            var lawyerlist = db.appointmentlists.Where(user => user.lawaveOpinion != null).OrderByDescending(p => p.lawaveStar);

            var hotlawaver = lawyerlist.Select(x => new
            {
                name = x.lawyerAccount.lastName + x.lawyerAccount.firstName,
                lawyeropinion = x.lawaveOpinion,
                shot = x.lawyerAccount.shot != null && x.lawyerAccount.shot != "" ? wbpath + x.lawyerAccount.shot : null,

            });

            return Ok(hotlawaver);
        }




    }
}
