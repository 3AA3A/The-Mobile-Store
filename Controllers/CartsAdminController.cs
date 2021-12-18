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
    public class CartsAdminController : Controller
    {
        private MobilesDBEntities db = new MobilesDBEntities();

        // GET: CartsAdmin
        public ActionResult Index()
        {
            var carts = db.Carts.Include(c => c.Customer);
            return View(carts.ToList());
        }

        // GET: CartsAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            ViewBag.total = cart.CartItems.Sum(p => (int)p.Quantity * (int)p.Product.Price);
            return View(cart);
        }

        // GET: CartsAdmin/Create
        public ActionResult Create()
        {
            ViewBag.CustomerName = new SelectList(db.Customers, "UserName", "Name");
            return View();
        }

        // POST: CartsAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CustomerName,Status")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Carts.Add(cart);
                db.SaveChanges();
                return RedirectToAction("../Admin/Index");
            }

            ViewBag.CustomerName = new SelectList(db.Customers, "UserName", "Name", cart.CustomerName);
            return View(cart);
        }

        // GET: CartsAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerName = new SelectList(db.Customers, "UserName", "Name", cart.CustomerName);
            return View(cart);
        }

        // POST: CartsAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CustomerName,Status")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                TempData["confirm"] = "Cart successfully edited!";
                db.Entry(cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Admin/Index");
            }
            ViewBag.CustomerName = new SelectList(db.Customers, "UserName", "Name", cart.CustomerName);
            return View(cart);
        }

        // GET: CartsAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: CartsAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cart cart = db.Carts.Find(id);
            try
            {
                CartItem cartitem = db.CartItems.First(c => (c.CartId == cart.Id));
            }
            catch (Exception)
            {
                TempData["confirm"] = cart.Customer.Name + "'s cart successfully deleted!";
                db.Carts.Remove(cart);
                db.SaveChanges();
                return RedirectToAction("../Admin/Index");
            }
            TempData["error"] = "The cart you tried to delete cannot be deleted, as it has an item inside.";
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

        public ActionResult DeliveryShip(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Create", "Deliveries", new { id = cart.Id });
        }
    }
}
