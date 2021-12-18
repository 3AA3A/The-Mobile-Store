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
    public class CartsController : Controller
    {
        private MobilesDBEntities db = new MobilesDBEntities();

        // GET: Carts
        public ActionResult Index()
        {
            string username = User.Identity.Name;
            var carts = db.Carts.Where(c => c.CustomerName == username);
            return View(carts.ToList());

        }

        // GET: Carts/Details/5
        public ActionResult Details(int? id, int? suppid)
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

            int total = 0;
            foreach (var item in cart.CartItems)
            {
                total += (int)item.Quantity * (int)item.Product.Price;
            }
            ViewBag.total = total;

            if (suppid != null)
            {
                try
                {
                    Supplier supplier = db.Suppliers.Find(suppid);
                    ViewBag.supplier = supplier;
                }
                catch (Exception) { }
            }

            return View(cart);
        }

        public ActionResult UpdateProfile()
        {
            string username = User.Identity.Name;
            string name = username.Split('@')[0];
            try
            {
                // 2 -- do we have a customer with that username?
                //If not found throws System.InvalidOperationException
                db.Customers.First(c => c.UserName.Equals(username));
            }
            catch (Exception)
            {
                // no we don't -- create one (need it due to foreign key constraint)
                db.Customers.Add(new Customer { UserName = username, Name = name });
                db.SaveChanges();
            }

            return RedirectToAction("Edit", "Customers", new { id = User.Identity.Name + "/" });
        }

        public ActionResult PaymentDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.First(p => p.CartId == id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Details", "Payments", new { id = payment.Id });
        }

        public ActionResult DeliveryDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delivery delivery = db.Deliveries.First(p => p.CartId == id);
            if (delivery == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Details", "Deliveries", new { id = delivery.Id });
        }

        // GET: Carts/Delete/5
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

        // POST: Carts/Delete/5
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
                db.Carts.Remove(cart);
                db.SaveChanges();
                //return RedirectToAction("~/Transaction/MyCart");
                return RedirectToAction("Index");
            }
            TempData["error"] = "The cart you tried to delete cannot be deleted, as it has an item inside.";
            return RedirectToAction("Details", new { id = id });
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
