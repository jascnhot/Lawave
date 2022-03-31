using Lawave.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Lawave.Security;
using Microsoft.AspNet.SignalR.Hubs;
using System.Text;

namespace Lawave
{
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        private static Dictionary<string, string> _connectionIds;

        public void start(int id, bool isLawyer)
        {
            string mail = "";
            string othermail = "";
            string name = "";
            string othername = "";
            var app = _db.appointmentlists.Find(id);
            var lawyerInfo = app.lawyerAccount;
            var publicInfo = app.publicAccount;
            if (isLawyer)
            {
                mail = lawyerInfo.mail;
                othermail = publicInfo.mail;
                name = lawyerInfo.lastName + lawyerInfo.firstName;
            }
            else
            {
                mail = publicInfo.mail;
                othermail = lawyerInfo.mail;
                name = publicInfo.lastName + publicInfo.firstName;
            }

            if (mail != null)
            {
                if (_connectionIds == null)
                {
                    _connectionIds = new Dictionary<string, string>();
                }

                //如果帳號沒有就新增
                if (!_connectionIds.ContainsKey(mail))
                {
                    _connectionIds.Add(mail, Context.ConnectionId);
                }
                else
                {
                    _connectionIds[mail] = Context.ConnectionId;
                }
            }

            //Clients.All.addNewMessageToPage(name, "my clientid:" + Context.ConnectionId,
            //    _connectionIds[name]);

            Clients.Client(_connectionIds[mail]).register(Context.ConnectionId);

            //通知其他人，有新使用者
            if (_connectionIds.ContainsKey(othermail))
                Clients.Client(_connectionIds[othermail]).addNewMessageToPage(name);
            Clients.Caller.addNewMessageToPage(name);
        }

        #region 第一隻測試的傳送
        //public void Sendone(int id, string key, string message)
        //{

        //    var msgTime = DateTime.Now;
        //    var applist = _db.appointmentlists.Where(x => x.id == id).FirstOrDefault();


        //    foreach (var item in _connectionIds)
        //    {
        //        Clients.Client(item.Value).topople(key, message);
        //    }

        //}
        #endregion

        #region 傳送訊息並新增歷史訊息前台還沒改
        public static string String2Json(string s)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < s.Length; i++)
            {
                var c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    case '\v':
                        sb.Append("\\v"); break;
                    case '\0':
                        sb.Append("\\0"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }
        public void PrivateSendMsg(int id, string message, bool isLawyer)
        {
            string connectID = "";
            string name = "";
            var msgTime = DateTime.Now;
            var app = _db.appointmentlists.Where(x => x.id == id).FirstOrDefault();
            var lawyerInfo = app.lawyerAccount;
            var publicInfo = app.publicAccount;
            if (lawyerInfo == null || publicInfo == null) return;

            if (isLawyer)
            {
                name = lawyerInfo.lastName + lawyerInfo.firstName;
                connectID = _connectionIds.ContainsKey(publicInfo.mail) ? _connectionIds[publicInfo.mail] : null;
            }
            else
            {
                name = publicInfo.lastName + publicInfo.firstName;
                connectID = _connectionIds.ContainsKey(lawyerInfo.mail) ? _connectionIds[lawyerInfo.mail] : null;
            }
    
            if(connectID!=null)
            Clients.Client(connectID).topople(name,message, msgTime.ToString("t"));

            Clients.Caller.topople(name, message, msgTime.ToString("t"));
            message = String2Json(message).Trim();
            if (string.IsNullOrEmpty(message)) return;         
            AddChatHistory(id, message, msgTime,isLawyer);

        }
        #endregion

        #region 取得歷史紀錄(用不到)

        //public void GetHistory(int id, bool isLawyer, string message, DateTime msgTime)
        //{
        //    string mail = "";
        //    string othermail = "";
        //    string name = "";
        //    var app = _db.appointmentlists.Find(id);
        //    var lawyerinfo = _db.lawyerAccounts.FirstOrDefault(x => x.id == app.lawyerAccountId);
        //    var publicinfo = _db.publicAccounts.FirstOrDefault(x => x.id == app.publicAccountId);
        //    var lawyermail = app.lawyerAccount;
        //    var pbulicmail = app.publicAccount;

        //    if (isLawyer)
        //    {
        //        mail = lawyermail.mail;
        //        othermail = pbulicmail.mail;
        //        name = lawyermail.lastName + lawyermail.firstName;
        //    }
        //    else
        //    {
        //        mail = pbulicmail.mail;
        //        othermail = lawyermail.mail;
        //        name = pbulicmail.lastName + pbulicmail.firstName;

        //    }
        //    var chatHistories = _db.Chatlog.Where(x => (x.SenderId == lawyerinfo.id && x.RecipientId == publicinfo.id) || (x.SenderId == publicinfo.id && x.RecipientId == lawyerinfo.id)).Select(h => new
        //                    {
        //                        h.SenderId,
        //                        h.RecipientId,
        //                        h.Message,
        //                        MsgTime = h.MsgTime.ToString("g")
        //                    });

        //    //通知其他人，有新使用者
        //    if (_connectionIds.ContainsKey(othermail))
        //        Clients.Client(_connectionIds[othermail]).messageHistory(chatHistories);
        //    Clients.Caller.messageHistory(chatHistories);

        //}

        #endregion 取得歷史紀錄

        #region 增加歷史訊息

        public void AddChatHistory(int id, string message, DateTime msgTime, bool isLawyer)
        {
            var app = _db.appointmentlists.FirstOrDefault(x => x.id == id);
            var lawyerinfo = _db.lawyerAccounts.FirstOrDefault(x => x.id == app.lawyerAccountId);
            var publicinfo = _db.publicAccounts.FirstOrDefault(x => x.id == app.publicAccountId);

            if (id == app.id)
            {
                var history = new Chatlog();
                if (isLawyer)
                {
                    history = new Chatlog()
                    {
                        SenderId = lawyerinfo.id,
                        RecipientId = publicinfo.id,
                        Message = message,
                        MsgTime = msgTime,
                        AppointmentId=id
                    };
                }
                else
                {
                    history = new Chatlog()
                    {
                        SenderId = publicinfo.id,
                        RecipientId = lawyerinfo.id,
                        Message = message,
                        MsgTime = msgTime,
                        AppointmentId = id

                    };
                }
                               

                _db.Chatlog.Add(history);
                _db.SaveChanges();
            }
        }

        #endregion 增加歷史訊息
    }


}