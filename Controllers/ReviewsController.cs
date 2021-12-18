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
    [Authorize]
    public class ReviewsController : Controller
    {
        private MobilesDBEntities db = new MobilesDBEntities();

        // GET: Reviews
        public ActionResult Index()
        {
            string username = User.Identity.Name;
            var reviews = db.Reviews.Where(r => r.CustomerUserName == username); ;
            if (username == "admin@admin.com")
            {
                reviews = db.Reviews.Include(r => r.Customer);
            }
            return View(reviews.ToList());
        }

        // GET: Reviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }
        [Authorize]
        // GET: Reviews/Create
        public ActionResult Create()
        {
            ViewBag.CustomerUserName = new SelectList(db.Customers, "UserName", "Name");
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CustomerUserName,Title,Date,Feedback,Reply")] Review review)
        {
            var rev = db.Reviews.Select(i => i);
            rev = rev.Where(i => i.Title == review.Title && i.Feedback == review.Feedback && i.CustomerUserName == review.CustomerUserName);
            if (rev.Count() != 0)
            {
                ViewBag.error = "You already created the same review!";
            }
            else if (ModelState.IsValid)
            {
                db.Reviews.Add(review);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerUserName = new SelectList(db.Customers, "UserName", "Name", review.CustomerUserName);
            return View(review);
        }
        [Authorize(Users = "admin@admin.com")]
        // GET: Reviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerUserName = new SelectList(db.Customers, "UserName", "Name", review.CustomerUserName);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CustomerUserName,Title,Date,Feedback,Reply")] Review review)
        {
            if (ModelState.IsValid)
            {
                TempData["confirm"] = "Review successfully edited!";
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Admin/Index");
            }
            ViewBag.CustomerUserName = new SelectList(db.Customers, "UserName", "Name", review.CustomerUserName);
            return View(review);
        }
        [Authorize(Users = "admin@admin.com")]
        // GET: Reviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TempData["confirm"] = "Review successfully deleted!";
            Review review = db.Reviews.Find(id);
            db.Reviews.Remove(review);
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
