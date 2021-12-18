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
    public class HistoriesController : Controller
    {
        private MobilesDBEntities db = new MobilesDBEntities();

        // GET: Histories
        public ActionResult Index()
        {
            string username = User.Identity.Name;
            var histories = db.Histories.Where(d => d.CustomerUserName == username);
            if (username == "admin@admin.com")
            {
                histories = db.Histories.Include(h => h.Customer).Include(h => h.Product);
            }
            return View(histories.ToList());
        }

        // GET: Histories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            History history = db.Histories.Find(id);
            if (history == null)
            {
                return HttpNotFound();
            }
            return View(history);
        }

        // GET: Histories/Create
        [Authorize(Users = "admin@admin.com")]
        public ActionResult Create()
        {
            ViewBag.CustomerUserName = new SelectList(db.Customers, "UserName", "Name");
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name");
            return View();
        }

        // POST: Histories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CustomerUserName,ProductId")] History history)
        {
            if (ModelState.IsValid)
            {
                db.Histories.Add(history);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerUserName = new SelectList(db.Customers, "UserName", "Name", history.CustomerUserName);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", history.ProductId);
            return View(history);
        }

        // GET: Histories/Edit/5
        [Authorize(Users = "admin@admin.com")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            History history = db.Histories.Find(id);
            if (history == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerUserName = new SelectList(db.Customers, "UserName", "Name", history.CustomerUserName);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", history.ProductId);
            return View(history);
        }

        // POST: Histories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CustomerUserName,ProductId")] History history)
        {
            if (ModelState.IsValid)
            {
                db.Entry(history).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerUserName = new SelectList(db.Customers, "UserName", "Name", history.CustomerUserName);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", history.ProductId);
            return View(history);
        }

        // GET: Histories/Delete/5
        [Authorize(Users = "admin@admin.com")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            History history = db.Histories.Find(id);
            if (history == null)
            {
                return HttpNotFound();
            }
            return View(history);
        }

        // POST: Histories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TempData["confirm"] = "History successfully deleted!";
            History history = db.Histories.Find(id);
            db.Histories.Remove(history);
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
