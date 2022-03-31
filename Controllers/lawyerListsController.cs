using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Lawave.Models;
using Lawave.Security;

namespace Lawave.Controllers
{
    [EnableCors("*", "*", "*")]
    public class lawyerListsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public class searchLawyer
        {
            public goodAtInfoEnum goodAtInfoId { set; get; }
            public areaEnum area { set; get; }
        }

        //搜尋後顯示(待修正
        [HttpGet]
        [Route("api/lawyerlistsearch/{goodAt}/{area}/{page:int?}")]
        public IHttpActionResult getareatype(goodAtInfoEnum goodAt, areaEnum area, int page = 1)//show資料分頁功能
        {
            var wbpath = "https://" + HttpContext.Current.Request.Url.Host + @"/img/shot/";
            int count = 9;//顯示9筆
            bool isPublicAccount = false;
            int publicId = 0;

            var allIds = db.lawyerAccounts.Select(user => user.id);
            var areaLawyerIds = area != areaEnum.未選擇? db.lawyerAreas.Where(user => user.area == area).Select(user => user.lawyerAccountId) : allIds;
            var goodAtLawyerIds = goodAt != goodAtInfoEnum.未選擇? db.lawyerGoodAtTypes.Where(user =>  user.goodAtInfoId == goodAt).Select(user => user.lawyerAccountId) : allIds;
            var selectIds = areaLawyerIds.Intersect(goodAtLawyerIds);

            var lawyerlist = db.lawyerAccounts.Where(user => user.isCertification == true & user.isPublic == true & selectIds.Contains(user.id))
                .OrderByDescending(p => p.totalScore).ThenBy(p => p.id).Skip((page - 1) * count).Take(count);


            //最大頁數
            double maxcount = lawyerlist!=null? lawyerlist.Count() / (double)count:0;
            var maxpage = Math.Ceiling(maxcount);

            try
            {
                var isLawyer = JwtAuthUtil.GetTokenisLawyer(Request.Headers.Authorization.Parameter);
                publicId = JwtAuthUtil.GetTokenId(Request.Headers.Authorization.Parameter);
                if (!isLawyer) isPublicAccount = true;
            }
            catch
            {
                isPublicAccount = false;
            }

            var datalist = lawyerlist.Select(x => new
            {
                id = x.id,
                name = x.lastName + x.firstName,
                office = x.LawyerExperiences.OrderByDescending(item => item.id).FirstOrDefault().companyName,
                saying = x.saying,
                shot = x.shot != null && x.shot != "" ? wbpath + x.shot : null,
                lawyerGoodAtType = x.lawyerGoodAtType.Select(item => item.goodAtInfoId.ToString()),
                collection = isPublicAccount ? x.publicCollection.Where(item => item.publicAccountId == publicId).Count() > 0 ? true : false : false
            });
            return Ok(new { maxpage = (int)maxpage, data = datalist });
        }
        /// <summary>
        /// 律師列表get用
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/lawyerlist/{page:int?}")]
        public IHttpActionResult getpage(int page = 1)//show資料分頁功能
        {
            var wbpath = "https://" + HttpContext.Current.Request.Url.Host + @"/img/shot/";
            int count = 9;//顯示9筆
            bool isPublicAccount = false;
            int publicId = 0;

            //最大頁數
            double maxcount = db.lawyerAccounts.Count() / (double)count;
            var maxpage = Math.Ceiling(maxcount);

            var lawyerlist = db.lawyerAccounts.Where(user => user.isCertification == true & user.isPublic == true).OrderByDescending(p => p.totalScore).ThenBy(p => p.id).Skip((page - 1) * count).Take(count);


            if (!ModelState.IsValid) return BadRequest(ModelState);


            try
            {
                var isLawyer = JwtAuthUtil.GetTokenisLawyer(Request.Headers.Authorization.Parameter);
                publicId = JwtAuthUtil.GetTokenId(Request.Headers.Authorization.Parameter);
                if (!isLawyer) isPublicAccount = true;
            }
            catch
            {
                isPublicAccount = false;
            }

            var datalist = lawyerlist.Select(x => new
            {

                id = x.id,
                name = x.lastName + x.firstName,
                office = x.LawyerExperiences.OrderByDescending(item=>item.id).FirstOrDefault().companyName,
                saying = x.saying,
                shot = x.shot != null && x.shot != "" ? wbpath + x.shot : null,
                lawyerGoodAtType = x.lawyerGoodAtType.Select(item=>item.goodAtInfoId.ToString()),
                collection = isPublicAccount ? x.publicCollection.Where(item => item.publicAccountId == publicId).Count() > 0 ? true : false : false
            });
            return Ok(new { maxpage = (int)maxpage, data = datalist });
            //var token = JwtAuthUtil.GetToken(Request.Headers.Authorization.Parameter);
            //bool isLawyer =  JwtAuthUtil.GetTokenisLawyer(Request.Headers.Authorization.Parameter);

            //if (!isLawyer & db.publicCollections.Count() > 0)
            //{

            //    int publicTokenid = Convert.ToInt32(token["id"]);
            //    var status = db.publicCollections.Where(x => x.publicAccountId == publicTokenid).Select(x => x.lawyerAccountId);

            //    return Ok(new { maxpage = (int)maxpage, data = datalist, publicCollections=status });
            //}

            //else
            //{
            //    return Ok(new { maxpage = (int)maxpage, data = datalist });
            //}
        }


        //愛心收藏功能(新增+刪除
        [HttpPost]
        [JwtAuthUtil]
        [Route("api/PublicCollection/{lawyerid}")]
        public IHttpActionResult PutCollection(int lawyerid)
        {
            var token = JwtAuthUtil.GetToken(Request.Headers.Authorization.Parameter);
            int publicTokenid = Convert.ToInt32(token["id"]);
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var status = db.publicCollections.Where(x => x.publicAccountId == publicTokenid & x.lawyerAccountId == lawyerid).FirstOrDefault();

            publicCollection collectionlist = new publicCollection
            {
                lawyerAccountId = lawyerid,
                publicAccountId = publicTokenid,
            };
            //如果null就新增資料
            if (status == null)
            {
                db.publicCollections.Add(collectionlist);
            }
            else
            {
                db.publicCollections.Remove(status);
            }
            db.SaveChanges();


            return Ok(publicTokenid);
        }

        [HttpGet]
        [Route("api/lawyerlist/selectlist")]
        public IHttpActionResult getSelectlist()
        {
            var areaList=Enum.GetValues(typeof(areaEnum)).Cast<areaEnum>().ToList<areaEnum>().OrderBy(item => item).Select(item=>item.ToString());
            var goodAtList= Enum.GetValues(typeof(goodAtInfoEnum)).Cast<goodAtInfoEnum>().ToList<goodAtInfoEnum>().OrderBy(item => item).Select(item => item.ToString());

            return Ok(new { areaList = areaList, goodAtList = goodAtList });
        }

        // GET: api/lawyerLists
        public IQueryable<lawyerAccount> GetlawyerAccounts()
        {
            return db.lawyerAccounts;
        }

        // GET: api/lawyerLists/5
        [ResponseType(typeof(lawyerAccount))]
        public IHttpActionResult GetlawyerAccount(int id)
        {
            lawyerAccount lawyerAccount = db.lawyerAccounts.Find(id);
            if (lawyerAccount == null)
            {
                return NotFound();
            }

            return Ok(lawyerAccount);
        }

        // PUT: api/lawyerLists/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutlawyerAccount(int id, lawyerAccount lawyerAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != lawyerAccount.id)
            {
                return BadRequest();
            }

            db.Entry(lawyerAccount).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!lawyerAccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/lawyerLists
        [ResponseType(typeof(lawyerAccount))]
        public IHttpActionResult PostlawyerAccount(lawyerAccount lawyerAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.lawyerAccounts.Add(lawyerAccount);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = lawyerAccount.id }, lawyerAccount);
        }

        // DELETE: api/lawyerLists/5
        [ResponseType(typeof(lawyerAccount))]
        public IHttpActionResult DeletelawyerAccount(int id)
        {
            lawyerAccount lawyerAccount = db.lawyerAccounts.Find(id);
            if (lawyerAccount == null)
            {
                return NotFound();
            }

            db.lawyerAccounts.Remove(lawyerAccount);
            db.SaveChanges();

            return Ok(lawyerAccount);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool lawyerAccountExists(int id)
        {
            return db.lawyerAccounts.Count(e => e.id == id) > 0;
        }

        //private List<string> goodAtOjtoList(ist<goodAtInfoEnum> items)
        //{
        //    List<string> result = new List<string>();
        //    foreach(goodAtInfoEnum item in items)
        //    {
        //        result.Add(item.ToString());
        //    }
        //    return result;
        //}
    }
}