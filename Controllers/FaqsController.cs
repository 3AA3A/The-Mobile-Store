using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MobilesProject.Models;

namespace MobilesProject.Controllers
{

    public class FaqsController : Controller
    {
        private MobilesDBEntities db = new MobilesDBEntities();

        // GET: Faqs
        public ActionResult Index()
        {
            return View(db.Faqs.ToList());
        }

        // GET: Faqs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Faq faq = db.Faqs.Find(id);
            if (faq == null)
            {
                return HttpNotFound();
            }
            return View(faq);
        }
        [Authorize(Users = "admin@admin.com")]
        // GET: Faqs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Faqs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Category,Question,Answer")] Faq faq)
        {
            var fq = db.Faqs.Select(i => i);
            fq = fq.Where(i => i.Question == faq.Question);
            if (fq.Count() != 0)
            {
                ViewBag.error = "Duplicate Question";
            }
            else if (ModelState.IsValid)
            {
                db.Faqs.Add(faq);
                db.SaveChanges();
                return RedirectToAction("../Admin/Index");
            }

            return View(faq);
        }
        [Authorize(Users = "admin@admin.com")]
        // GET: Faqs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Faq faq = db.Faqs.Find(id);
            if (faq == null)
            {
                return HttpNotFound();
            }
            return View(faq);
        }

        // POST: Faqs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Category,Question,Answer")] Faq faq)
        {
            if (ModelState.IsValid)
            {
                TempData["confirm"] = "FAQ successfully edited!";
                db.Entry(faq).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Admin/Index");
            }
            return View(faq);
        }
        [Authorize(Users = "admin@admin.com")]
        // GET: Faqs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Faq faq = db.Faqs.Find(id);
            if (faq == null)
            {
                return HttpNotFound();
            }
            return View(faq);
        }

        // POST: Faqs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TempData["confirm"] = "FAQ successfully deleted!";
            Faq faq = db.Faqs.Find(id);
            db.Faqs.Remove(faq);
            db.SaveChanges();
            return RedirectToAction("../Admin/Index");
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
