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
    public class WishlistsController : Controller
    {
        private MobilesDBEntities db = new MobilesDBEntities();

        // GET: Wishlists
        public ActionResult Index()
        {
            string username = User.Identity.Name;
            var wishlists = db.Wishlists.Where(d => d.CustomerUserName == username);
            if (username == "admin@admin.com")
            {
                wishlists = db.Wishlists.Include(w => w.Customer).Include(w => w.Product);
            }
            return View(wishlists.ToList());
        }

        // GET: Wishlists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wishlist wishlist = db.Wishlists.Find(id);
            if (wishlist == null)
            {
                return HttpNotFound();
            }
            return View(wishlist);
        }

        // GET: Wishlists/Create
        public ActionResult Create()
        {
            ViewBag.CustomerUserName = new SelectList(db.Customers, "UserName", "Name");
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name");
            return View();
        }

        // POST: Wishlists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CustomerUserName,ProductId")] Wishlist wishlist)
        {
            if (ModelState.IsValid)
            {
                db.Wishlists.Add(wishlist);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerUserName = new SelectList(db.Customers, "UserName", "Name", wishlist.CustomerUserName);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", wishlist.ProductId);
            return View(wishlist);
        }

        // GET: Wishlists/Edit/5
        [Authorize(Users = "admin@admin.com")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wishlist wishlist = db.Wishlists.Find(id);
            if (wishlist == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerUserName = new SelectList(db.Customers, "UserName", "Name", wishlist.CustomerUserName);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", wishlist.ProductId);
            return View(wishlist);
        }

        // POST: Wishlists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CustomerUserName,ProductId")] Wishlist wishlist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wishlist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerUserName = new SelectList(db.Customers, "UserName", "Name", wishlist.CustomerUserName);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", wishlist.ProductId);
            return View(wishlist);
        }

        // GET: Wishlists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wishlist wishlist = db.Wishlists.Find(id);
            if (wishlist == null)
            {
                return HttpNotFound();
            }
            return View(wishlist);
        }

        // POST: Wishlists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Wishlist wishlist = db.Wishlists.Find(id);
            db.Wishlists.Remove(wishlist);
            db.SaveChanges();
            if(User.Identity.Name == "admin@admin.com")
            {
                TempData["confirm"] = "Item successfully deleted from wishlist!";
                return RedirectToAction("../Admin/Index");
            }
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
