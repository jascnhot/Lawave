using Lawave.Models;
using Lawave.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using WebGrease.Css.Extensions;

namespace Lawave.Controllers
{
    [EnableCors("*", "*", "*")]
    public class membercentreController : ApiController
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        /// <summary>
        /// 會員資訊
        /// </summary>
        public class MemInfo
        {
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string phone { get; set; }
            public LawyerExperience[] experienceList { get; set; }
            public string saying { get; set; }
            public goodAtInfoEnum[] goodAtItem { get; set; }
            public string introduction { get; set; }
            //public string experience { get; set; }
            //public string education { get; set; }
            public LawyerEducation[] education { get; set; }
            //public string bachelor { get; set; }
            public areaEnum[] areaItem { get; set; }
            public string professional { get; set; }
            public string phoneCost { get; set; }
            public string faceCost { get; set; }
        }
        public class LawyerStatus
        {
            public bool isCertification { set; get; }
            public bool isPublic { set; get; }
        }
        public class LawyerReservationItem
        {
            public string time { get; set; }
            public string startTime { get; set; }
            public string endTime { get; set; }
        }
        public class LawyerReservation
        {
            public LawyerReservationItem[] sun { get; set; }
            public LawyerReservationItem[] mon { get; set; }
            public LawyerReservationItem[] tues { get; set; }
            public LawyerReservationItem[] wed { get; set; }
            public LawyerReservationItem[] thur { get; set; }
            public LawyerReservationItem[] fri { get; set; }
            public LawyerReservationItem[] sat { get; set; }

        }
        public class PublicReservation
        {
            public string date { get; set; }
            public string time { get; set; }
            public caseTypeEnum[] caseType { get; set; }
            public string caseInfo { get; set; }
        }
        public class AppointmentStatus
        {
            public appointmentStatus status { get; set; }
        }
        public class ReservationStatus
        {
            public int id { set; get; }
            public bool status { set; get; }
            public string rejection { set; get; }
        }
        public class RemindMail
        {
            public int id { set; get; }
            public string mailBody { set; get; }
        }
      
        /// <summary>
        /// 5.a.會員個人資料-GET
        /// </summary>
        /// <returns></returns>    
        [HttpGet]
        [JwtAuthUtil]
        [Route("mem/Info")]
        // GET: lawyer/Info
        public IHttpActionResult getMemInfo()
        {
            int id = JwtAuthUtil.GetTokenId(Request.Headers.Authorization.Parameter);
            bool isLawyer = JwtAuthUtil.GetTokenisLawyer(Request.Headers.Authorization.Parameter);
            if (isLawyer)
            {
                var user = _db.lawyerAccounts.Find(id);
                List<string> goodAtInfoList = new List<string>();
                foreach (goodAtInfoEnum item in user.lawyerGoodAtType.Select(x => x.goodAtInfoId))
                {
                    goodAtInfoList.Add(item.ToString());
                }
                List<string> areaList = new List<string>();
                foreach (areaEnum item in user.lawyerArea.Select(x => x.area))
                {
                    areaList.Add(item.ToString());
                }
                return Ok(new
                {
                    user.id,
                    user.firstName,
                    user.lastName,
                    user.mail,
                    user.phone,
                    experienceList = user.LawyerExperiences.Select(item => new { item.companyName, item.jobTitle }),
                    user.saying,
                    goodAtItem = goodAtInfoList,
                    user.introduction,
                    education = user.lawyerEducations.Select(item => new { item.schoolName, item.departmentName, item.degree }),
                    areaItem = areaList,
                    user.professional,
                    user.phoneCost,
                    user.faceCost,
                    isPublic = user.isCertification ? user.isPublic : false
                });
            }
            else
            {
                return Ok(_db.publicAccounts.Where(item => item.id == id).Select(user => new
                {
                    user.id,
                    user.firstName,
                    user.lastName,
                    user.mail,
                    user.phone,
                    user.introduction
                }).FirstOrDefault());
            }
        }

        /// <summary>
        /// 5.a.會員個人資料-PUT
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>        
        [HttpPut]
        [JwtAuthUtil]
        [Route("mem/Info")]
        public IHttpActionResult putMemInfo([FromBody] MemInfo info)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            int id = JwtAuthUtil.GetTokenId(Request.Headers.Authorization.Parameter);
            bool isLawyer = JwtAuthUtil.GetTokenisLawyer(Request.Headers.Authorization.Parameter);
            if (isLawyer)
            {
                var user = _db.lawyerAccounts.Find(id);
                user.firstName = info.firstName;
                user.lastName = info.lastName;
                user.phone = info.phone;
                user.saying = info.saying;
                user.introduction = info.introduction;
                user.professional = info.professional;
                user.phoneCost = info.phoneCost;
                user.faceCost = info.faceCost;
                user.isPublic = user.isCertification ? user.isPublic : false;
                _db.Entry(user).State = EntityState.Modified;

                var oldEducation = _db.LawyerEducations.Where(item => item.lawyerAccountId == id);
                foreach (LawyerEducation i in oldEducation)
                {
                    _db.LawyerEducations.Remove(i);
                }
                if (info.education != null)
                    foreach (LawyerEducation i in info.education)
                    {
                        LawyerEducation newEducation = new LawyerEducation();
                        newEducation.lawyerAccountId = id;
                        newEducation.schoolName = i.schoolName;
                        newEducation.departmentName = i.departmentName;
                        newEducation.degree = i.degree;
                        _db.LawyerEducations.Add(newEducation);
                    }

                var oldExperience = _db.LawyerExperiences.Where(item => item.lawyerAccountId == id);
                foreach (LawyerExperience i in oldExperience)
                {
                    _db.LawyerExperiences.Remove(i);
                }
                if (info.education != null)
                    foreach (LawyerExperience i in info.experienceList)
                    {
                        LawyerExperience newExperience = new LawyerExperience();
                        newExperience.lawyerAccountId = id;
                        newExperience.companyName = i.companyName;
                        newExperience.jobTitle = i.jobTitle;
                        _db.LawyerExperiences.Add(newExperience);
                    }

                var oldlawyerType = _db.lawyerGoodAtTypes.Where(item => item.lawyerAccountId == id);
                foreach (lawyerGoodAtType i in oldlawyerType)
                {
                    if (!info.goodAtItem.Contains(i.goodAtInfoId)) _db.lawyerGoodAtTypes.Remove(i);
                }
                if (info.goodAtItem != null)
                    foreach (goodAtInfoEnum i in info.goodAtItem)
                    {
                        if ((_db.lawyerGoodAtTypes.Count(item => item.lawyerAccountId == id & i == item.goodAtInfoId)) == 0)
                        {
                            lawyerGoodAtType newlawyerType = new lawyerGoodAtType();
                            newlawyerType.lawyerAccountId = id;
                            newlawyerType.goodAtInfoId = i;
                            _db.lawyerGoodAtTypes.Add(newlawyerType);
                        }
                    }
                var oldlawyerArea = _db.lawyerAreas.Where(item => item.lawyerAccountId == id);
                foreach (lawyerArea i in oldlawyerArea)
                {
                    if (!info.areaItem.Contains(i.area)) _db.lawyerAreas.Remove(i);
                }
                if (info.areaItem != null)
                    foreach (areaEnum i in info.areaItem)
                    {
                        if ((_db.lawyerAreas.Count(item => item.lawyerAccountId == id & i == item.area)) == 0)
                        {
                            lawyerArea newlawyerArea = new lawyerArea();
                            newlawyerArea.lawyerAccountId = id;
                            newlawyerArea.area = i;
                            _db.lawyerAreas.Add(newlawyerArea);
                        }
                    }
            }
            else
            {
                var user = _db.publicAccounts.Find(id);
                user.firstName = info.firstName;
                user.lastName = info.lastName;
                user.phone = info.phone;
                user.introduction = info.introduction;
                _db.Entry(user).State = EntityState.Modified;
            }
            try
            {
                _db.SaveChanges();
                return Ok(id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// 5.b.會員中心，左邊列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [JwtAuthUtil]
        [Route("mem/lsideInfo")]
        public IHttpActionResult getMemLeftSideInfo()
        {
            var wbpath = "https://" + HttpContext.Current.Request.Url.Host + @"/img/shot/";
            try
            {
                int id = JwtAuthUtil.GetTokenId(Request.Headers.Authorization.Parameter);
                bool isLawyer = JwtAuthUtil.GetTokenisLawyer(Request.Headers.Authorization.Parameter);
                if (isLawyer)
                {
                    var user = _db.lawyerAccounts.Find(id);
                    if (user == null) BadRequest("無此帳號");
                    return Ok(new
                    {
                        user.firstName,
                        user.lastName,
                        shot = user.shot != null && user.shot != "" ? wbpath + user.shot : null,
                        user.isCertification,
                        starAvg = user.appointmentlist.Where(item => item.lawyerStar != null).Average(item => item.lawyerStar)
                    });
                }
                else
                {
                    var user = _db.publicAccounts.Find(id);
                    return Ok(new
                    {
                        user.firstName,
                        user.lastName,
                        shot = user.shot != null && user.shot != "" ? wbpath + user.shot : null,
                        collection = user.publicCollection.Count()
                    });
                }
            }
            catch
            {
                return BadRequest("token錯誤");
            }
        }

        /// <summary>
        /// 2.1律師媒合詳細
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("lawyerlist/Info/{id}")]
        public IHttpActionResult getlawyerInfo(int id)
        {
            var wbpath = "https://" + HttpContext.Current.Request.Url.Host + @"/img/shot/";
            var user = _db.lawyerAccounts.Find(id);
            if (user == null) return BadRequest("無此律師");
            return Ok(new
            {
                user.id,
                shot = user.shot != null && user.shot != "" ? wbpath + user.shot : null,
                user.firstName,
                user.lastName,
                user.mail,
                user.phone,
                office = user.LawyerExperiences.OrderByDescending(item => item.id).FirstOrDefault() != null ?
                    user.LawyerExperiences.OrderByDescending(item => item.id).FirstOrDefault().companyName : null,
                user.saying,
                user.phoneCost,
                user.faceCost,
                goodAtItem = user.lawyerGoodAtType.Select(x => x.goodAtInfoId.ToString()),
                experienceList = user.LawyerExperiences.Select(item => new { item.companyName, item.jobTitle }),
                education = user.lawyerEducations.Select(item => new { item.schoolName, item.departmentName, item.degree }),
                areaItem = user.lawyerArea.Select(x => x.area.ToString()),
                user.introduction,
                user.professional,
            });
        }

        /// <summary>
        /// 2.1.b 律師評價
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("lawyerlist/lawyerReview/{id}")]
        public IHttpActionResult getlawyer_review(int id)
        {
            var wbpath = "https://" + HttpContext.Current.Request.Url.Host + @"/img/shot/";
            var lawercount = _db.appointmentlists.Where(x => x.lawyerAccountId == id && x.lawyerStar != null).Select(x => x.lawyerOpinion).Count();
            var totalScore = _db.appointmentlists.Where(item => item.lawyerStar != null&&item.lawyerAccountId==id).Average(item => item.lawyerStar);
            var lawyerlist = _db.appointmentlists.Where(x => x.lawyerAccountId == id && x.lawyerStar != null).OrderByDescending(x => x.startTime).Select(x => new
            {
                id = x.id,
                name = x.publicAccount.lastName + x.publicAccount.firstName,
                LawaverStar = x.lawyerStar,
                lawaverOpinion = x.lawyerOpinion,
                shot = x.publicAccount.shot,
                caseType = x.caseType,
                evalTime = x.evalTime
            }).AsEnumerable().Select(item => new
            {
                item.id,
                item.name,
                item.LawaverStar,
                item.lawaverOpinion,
                shot = item.shot != null && item.shot != "" ? wbpath + item.shot : null,
                caseType = caseTypDbToListByReservation(item.caseType),
                item.evalTime
            });
            return Ok(new { lawercount, totalScore, lawyerlist });
        }

        /// <summary>
        /// 5.c.會員大頭照-GET
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [JwtAuthUtil]
        [Route("mem/shotPhoto")]
        public IHttpActionResult getShotPhoto()
        {
            var wbpath = "https://" + HttpContext.Current.Request.Url.Host + @"/img/shot/";
            int id = JwtAuthUtil.GetTokenId(Request.Headers.Authorization.Parameter);
            bool isLawyer = JwtAuthUtil.GetTokenisLawyer(Request.Headers.Authorization.Parameter);
            string shot = isLawyer ? _db.lawyerAccounts.Find(id).shot : _db.publicAccounts.Find(id).shot;
            return Ok(new
            {
                shot = shot != null && shot != "" ? $"{wbpath}{shot}" : null,
            });
        }

        /// <summary>
        /// 5.c.會員大頭照-PUT
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [JwtAuthUtil]
        [Route("mem/shotPhoto")]
        public IHttpActionResult putShotPhoto()
        {
            var wbpath = "https://" + HttpContext.Current.Request.Url.Host + @"/img/shot/";
            //獲取引數資訊
            var request = HttpContext.Current.Request;
            if (request.Files.Count <= 0) return NotFound();
            var fileType = request.Files[0]?.FileName.Substring(request.Files[0].FileName.LastIndexOf('.') + 1);
            var newImgName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + fileType;
            var path = HttpContext.Current.Server.MapPath("/") + "img/shot/";
            var imgPath = path + newImgName;
            var directoryInfo = new DirectoryInfo(path);
            if (!directoryInfo.Exists) directoryInfo.Create();

            int id = JwtAuthUtil.GetTokenId(Request.Headers.Authorization.Parameter);
            bool isLawyer = JwtAuthUtil.GetTokenisLawyer(Request.Headers.Authorization.Parameter);
            string shot = isLawyer ? _db.lawyerAccounts.Find(id).shot : _db.publicAccounts.Find(id).shot;
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                if (isLawyer)
                {
                    var user = _db.lawyerAccounts.Find(id);
                    user.shot = newImgName;
                    _db.Entry(user).State = EntityState.Modified;
                }
                else
                {
                    var user = _db.publicAccounts.Find(id);
                    user.shot = newImgName;
                    _db.Entry(user).State = EntityState.Modified;
                }
                request.Files[0]?.SaveAs(imgPath);
                _db.SaveChanges();
                return Ok(wbpath + newImgName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// 5.d.修改律師會員公開欄位
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPut]
        [JwtAuthUtil]
        [Route("mem/publicInfo")]
        public IHttpActionResult putPublic(LawyerStatus status)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            int id = JwtAuthUtil.GetTokenId(Request.Headers.Authorization.Parameter);
            bool isLawyer = JwtAuthUtil.GetTokenisLawyer(Request.Headers.Authorization.Parameter);
            var user = _db.lawyerAccounts.Find(id);
            if (!isLawyer) return BadRequest("民眾無此功能");
            if (!user.isCertification) return BadRequest("未驗證不能修改");

            user.isPublic = status.isPublic;
            _db.Entry(user).State = EntityState.Modified;

            try
            {
                _db.SaveChanges();
                return Ok(user.isPublic);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// 5.e.修改律師會員驗證欄位
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPut]
        [JwtAuthUtil]
        [Route("mem/VeriInfo")]
        public IHttpActionResult putVeriInfoc(LawyerStatus status)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            int id = JwtAuthUtil.GetTokenId(Request.Headers.Authorization.Parameter);
            bool isLawyer = JwtAuthUtil.GetTokenisLawyer(Request.Headers.Authorization.Parameter);
            var user = _db.lawyerAccounts.Find(id);
            if (!isLawyer) return BadRequest("民眾無此功能");

            user.isCertification = status.isCertification;
            _db.Entry(user).State = EntityState.Modified;

            try
            {
                _db.SaveChanges();
                return Ok(user.isCertification);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// 5.f.查詢是否為律師
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [JwtAuthUtil]
        [Route("mem/isLawyer")]
        public IHttpActionResult getIsLawyer()
        {
            bool isLawyer = JwtAuthUtil.GetTokenisLawyer(Request.Headers.Authorization.Parameter);
            return Ok(new { isLawyer = isLawyer });
        }

        /// <summary>
        /// 5.1.會員認證照片-GET
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [JwtAuthUtil]
        [Route("lawyer/veriPhoto")]
        public IHttpActionResult getVeriPhoto()
        {
            var wbpath = "https://" + HttpContext.Current.Request.Url.Host + @"/img/verification/";
            int id = JwtAuthUtil.GetTokenId(Request.Headers.Authorization.Parameter);
            var user = _db.lawyerAccounts.Find(id);
            return Ok(new
            {
                verifyPhotolawyer = user.verifyPhotolawyer !=null ? wbpath + user.verifyPhotolawyer:null,
                verifyPhotoFir = user.verifyPhotoFir!=null? wbpath + user.verifyPhotoFir:null,
                verifyPhotoSec = user.verifyPhotoSec!=null? wbpath + user.verifyPhotoSec:null
            });
        }

        /// <summary>
        /// 5.1.會員認證照片-POST
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [JwtAuthUtil]
        [Route("lawyer/veriPhoto")]
        public IHttpActionResult postVeriPhoto()
        {
            var wbpath = "https://" + HttpContext.Current.Request.Url.Host + @"/img/verification/";
            //獲取引數資訊
            var request = HttpContext.Current.Request;
            if (request.Files.Count <= 0) return BadRequest("無上傳檔案");
            var path = HttpContext.Current.Server.MapPath("/") + "img/verification/";
            var directoryInfo = new DirectoryInfo(path);
            if (!directoryInfo.Exists) directoryInfo.Create();

            int id = JwtAuthUtil.GetTokenId(Request.Headers.Authorization.Parameter);
            var user = _db.lawyerAccounts.Find(id);
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (user == null) return BadRequest("無此項目");
            try
            {
                List<string> result = new List<string>();
                string newImgName = "";
                for (int i = 0; i < 3; i++)
                {
                    if (request.Files[i].FileName.Length == 0)
                    {
                        result.Add("");
                        continue;
                    }
                    var fileType = request.Files[0]?.FileName.Substring(request.Files[0].FileName.LastIndexOf('.') + 1);
                    newImgName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + user.id + i + "." + fileType;
                    var imgPath = path + newImgName;
                    request.Files[i]?.SaveAs(imgPath);
                    if (i == 0) user.verifyPhotolawyer = newImgName;
                    else if (i == 1) user.verifyPhotoFir = newImgName;
                    else user.verifyPhotoSec = newImgName;
                    result.Add(wbpath + newImgName);
                }
                user.isCertification = (user.verifyPhotolawyer != null && user.verifyPhotoFir != null && user.verifyPhotoSec != null) ? true : false;
                _db.Entry(user).State = EntityState.Modified;
                _db.SaveChanges();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// 5-4-1.律師設定預約時間顯示
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [JwtAuthUtil]
        [Route("mem/lawyerReservationSet")]
        public IHttpActionResult getLawyerReservationSet()
        {

            int id = JwtAuthUtil.GetTokenId(Request.Headers.Authorization.Parameter);
            bool isLawyer = JwtAuthUtil.GetTokenisLawyer(Request.Headers.Authorization.Parameter);
            var user = _db.lawyerAccounts.Find(id);
            LawyerReservation result = new LawyerReservation();
            if (user == null) return BadRequest("無此帳號");
            if (!isLawyer) return BadRequest("民眾無此服務");

            return Ok(new
            {
                sun = calculateTime(user.rule.sum),
                mon = calculateTime(user.rule.mon),
                tues = calculateTime(user.rule.tues),
                wed = calculateTime(user.rule.wed),
                thur = calculateTime(user.rule.thur),
                fri = calculateTime(user.rule.fri),
                sat = calculateTime(user.rule.sat),
            });


        }

        /// <summary>
        /// 5-4-1.律師設定預約時間設定
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        [HttpPut]
        [JwtAuthUtil]
        [Route("mem/lawyerReservationSet")]
        public IHttpActionResult putLawyerReservationSet([FromBody] LawyerReservation rule)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            int id = JwtAuthUtil.GetTokenId(Request.Headers.Authorization.Parameter);
            bool isLawyer = JwtAuthUtil.GetTokenisLawyer(Request.Headers.Authorization.Parameter);
            var user = _db.lawyerAccounts.Find(id);
            if (!isLawyer) return BadRequest("民眾無此功能");

            user.rule.sum = setLawyerTime(rule.sun);
            user.rule.mon = setLawyerTime(rule.mon);
            user.rule.tues = setLawyerTime(rule.tues);
            user.rule.wed = setLawyerTime(rule.wed);
            user.rule.thur = setLawyerTime(rule.thur);
            user.rule.fri = setLawyerTime(rule.fri);
            user.rule.sat = setLawyerTime(rule.sat);

            _db.Entry(user).State = EntityState.Modified;

            try
            {
                _db.SaveChanges();
                return Ok($"律師id={id},設定成功");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// 2-2.民眾設定預約時間頁面顯示
        /// </summary>
        /// <param name="lawyerId"></param>
        /// <returns></returns>
        [HttpGet]
        //[JwtAuthUtil]
        [Route("mem/publicReservationSet/{lawyerId}")]
        public IHttpActionResult getLawyerReservationSet(int lawyerId)
        {
            //if (!ModelState.IsValid) return BadRequest(ModelState);
            //int publicId = JwtAuthUtil.GetTokenId(Request.Headers.Authorization.Parameter);
            //bool isLawyer = JwtAuthUtil.GetTokenisLawyer(Request.Headers.Authorization.Parameter);
            var lawyerUser = _db.lawyerAccounts.Find(lawyerId);
            //var publicUser = _db.publicAccounts.Find(publicId);
            //if (isLawyer) return BadRequest("律師無此功能");
            if (lawyerUser == null) return BadRequest("無此律師");
            //if (publicUser == null) return BadRequest("無此民眾");
            DateTime now = DateTime.Now;
            DateTime endDate = now.AddDays(30);
            List<PublicReservation> result = new List<PublicReservation>();
            while (now < endDate)
            {
                switch (now.DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        result.AddRange(calculateTimeDbToPublic(lawyerUser, lawyerUser.rule.sum, now.ToString("yyyy/MM/dd")));
                        break;
                    case DayOfWeek.Monday:
                        result.AddRange(calculateTimeDbToPublic(lawyerUser, lawyerUser.rule.mon, now.ToString("yyyy/MM/dd")));
                        break;
                    case DayOfWeek.Tuesday:
                        result.AddRange(calculateTimeDbToPublic(lawyerUser, lawyerUser.rule.tues, now.ToString("yyyy/MM/dd")));
                        break;
                    case DayOfWeek.Wednesday:
                        result.AddRange(calculateTimeDbToPublic(lawyerUser, lawyerUser.rule.wed, now.ToString("yyyy/MM/dd")));
                        break;
                    case DayOfWeek.Thursday:
                        result.AddRange(calculateTimeDbToPublic(lawyerUser, lawyerUser.rule.thur, now.ToString("yyyy/MM/dd")));
                        break;
                    case DayOfWeek.Friday:
                        result.AddRange(calculateTimeDbToPublic(lawyerUser, lawyerUser.rule.fri, now.ToString("yyyy/MM/dd")));
                        break;
                    case DayOfWeek.Saturday:
                        result.AddRange(calculateTimeDbToPublic(lawyerUser, lawyerUser.rule.sat, now.ToString("yyyy/MM/dd")));
                        break;
                }
                now = now.AddDays(1);
            }
            return Ok(result.Select(item => new { item.date, item.time }));
        }

        /// <summary>
        /// 2-2.民眾設定預約時間頁面設定
        /// </summary>
        /// <param name="lawyerId">律師Id</param>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [JwtAuthUtil]
        [Route("mem/publicReservationSet/{lawyerId}")]
        public IHttpActionResult putLawyerReservationSet(int lawyerId, [FromBody] PublicReservation item)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            int publicId = JwtAuthUtil.GetTokenId(Request.Headers.Authorization.Parameter);
            bool isLawyer = JwtAuthUtil.GetTokenisLawyer(Request.Headers.Authorization.Parameter);
            if (isLawyer) return BadRequest("律師無此功能");

            DateTime dateTime = Convert.ToDateTime($"{item.date} {item.time}");
            if (_db.appointmentlists.Where(user => user.lawyerAccountId == lawyerId & user.startTime == dateTime).Count() > 0) return BadRequest("律師此時段被預約");
            appointmentlist newItem = new appointmentlist();
            newItem.publicAccountId = publicId;
            newItem.lawyerAccountId = lawyerId;
            if (_db.lawyerBlacklists.Where(list => list.lawyerAccountId == lawyerId && list.publicAccountId == publicId).Count() > 0)
            {
                newItem.status = appointmentStatus.reject;
                newItem.rejection = "該律師無法受理此次預約";
                newItem.rejectionTime = DateTime.Now;
            }
            else
            {
                newItem.status = appointmentStatus.audit;
            }
            newItem.startTime = dateTime;
            newItem.caseType = "";
            newItem.caseInfo = item.caseInfo;
            foreach (int i in item.caseType) newItem.caseType += i + ",";
            newItem.InitDate = DateTime.Now;

            _db.appointmentlists.Add(newItem);

            try
            {
                _db.SaveChanges();
                return Ok($"民眾id={publicId},預約律師id={lawyerId},時間為{newItem.startTime.ToString("yyyy/MM/dd HH:00")},狀態為{newItem.status.ToString()}");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// 5-4-2.a.審核中/已預約/已完成頁面
        /// </summary>
        /// <param name="status"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        [JwtAuthUtil]
        [Route("mem/reservation/{status}/{page:int?}")]
        public IHttpActionResult getReservation(appointmentStatus status, int page = 1)
        {
            changeReservationStatus();
            var wbpath = "https://" + HttpContext.Current.Request.Url.Host + @"/img/shot/";
            int count = 5;//顯示5筆

            int id = JwtAuthUtil.GetTokenId(Request.Headers.Authorization.Parameter);
            bool isLawyer = JwtAuthUtil.GetTokenisLawyer(Request.Headers.Authorization.Parameter);
            if (isLawyer)
            {
                var datalist = _db.appointmentlists.Where(item => item.lawyerAccount.id == id & item.status == status)
                    .Select(item => new
                    {
                        id = item.id,
                        lastName = item.publicAccount.lastName,
                        firstName = item.publicAccount.firstName,
                        shot = item.publicAccount.shot,
                        caseInfo = item.caseInfo,
                        caseType = item.caseType,
                        startTime = item.startTime
                    }).AsEnumerable();
                if (status == appointmentStatus.audit || status == appointmentStatus.booked)
                    datalist = datalist.Where(x => x.startTime.AddHours(1) >= DateTime.Now);
                var data = datalist.Select(item => new
                {
                    item.id,
                    item.lastName,
                    item.firstName,
                    shot = item.shot != null && item.shot != "" ? wbpath + item.shot : null,
                    item.caseInfo,
                    caseType = caseTypDbToListByReservation(item.caseType),
                    item.startTime
                }).OrderBy(item => item.startTime).Skip((page - 1) * count).Take(count);
                double maxpage = datalist.Count() != 0 ? Math.Ceiling(datalist.Count() / (double)count) : 0;
                return Ok(new { maxpage = (int)maxpage, data = data });
            }
            else
            {
                var datalist = _db.appointmentlists.Where(item => item.publicAccountId == id)
                    .Select(item => new
                    {
                        id = item.id,
                        lastName = item.lawyerAccount.lastName,
                        firstName = item.lawyerAccount.firstName,
                        shot = item.lawyerAccount.shot,
                        caseInfo = item.caseInfo,
                        caseType = item.caseType,
                        startTime = item.startTime,
                        status = item.status,
                        evalTime=item.evalTime
                    }).AsEnumerable();
                if (status == appointmentStatus.completed)
                {
                    datalist = datalist.Where(item => item.status == appointmentStatus.completed || item.status == appointmentStatus.reject);
                }
                else
                {
                    datalist = datalist.Where(item => item.status == status);
                    if (status == appointmentStatus.audit || status == appointmentStatus.booked)
                        datalist = datalist.Where(x => x.startTime.AddHours(1) >= DateTime.Now);
                }
                var data = datalist.Select(item => new
                {
                    item.id,
                    item.lastName,
                    item.firstName,
                    shot = item.shot != null && item.shot != "" ? wbpath + item.shot : null,
                    item.caseInfo,
                    caseType = caseTypDbToListByReservation(item.caseType),
                    item.startTime,
                    status = item.status.ToString(),
                    isEvaluation= status== appointmentStatus.completed?item.evalTime!=null?true:false:false
                }).OrderBy(item => item.startTime).Skip((page - 1) * count).Take(count);
                double maxpage = datalist.Count() != 0 ? Math.Ceiling(datalist.Count() / (double)count) : 0;
                return Ok(new { maxpage = (int)maxpage, data = data });
            }
        }

        /// <summary>
        /// 5-4-2.b.修改審核結果
        /// </summary>
        /// <param name="reservationStatus"></param>
        /// <returns></returns>
        [HttpPut]
        [JwtAuthUtil]
        [Route("mem/reservationAssent")]
        public IHttpActionResult putReservationAssent([FromBody] ReservationStatus reservationStatus)
        {
            int lawyerId = JwtAuthUtil.GetTokenId(Request.Headers.Authorization.Parameter);
            bool isLawyer = JwtAuthUtil.GetTokenisLawyer(Request.Headers.Authorization.Parameter);
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!isLawyer) return BadRequest("民眾無此功能");
            var lawyerUser = _db.lawyerAccounts.Find(lawyerId);
            if (lawyerUser == null) return BadRequest("無此律師");
            var reservation = lawyerUser.appointmentlist
                .Where(item => item.id == reservationStatus.id & item.status == appointmentStatus.audit).FirstOrDefault();
            if (reservation == null) return BadRequest("無此審核中的預約");
            if (reservationStatus.status)
            {
                reservation.status = appointmentStatus.booked;
            }
            else
            {
                reservation.status = appointmentStatus.reject;
                reservation.rejection = reservationStatus.rejection;
                reservation.rejectionTime = DateTime.Now;
            }
            try
            {
                _db.Entry(reservation).State = EntityState.Modified;
                _db.SaveChanges();
                return Ok($"stutas已切換為{reservation.status.ToString()}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// 5-4-3.寄送提醒信件
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        [HttpPost]
        [JwtAuthUtil]
        [Route("lawyer/remindMail")]
        public IHttpActionResult postRemindMail([FromBody] RemindMail mail)
        {
            int lawyerId = JwtAuthUtil.GetTokenId(Request.Headers.Authorization.Parameter);
            bool isLawyer = JwtAuthUtil.GetTokenisLawyer(Request.Headers.Authorization.Parameter);
            if (!isLawyer) return BadRequest("民眾無此功能");
            var lawyerUser = _db.lawyerAccounts.Find(lawyerId);
            if (lawyerUser == null) return BadRequest("無此律師");
            var reservation = lawyerUser.appointmentlist
                .Where(item => item.id == mail.id & item.status == appointmentStatus.booked).FirstOrDefault();
            if (reservation == null) return BadRequest("無此預約");
            var publicUser = reservation.publicAccount;
            string title = "法學電波提醒您，有個線上諮詢預約";
            VeriCode.SendMail(publicUser.mail, publicUser.lastName + publicUser.firstName, title, mail.mailBody);
            return Ok("已發送");
        }

        /// <summary>
        /// 5-4-4.b.預約單評價
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("lawyer/reservationReview/{id:int?}")]
        public IHttpActionResult getReservationReview(int id=0)
        {
            var wbpath = "https://" + HttpContext.Current.Request.Url.Host + @"/img/shot/";
            int lawyerId = JwtAuthUtil.GetTokenId(Request.Headers.Authorization.Parameter);
            bool isLawyer = JwtAuthUtil.GetTokenisLawyer(Request.Headers.Authorization.Parameter);
            if (!isLawyer) return BadRequest("民眾無此功能");
            var lawyerUser = _db.lawyerAccounts.Find(lawyerId);
            if (lawyerUser == null) return BadRequest("無此律師");
            appointmentlist review = new appointmentlist();
            if (id == 0)
            {
                review = lawyerUser.appointmentlist.Where(item => item.status == appointmentStatus.completed & item.evalTime!=null).OrderByDescending(x => x.evalTime).FirstOrDefault();
                
            }
            else
            {
                review = lawyerUser.appointmentlist.Where(item => item.id == id & item.status == appointmentStatus.completed & item.evalTime!=null).FirstOrDefault();
                
            }
            if (review == null) return Ok(review);

            var reviewitem = new {
                shot = review.publicAccount.shot != null && review.publicAccount.shot != "" ? wbpath + review.publicAccount.shot : null,
                name = review.publicAccount.lastName + review.publicAccount.firstName,
                caseType = caseTypDbToListByReservation(review.caseType),
                review.caseInfo,
                review.lawyerStar,
                review.lawyerOpinion,
                time = review.evalTime
            };
            if (id == 0)
            {
                return Ok(reviewitem);
            }
            else
            {
                return Ok(new
                {
                    review.lawyerStar,
                    review.lawyerOpinion,
                    time = review.evalTime
                });
            }
            
        }

        /// <summary>
        /// 5-4-4.c.封鎖功能(新增
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <returns></returns>
        [HttpPost]
        [JwtAuthUtil]
        [Route("lawyer/blockReservation/add/{appointmentId}")]
        public IHttpActionResult postBlockStatusAdd(int appointmentId)
        {
            int lawyerId = JwtAuthUtil.GetTokenId(Request.Headers.Authorization.Parameter);
            bool isLawyer = JwtAuthUtil.GetTokenisLawyer(Request.Headers.Authorization.Parameter);
            if (!isLawyer) return BadRequest("民眾無此功能");
            var lawyerUser = _db.lawyerAccounts.Find(lawyerId);
            if (lawyerUser == null) return BadRequest("無此律師");
            int publicId = (int)lawyerUser.appointmentlist.Where(item => item.id == appointmentId).FirstOrDefault().publicAccountId;
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var status = _db.lawyerBlacklists.Where(x => x.publicAccountId == publicId & x.lawyerAccountId == lawyerId).FirstOrDefault();

            lawyerBlacklist lawyerBlack = new lawyerBlacklist
            {
                lawyerAccountId = lawyerId,
                publicAccountId = publicId,
            };
            //如果null就新增資料
            if (status == null)
            {
                var items = lawyerUser.appointmentlist.Where(x => x.publicAccountId == publicId & x.status == appointmentStatus.audit);
                foreach (appointmentlist item in items)
                {
                    item.status = appointmentStatus.reject;
                    item.rejection = "該律師無法受理此次預約";
                    item.rejectionTime = DateTime.Now;
                    _db.Entry(item).State = EntityState.Modified;
                }
                _db.lawyerBlacklists.Add(lawyerBlack);
                _db.SaveChanges();
            }


            return Ok(status == null ? $"律師id={lawyerId},封鎖民眾id={publicId},封鎖成功" : "重複封鎖");
        }

        /// <summary>
        /// 5-4-4.d.封鎖功能(新增
        /// </summary>
        /// <param name="publicId"></param>
        /// <returns></returns>
        [HttpPost]
        [JwtAuthUtil]
        [Route("lawyer/blockReservation/del/{publicId}")]
        public IHttpActionResult postBlockStatusDel(int publicId)
        {
            int lawyerId = JwtAuthUtil.GetTokenId(Request.Headers.Authorization.Parameter);
            bool isLawyer = JwtAuthUtil.GetTokenisLawyer(Request.Headers.Authorization.Parameter);
            if (!isLawyer) return BadRequest("民眾無此功能");
            var lawyerUser = _db.lawyerAccounts.Find(lawyerId);
            if (lawyerUser == null) return BadRequest("無此律師");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var status = lawyerUser.lawyerBlacklist.Where(x => x.publicAccountId == publicId).FirstOrDefault();

            lawyerBlacklist lawyerBlack = new lawyerBlacklist
            {
                lawyerAccountId = lawyerId,
                publicAccountId = publicId,
            };
            //如果null就新增資料
            if (status != null)
            {
                _db.lawyerBlacklists.Remove(status);
            }
            _db.SaveChanges();

            return Ok(status != null ? $"律師id={lawyerId},封鎖民眾id={publicId},解除封鎖" : "此民眾未被封鎖");
        }

        /// <summary>
        /// 5-5-5.封鎖名單頁面
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        [JwtAuthUtil]
        [Route("lawyer/blackList/{page:int?}")]
        public IHttpActionResult getBlackList(int page = 1)
        {
            var wbpath = "https://" + HttpContext.Current.Request.Url.Host + @"/img/shot/";
            int count = 5;//顯示5筆

            int lawyerId = JwtAuthUtil.GetTokenId(Request.Headers.Authorization.Parameter);
            bool isLawyer = JwtAuthUtil.GetTokenisLawyer(Request.Headers.Authorization.Parameter);
            if (!isLawyer) return BadRequest("民眾無此功能");
            var lawyerUser = _db.lawyerAccounts.Find(lawyerId);
            var datalist = lawyerUser.lawyerBlacklist.Join(
                _db.publicAccounts,
                blacklists => blacklists.publicAccountId,
                publiclists => publiclists.id,
                (blacklists, publiclists) => new
                {
                    id = publiclists.id,
                    lastName = publiclists.lastName,
                    firstName = publiclists.firstName,
                    phone = publiclists.phone,
                    mail = publiclists.mail,
                    shot = publiclists.shot != null && publiclists.shot != "" ? wbpath + publiclists.shot : null,
                });
            var data = datalist.Skip((page - 1) * count).Take(count);

            double maxpage = datalist.Count() != 0 ? Math.Ceiling(datalist.Count() / (double)count) : 0;
            return Ok(new { maxpage = (int)maxpage, data = data });
        }

        /// <summary>
        /// 6-3.拒絕內容顯示
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <returns></returns>
        [HttpGet]
        [JwtAuthUtil]
        [Route("public/rejection/{appointmentId}")]
        public IHttpActionResult getRejection(int appointmentId)
        {
            int publicId = JwtAuthUtil.GetTokenId(Request.Headers.Authorization.Parameter);
            bool isLawyer = JwtAuthUtil.GetTokenisLawyer(Request.Headers.Authorization.Parameter);
            if (isLawyer) return BadRequest("律師無此功能");
            var user = _db.publicAccounts.Find(publicId);
            if (user == null) return BadRequest("無此民眾");

            return Ok(user.appointmentlist.Where(item => item.id == appointmentId & item.status == appointmentStatus.reject).Select(item => new
            {
                item.rejection,
                item.rejectionTime
            }).FirstOrDefault());
        }

        /// <summary>
        /// 6-3-1.刪除預約
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <returns></returns>
        [HttpDelete]
        [JwtAuthUtil]
        [Route("public/delReservation/{appointmentId}")]
        public IHttpActionResult delReservation(int appointmentId)
        {
            int publicId = JwtAuthUtil.GetTokenId(Request.Headers.Authorization.Parameter);
            bool isLawyer = JwtAuthUtil.GetTokenisLawyer(Request.Headers.Authorization.Parameter);
            if (isLawyer) return BadRequest("律師無此功能");
            var user = _db.publicAccounts.Find(publicId);
            if (user == null) return BadRequest("無此民眾");

            var item = user.appointmentlist.FirstOrDefault(x => x.id == appointmentId);

            try
            {
                _db.appointmentlists.Remove(item);
                _db.SaveChanges();
                return Ok(item.id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// 6-4.我的收藏
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        [JwtAuthUtil]
        [Route("public/myCollection/{page:int?}")]
        public IHttpActionResult getMyCollection(int page = 1)
        {
            int count = 5;//顯示5筆

            int id = JwtAuthUtil.GetTokenId(Request.Headers.Authorization.Parameter);
            bool isLawyer = JwtAuthUtil.GetTokenisLawyer(Request.Headers.Authorization.Parameter);
            if (isLawyer) return BadRequest("律師無此功能");
            var user = _db.publicAccounts.Find(id);
            if (user == null) return BadRequest("無此民眾");

            var datalist = user.publicCollection.Select(item => new
            {
                id = item.lawyerAccountId,
                lastName = item.lawyerAccount.lastName,
                firstName = item.lawyerAccount.firstName,
                office = item.lawyerAccount.LawyerExperiences.OrderByDescending(x => x.id).FirstOrDefault() != null ?
                        item.lawyerAccount.LawyerExperiences.OrderByDescending(x => x.id).FirstOrDefault().companyName : null,
                phone = item.lawyerAccount.phone,
                mail = item.lawyerAccount.mail,
                goodAtItem = item.lawyerAccount.lawyerGoodAtType.Select(x => x.goodAtInfoId.ToString()),
            });
            var data = datalist.OrderBy(item => item.id).Skip((page - 1) * count).Take(count);
            double maxpage = datalist.Count() != 0 ? Math.Ceiling(datalist.Count() / (double)count) : 0;
            return Ok(new { maxpage = (int)maxpage, data = data });
        }




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private List<LawyerReservationItem> calculateTime(string rule)
        {
            List<LawyerReservationItem> result = new List<LawyerReservationItem>();
            int startTime = -1;
            int endTime = -1;
            const int amEnd = 11;
            const int pmEnd = 11;

            if (rule == null) return result;
            if (rule.Length != 24) return result;

            for (int i = 0; i < 23; i++)
            {
                if (rule[i] == '1')
                {
                    if (startTime == -1) startTime = i;
                    if (i == amEnd || i == pmEnd)
                    {
                        endTime = i;
                        LawyerReservationItem item = new LawyerReservationItem();
                        item.time = i <= 11 ? "上午" : "下午";
                        item.startTime = startTime + ":00";
                        item.endTime = endTime + 1 + ":00"; ;
                        result.Add(item);
                        startTime = -1;
                        endTime = -1;
                    }
                }
                else
                {
                    if (startTime != -1)
                    {
                        endTime = i - 1;
                        LawyerReservationItem item = new LawyerReservationItem();
                        item.time = i <= 11 ? "上午" : "下午";
                        item.startTime = startTime + ":00";
                        item.endTime = endTime + 1 + ":00";
                        result.Add(item);
                        startTime = -1;
                        endTime = -1;
                    }
                }
            }

            return result;
        }
        private string setLawyerTime(LawyerReservationItem[] items)
        {
            char[] rule = new char[24];

            for (int i = 0; i < 24; i++)
            {
                rule[i] = '0';
            }

            foreach (LawyerReservationItem item in items)
            {
                if (item.startTime == "" || item.endTime == "" || item.startTime == item.endTime ||item.startTime== "－－") continue;
                int startTime = Convert.ToInt32(Convert.ToDateTime(item.startTime).Hour);
                int endTime = Convert.ToInt32(Convert.ToDateTime(item.endTime).Hour);
                for (int i = startTime; i < endTime; i++)
                {
                    rule[i] = '1';
                }
            }
            return new string(rule);
        }

        private List<PublicReservation> calculateTimeDbToPublic(lawyerAccount user, string rule, string date)
        {
            List<PublicReservation> result = new List<PublicReservation>();
            if (rule == null) return result;
            if (rule.Length != 24) return result;

            for (int i = 0; i < rule.Length; i++)
            {
                PublicReservation item = new PublicReservation();
                if (rule[i] == '1')
                {
                    item.date = date;
                    item.time = $"{i}:00";
                    if (Convert.ToDateTime($"{item.date} {item.time}") < DateTime.Now) continue;
                    if (user.appointmentlist.Where(x => x.startTime == Convert.ToDateTime($"{item.date} {item.time}") & x.status != appointmentStatus.reject).Count() > 0) continue;
                    result.Add(item);
                }
            }
            return result;
        }
        private List<string> caseTypDbToListByReservation(string db)
        {
            List<string> result = new List<string>();
            if (db == "") return result;
            db = db.Trim(',');
            string[] item = db.Split(',');
            for (int i = 0; i < item.Length; i++)
            {
                result.Add(((caseTypeEnum)Convert.ToInt32(item[i])).ToString());
            }
            return result;
        }
        private void changeReservationStatus()
        {
            DateTime now = DateTime.Now;
            var list = _db.appointmentlists.Where(x =>
                x.status == appointmentStatus.booked).AsEnumerable();
            list.Where(x => x.startTime.AddHours(1) < DateTime.Now).ForEach(x => x.status = appointmentStatus.completed);

            _db.SaveChanges();
        }
    }
}
