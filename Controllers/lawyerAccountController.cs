using Microsoft.AspNetCore.Cors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Lawave.Controllers
{
    [EnableCors("*")]
    public class lawyerAccountController : ApiController
    {
        // GET: api/lawyerAccount
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/lawyerAccount/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/lawyerAccount
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/lawyerAccount/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/lawyerAccount/5
        public void Delete(int id)
        {
        }
    }
}
