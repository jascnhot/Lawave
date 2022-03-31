using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Lawave.Models;

namespace Lawave.Areas.admin.Controllers
{
    [Authorize]
    public class BacklawyerAccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: admin/BacklawyerAccounts
        public ActionResult Index()
        {
            return View(db.lawyerAccounts.ToList());
        }

        // GET: admin/BacklawyerAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            lawyerAccount lawyerAccount = db.lawyerAccounts.Find(id);
            if (lawyerAccount == null)
            {
                return HttpNotFound();
            }
            return View(lawyerAccount);
        }

        // GET: admin/BacklawyerAccounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: admin/BacklawyerAccounts/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,mail,password,PasswordSalt,isCommunity,googleID,firstName,lastName,shot,verifyPhotolawyer,verifyPhotoFir,verifyPhotoSec,isPublic,phone,saying,introduction,professional,phoneCost,faceCost,isCertification,rule,totalScore,initDate")] lawyerAccount lawyerAccount)
        {
            if (ModelState.IsValid)
            {
                db.lawyerAccounts.Add(lawyerAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lawyerAccount);
        }

        // GET: admin/BacklawyerAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            lawyerAccount lawyerAccount = db.lawyerAccounts.Find(id);
            if (lawyerAccount == null)
            {
                return HttpNotFound();
            }
            return View(lawyerAccount);
        }

        // POST: admin/BacklawyerAccounts/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,mail,password,PasswordSalt,isCommunity,googleID,firstName,lastName,shot,verifyPhotolawyer,verifyPhotoFir,verifyPhotoSec,isPublic,phone,saying,introduction,professional,phoneCost,faceCost,isCertification,rule,totalScore,initDate")] lawyerAccount lawyerAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lawyerAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lawyerAccount);
        }

        // GET: admin/BacklawyerAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            lawyerAccount lawyerAccount = db.lawyerAccounts.Find(id);
            if (lawyerAccount == null)
            {
                return HttpNotFound();
            }
            return View(lawyerAccount);
        }

        // POST: admin/BacklawyerAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            lawyerAccount lawyerAccount = db.lawyerAccounts.Find(id);
            db.lawyerAccounts.Remove(lawyerAccount);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
