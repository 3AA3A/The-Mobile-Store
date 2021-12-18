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
    public class PaymentsController : Controller
    {
        private MobilesDBEntities db = new MobilesDBEntities();

        // GET: Payments
        [Authorize(Users = "admin@admin.com")]
        public ActionResult Index()
        {
            var payments = db.Payments.Include(p => p.Cart);
            return View(payments.ToList());
        }

        // GET: Payments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // GET: Payments/Create
        public ActionResult Create(int? id)
        {
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
            int total = 0;
            int discount = 0;
            foreach (var item in cart.CartItems)
            {
                total += (int)item.Quantity * (int)item.Product.Price;
               
            }
            string username = User.Identity.Name;
            string name = username.Split('@')[0];
            var query = from a in db.Coupons
                        where a.CustomerName == name && a.Status == "Active"
                        select a;
            var itemx = query.FirstOrDefault();
            if (itemx != null)
            {
                discount = ((total * (int)itemx.Discount) / 100);
                itemx.Status = "Void";
                db.SaveChanges();
            }
            

            @TempData["amount"] = total;
            @TempData["discount"] = discount;
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CartId,Type,Amount,Discount,Tax,Total,PaymentDate")] Payment payment)
        {
            var pay = db.Payments.Select(i => i);
            pay = pay.Where(i => i.CartId == payment.CartId);
            if (pay.Count() != 0)
            {
                ViewBag.error = "This cart already has a payment!";
            }
            else if (ModelState.IsValid)
            {
                
                db.Payments.Add(payment);
                Cart cart = db.Carts.Find(payment.CartId);
                cart.Status = "paid";
                db.SaveChanges();
                foreach (var item in cart.CartItems)
                {
                    var query = from a in db.Inventories
                                where a.ProductId == item.ProductId
                                select a;
                    var itemx = query.FirstOrDefault();
                    if (itemx != null)
                    {
                        var inven = db.Inventories.First(x => x.ProductId == item.Product.Id);

                        inven.Amount -= (int)item.Quantity;
                        db.SaveChanges();
                    }
                }
               
                return RedirectToAction("Index", "Carts");
            }

            ViewBag.CartId = new SelectList(db.Carts, "Id", "CustomerName", payment.CartId);
            return View(payment);
        }

        // GET: Payments/Edit/5
        [Authorize(Users = "admin@admin.com")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CartId = new SelectList(db.Carts, "Id", "CustomerName", payment.CartId);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CartId,Type,Amount,Discount,Tax,Total,PaymentDate")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CartId = new SelectList(db.Carts, "Id", "CustomerName", payment.CartId);
            return View(payment);
        }

        // GET: Payments/Delete/5
        [Authorize(Users = "admin@admin.com")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TempData["confirm"] = "Payment record successfully deleted!";
            Payment payment = db.Payments.Find(id);
            db.Payments.Remove(payment);
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
