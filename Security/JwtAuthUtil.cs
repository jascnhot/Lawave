using Jose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Lawave.Security
{
    public class JwtAuthUtil : ActionFilterAttribute
    {
        private const string Key = "BsetRocketTW";
        /// <summary>
        /// 取的已存在Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static Dictionary<string, object> GetToken(string token)
        {
            return JWT.Decode<Dictionary<string, object>>(token, Encoding.UTF8.GetBytes(Key), JwsAlgorithm.HS512);
        }

        public static int GetTokenId(string token)
        {
            var tokenData = JWT.Decode<Dictionary<string, object>>(token, Encoding.UTF8.GetBytes(Key), JwsAlgorithm.HS512);
            return (int)tokenData["id"];
        }

        internal static int GetTokenId(object parameter)
        {
            throw new NotImplementedException();
        }

        public static bool GetTokenisLawyer(string token)
        {
            var tokenData = JWT.Decode<Dictionary<string, object>>(token, Encoding.UTF8.GetBytes(Key), JwsAlgorithm.HS512);
            return (bool)tokenData["isLawyer"];
        }

        //過濾有用標籤[JwtAuthFilter] 請求的 JwtToken 狀態
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string secretKey = "BsetRocketTW"; // 加解密的 key,如果不一樣會無法成功解密
            var request = actionContext.Request;
            if (!WithoutVerifyToken(request.RequestUri.ToString()))
            {
                if (request.Headers.Authorization == null || request.Headers.Authorization.Scheme != "Bearer")
                {

                    var errorMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        ReasonPhrase = "Lost Token",
                        Content = new StringContent("Token遺失"),
                    };
                    throw new HttpResponseException(errorMessage);
                }
                else
                {
                    try
                    {
                        // 解密後會回傳 Json 格式的物件 (即加密前的資料)
                        var jwtObject = JWT.Decode<Dictionary<string, Object>>(
                        request.Headers.Authorization.Parameter,
                        Encoding.UTF8.GetBytes(secretKey),
                        JwsAlgorithm.HS512);

                        if (IsTokenExpired(jwtObject["Exp"].ToString()))
                        {
                            var errorMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
                            {                                
                                ReasonPhrase = "Token Expired",
                                Content = new StringContent("JwtToken 過期，需導引重新登入"),
                            };
                            throw new HttpResponseException(errorMessage);
                            //throw new Exception("JwtToken 過期，需導引重新登入");
                        }
                    }
                    catch (Exception e)
                    {
                        var errorMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            ReasonPhrase = "Lost Token",
                            Content = new StringContent($"Token遺失,發生錯誤：{e}"),

                        };
                        throw new HttpResponseException(errorMessage);
                        //throw new Exception(e.Message); ;
                    }

                }
            }

            base.OnActionExecuting(actionContext);
        }
        //public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        //{
        //    var request = actionContext.Request;

        //    if (request.Headers.Authorization == null || request.Headers.Authorization.Scheme != "Bearer")
        //    {
        //        throw new Exception("Token 遺失");
        //    }

        //    //解密後會回傳Json格式的物件(即加密前的資料)
        //    try
        //    {
        //        var jwtObject = JWT.Decode<Dictionary<string, object>>(request.Headers.Authorization.Parameter,
        //            Encoding.UTF8.GetBytes(Key), JwsAlgorithm.HS512);
        //        if (IsTokenExpired(jwtObject["Exp"].ToString()))
        //        {
        //            throw new Exception("Token 過期");
        //        }
        //    }
        //    catch
        //    {
        //        // ignored
        //    }
        //    base.OnActionExecuting(actionContext);
        //}
        /// <summary>
        /// Login不需要驗證因為還沒有token
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public bool WithoutVerifyToken(string requestUri)
        {
            if (requestUri.EndsWith("/login"))
                return true;
            return false;
        }
        
        /// <summary>
        /// 驗證token時效
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public bool IsTokenExpired(string dateTime)
        {
            return Convert.ToDateTime(dateTime) < DateTime.Now;
        }
    }
}