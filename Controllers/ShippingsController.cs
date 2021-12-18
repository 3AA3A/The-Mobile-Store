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
    [Authorize(Users = "admin@admin.com")]
    public class ShippingsController : Controller
    {
        private MobilesDBEntities db = new MobilesDBEntities();

        // GET: Shippings
        public ActionResult Index()
        {
            return View(db.Shippings.ToList());
        }

        // GET: Shippings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipping shipping = db.Shippings.Find(id);
            if (shipping == null)
            {
                return HttpNotFound();
            }
            return View(shipping);
        }
        [Authorize(Users = "admin@admin.com")]
        // GET: Shippings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Shippings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ShippingCarrier,ShippingCost,ExpectedDate,ArrivalDate,Status,Location")] Shipping shipping)
        {
            var ship = db.Shippings.Select(i => i);
            ship = ship.Where(i => i.ShippingCarrier == shipping.ShippingCarrier && i.ExpectedDate == shipping.ExpectedDate && i.Location == shipping.Location);
            if (ship.Count() != 0)
            {
                ViewBag.error = "This shipping record already exist!";
            }
            else if (ModelState.IsValid)
            {
                db.Shippings.Add(shipping);
                db.SaveChanges();
                return RedirectToAction("../Admin/Index");
            }

            return View(shipping);
        }
        [Authorize(Users = "admin@admin.com")]
        // GET: Shippings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipping shipping = db.Shippings.Find(id);
            if (shipping == null)
            {
                return HttpNotFound();
            }
            return View(shipping);
        }

        // POST: Shippings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ShippingCarrier,ShippingCost,ExpectedDate,ArrivalDate,Status,Location")] Shipping shipping)
        {
            if (ModelState.IsValid)
            {
                TempData["confirm"] = "Shipping record successfully edited!";
                db.Entry(shipping).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Admin/Index");
            }
            return View(shipping);
        }
        [Authorize(Users = "admin@admin.com")]
        // GET: Shippings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipping shipping = db.Shippings.Find(id);
            if (shipping == null)
            {
                return HttpNotFound();
            }
            try
            {
                Delivery delivery = db.Deliveries.First(c => (c.ShippingId == shipping.Id));
            }
            catch (Exception)
            {
                return View(shipping);
            }
            ViewBag.error = "This shipping already has a delivery scheduled, it cannot be deleted.";
            return View(shipping);
        }

        // POST: Shippings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TempData["confirm"] = "Shipping record successfully deleted!";
            Shipping shipping = db.Shippings.Find(id);
            db.Shippings.Remove(shipping);
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
