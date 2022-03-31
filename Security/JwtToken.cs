using Jose;
using Lawave.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Lawave.Security
{
    public class JwtToken
    {
        private const string Key = "BsetRocketTW";
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        public string GenerateToken(int id, bool isLawyer)
        {

            var user = _db.lawyerAccounts.Find(id);
            var user1 = _db.publicAccounts.Find(id);

            var payload = new Dictionary<string, object>
            {

                {"id", isLawyer?user.id:user1.id},
                {"isLawyer", isLawyer},
                { "iat", DateTime.Now.ToString() },
                { "Exp", DateTime.Now.AddMinutes(60).ToString()}
            };



            //payload 需透過token傳遞的資料
            var token = JWT.Encode(payload, Encoding.UTF8.GetBytes(Key), JwsAlgorithm.HS512);//產生token
            return token;
        }
    }
}