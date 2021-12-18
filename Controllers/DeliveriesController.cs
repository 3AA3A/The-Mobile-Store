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
    public class DeliveriesController : Controller
    {
        private MobilesDBEntities db = new MobilesDBEntities();

        // GET: Deliveries
        [Authorize(Users = "admin@admin.com")]
        public ActionResult Index()
        {
            var deliveries = db.Deliveries.Include(d => d.Cart).Include(d => d.Shipping);
            return View(deliveries.ToList());
        }

        // GET: Deliveries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delivery delivery = db.Deliveries.Find(id);
            if (delivery == null)
            {
                return HttpNotFound();
            }
            return View(delivery);
        }
        [Authorize(Users = "admin@admin.com")]
        // GET: Deliveries/Create
        public ActionResult Create(int? id)
        {
            ViewBag.CartId = new SelectList(db.Carts, "Id", "Id");
            ViewBag.ShippingId = new SelectList(db.Shippings, "Id", "Id");
            @TempData["CartId"] = id;
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View();
        }

        // POST: Deliveries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Type,Status,CartId,ShippingId")] Delivery delivery)
        {
            var del = db.Deliveries.Select(i => i);
            del = del.Where(i => i.CartId == delivery.CartId);
            if (del.Count() != 0)
            {
                ViewBag.error = "This cart already has a delivery!";
            }
            else if (ModelState.IsValid)
            {
                db.Deliveries.Add(delivery);
                Cart cart = db.Carts.Find(delivery.CartId);
                cart.Status = "shipped";
                db.SaveChanges();
                return RedirectToAction("../Admin/Index");
            }
            @TempData["CartId"] = delivery.CartId;
            ViewBag.CartId = new SelectList(db.Carts, "Id", "Id", delivery.CartId);
            ViewBag.ShippingId = new SelectList(db.Shippings, "Id", "Id", delivery.ShippingId);
            return View(delivery);
        }
        [Authorize(Users = "admin@admin.com")]
        // GET: Deliveries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delivery delivery = db.Deliveries.Find(id);
            if (delivery == null)
            {
                return HttpNotFound();
            }
            ViewBag.CartId = new SelectList(db.Carts, "Id", "Id", delivery.CartId);
            ViewBag.ShippingId = new SelectList(db.Shippings, "Id", "Id", delivery.ShippingId);
            return View(delivery);
        }

        // POST: Deliveries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Type,Status,CartId,ShippingId")] Delivery delivery)
        {
            if (ModelState.IsValid)
            {
                TempData["confirm"] = "Delivery successfully edited!";
                db.Entry(delivery).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Admin/Index");
            }
            ViewBag.CartId = new SelectList(db.Carts, "Id", "Id", delivery.CartId);
            ViewBag.ShippingId = new SelectList(db.Shippings, "Id", "Id", delivery.ShippingId);
            return View(delivery);
        }
        [Authorize(Users = "admin@admin.com")]
        // GET: Deliveries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delivery delivery = db.Deliveries.Find(id);
            if (delivery == null)
            {
                return HttpNotFound();
            }
            return View(delivery);
        }

        // POST: Deliveries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TempData["confirm"] = "Delivery successfully deleted!";
            Delivery delivery = db.Deliveries.Find(id);
            db.Deliveries.Remove(delivery);
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
