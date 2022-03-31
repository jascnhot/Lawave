using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using MailKit.Net.Smtp;
using MimeKit;

namespace Lawave.Security
{
    public class VeriCode
    {
        public static string GetVeriCode(int len)
        {
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var Charsarr = new char[len];
            var random = new Random();

            for (int i = 0; i < len; i++)
            {
                Charsarr[i] = characters[random.Next(characters.Length)];
            }

            var resultString = new String(Charsarr);
            return (resultString);
        }

        public static object VericodeSMSSend(string phone, string veriCode)
        {
            string snsAccount = WebConfigurationManager.AppSettings["snsAccount"];
            string snsPassword = WebConfigurationManager.AppSettings["snsPassword"];
            string smbody = HttpUtility.UrlEncode($"感謝您註冊\"法律電波Lawave\"會員，手機驗證碼為{veriCode}，請於30分鐘內完成認證。",
                                                    Encoding.GetEncoding("Big5"));

            WebClient client = new WebClient();
            var url = string.Format("http://202.39.48.216/kotsmsapi-1.php?"+
                                   $"username={snsAccount}" + 
                                   $"&password={snsPassword}"+
                                   $"&dstaddr={phone}"+
                                   $"&smbody={smbody}")+
                                   $"&response=https://localhost:44311/snsResponse/response.php";
            var result = client.DownloadString(url);
            return result;
        }
        public static string SendMailVeriCode( string toAddress, string veriCode)
        {
            string errMsg = "";
            string fromAddress = WebConfigurationManager.AppSettings["gmailAccount"];
            string fromName = "法律電波";
            string title = "法律電波 會員系統通知-忘記密碼";
            string toName = "法律電波會員";
            string serverAddress = "smtp.gmail.com";
            int port = 587;
            string mailAccount = WebConfigurationManager.AppSettings["gmailAccount"];
            string mailPassword = WebConfigurationManager.AppSettings["gmailPassword"];


            //建立建立郵件
            MimeMessage mail = new MimeMessage();
            // 添加寄件者
            mail.From.Add(new MailboxAddress(fromName, fromAddress));
            // 添加收件者
            mail.To.Add(new MailboxAddress(toName, toAddress.Trim()));
            // 設定郵件標題
            mail.Subject = title;
            //使用 BodyBuilder 建立郵件內容
            BodyBuilder body = new BodyBuilder();
            //設定文字內容
            body.TextBody = $"您的網路驗證碼為\n{veriCode}\n請於30分鐘內完成認證。";
            // 設定 HTML 內容
            //body.HtmlBody = "<p> HTML 內容 </p>";
            // 設定附件
            //body.Attachments.Add("檔案路徑");
            // 設定郵件內容
            mail.Body = body.ToMessageBody();

            using (var client = new SmtpClient())
            {
                //client.CheckCertificateRevocation = false;
                //SSL加密
                bool useSsl = false;
                //連線Mail Server
                client.Connect(serverAddress, port, useSsl);
                //帳號驗證
                client.Authenticate(mailAccount, mailPassword);
                //送出信件
                client.Send(mail);
                //中斷連線
                client.Disconnect(true);
            }
            return errMsg;
        }
        public static string SendMail(string toAddress, string toName, string title, string mailbody)
        {
            string errMsg = "";
            string fromAddress = WebConfigurationManager.AppSettings["gmailAccount"];
            string fromName = "法律電波";
            string serverAddress = "smtp.gmail.com";
            int port = 587;
            string mailAccount = WebConfigurationManager.AppSettings["gmailAccount"];
            string mailPassword = WebConfigurationManager.AppSettings["gmailPassword"];

            //建立建立郵件
            MimeMessage mail = new MimeMessage();
            // 添加寄件者
            mail.From.Add(new MailboxAddress(fromName, fromAddress));
            // 添加收件者
            mail.To.Add(new MailboxAddress(toName, toAddress.Trim()));
            // 設定郵件標題
            mail.Subject = title;
            //使用 BodyBuilder 建立郵件內容
            BodyBuilder body = new BodyBuilder();
            //設定文字內容
            body.TextBody = mailbody;
            // 設定 HTML 內容
            //body.HtmlBody = "<p> HTML 內容 </p>";
            // 設定附件
            //body.Attachments.Add("檔案路徑");
            // 設定郵件內容
            mail.Body = body.ToMessageBody();

            using (var client = new SmtpClient())
            {
                //client.CheckCertificateRevocation = false;
                //SSL加密
                bool useSsl = false;
                //連線Mail Server
                client.Connect(serverAddress, port, useSsl);
                //帳號驗證
                client.Authenticate(mailAccount, mailPassword);
                //送出信件
                client.Send(mail);
                //中斷連線
                client.Disconnect(true);
            }
            return errMsg;
        }
    }
}