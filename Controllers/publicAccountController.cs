using Lawave.Models;
using Lawave.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Lawave.Controllers
{
    [EnableCors("*", "*", "*")]
    public class publicAccountController : ApiController
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        public class Opinion
        {
            public string lawyerStar { get; set; }//民眾評律師星數
            public string lawyerOpinion { get; set; }//民眾評價
            public string lawaveOpinion { get; set; }//平台評價
            public string LawaveStar { get; set; }//平台星數

        }


        //民眾評分會更新律師/律師評平台
        [HttpPut]
        [JwtAuthUtil]
        [Route("api/lawyerOpinion/{appid}")]
        public IHttpActionResult PostlawyerOpinion(int appid, appointmentlist Opinion)
        {

            bool isLawyer = JwtAuthUtil.GetTokenisLawyer(Request.Headers.Authorization.Parameter);
            var app = _db.appointmentlists.Find(appid);


            var lawyerAccounts = _db.lawyerAccounts.FirstOrDefault(x => x.id == app.lawyerAccountId);


            if (isLawyer)
            {
                app.lawaveStar = Opinion.lawaveStar;
                app.lawaveOpinion = Opinion.lawaveOpinion;
                app.status = appointmentStatus.completed;
                _db.SaveChanges();

                return Ok(new { message = "success", app.lawaveStar, app.lawaveOpinion });
            }
            else
            {
                
                app.lawyerStar = Opinion.lawyerStar;
                app.lawyerOpinion = Opinion.lawyerOpinion;               
                app.evalTime = DateTime.Now;
                app.status = appointmentStatus.completed;
                _db.SaveChanges();
                return Ok(new { message = "success", app.lawyerStar, app.lawyerOpinion });
            }

        }
    }
}
