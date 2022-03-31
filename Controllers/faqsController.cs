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
using Newtonsoft.Json;

namespace Lawave.Controllers
{
    /// <summary>
    /// 常見問題
    /// </summary>
    [EnableCors("*", "*", "*")]
    public class FaQsController : ApiController
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        /// <summary>
        /// 取得常見問題
        /// </summary>
        /// <returns></returns>
        [Route("api/faq")]
        public IHttpActionResult GetFaq()
        {
            var data = _db.FAQs.Select(x => new
            {
                x.id,
                x.title,
                x.ans,
                x.initDate,
            });
         
            //string output = JsonConvert.SerializeObject(news);
            return Ok(new { data});
        }

        /// <summary>
        /// 後台介面使用
        /// </summary>
        /// <returns></returns>
        // GET: api/FAQs
        public IQueryable<FAQ> GetFaQs()
        {
            
            return _db.FAQs;
        }
        /// <summary>
        /// 後台介面使用
        /// </summary>
        /// <returns></returns>
        // GET: api/FAQs/5
        [ResponseType(typeof(FAQ))]
        public IHttpActionResult GetFaq(int id)
        {
            FAQ fAq = _db.FAQs.Find(id);
            if (fAq == null)
            {
                return NotFound();
            }

            return Ok(fAq);
        }

        /// <summary>
        /// 後台修改
        /// </summary>
        /// <returns></returns>
        // PUT: api/FAQs/5
        /// <summary>
        /// 修改常見問題
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fAq"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFaq(int id, FAQ fAq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fAq.id)
            {
                return BadRequest();
            }

            _db.Entry(fAq).State = EntityState.Modified;

            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FaqExists(id))
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

        /// <summary>
        /// 後台編輯使用
        /// </summary>
        /// <returns></returns>
        // POST: api/FAQs
        [ResponseType(typeof(FAQ))]
        public IHttpActionResult PostFaq(FAQ fAq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.FAQs.Add(fAq);
            _db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = fAq.id }, fAq);
        }

        /// <summary>
        /// 後台刪除
        /// </summary>
        /// <returns></returns>
        // DELETE: api/FAQs/5
        [ResponseType(typeof(FAQ))]
        public IHttpActionResult DeleteFaq(int id)
        {
            FAQ fAq = _db.FAQs.Find(id);
            if (fAq == null)
            {
                return NotFound();
            }

            _db.FAQs.Remove(fAq);
            _db.SaveChanges();

            return Ok(fAq);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FaqExists(int id)
        {
            return _db.FAQs.Count(e => e.id == id) > 0;
        }



    }
}