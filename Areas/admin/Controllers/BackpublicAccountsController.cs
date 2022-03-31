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
    public class BackpublicAccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: admin/BackpublicAccounts
        public ActionResult Index()
        {
            return View(db.publicAccounts.ToList());
        }

        // GET: admin/BackpublicAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            publicAccount publicAccount = db.publicAccounts.Find(id);
            if (publicAccount == null)
            {
                return HttpNotFound();
            }
            return View(publicAccount);
        }

        // GET: admin/BackpublicAccounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: admin/BackpublicAccounts/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,mail,password,PasswordSalt,isCommunity,googleID,firstName,lastName,shot,phone,introduction,initDate")] publicAccount publicAccount)
        {
            if (ModelState.IsValid)
            {
                db.publicAccounts.Add(publicAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(publicAccount);
        }

        // GET: admin/BackpublicAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            publicAccount publicAccount = db.publicAccounts.Find(id);
            if (publicAccount == null)
            {
                return HttpNotFound();
            }
            return View(publicAccount);
        }

        // POST: admin/BackpublicAccounts/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,mail,password,PasswordSalt,isCommunity,googleID,firstName,lastName,shot,phone,introduction,initDate")] publicAccount publicAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(publicAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(publicAccount);
        }

        // GET: admin/BackpublicAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            publicAccount publicAccount = db.publicAccounts.Find(id);
            if (publicAccount == null)
            {
                return HttpNotFound();
            }
            return View(publicAccount);
        }

        // POST: admin/BackpublicAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            publicAccount publicAccount = db.publicAccounts.Find(id);
            db.publicAccounts.Remove(publicAccount);
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
