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
    public class CustomersController : Controller
    {
        private MobilesDBEntities db = new MobilesDBEntities();

        // GET: Customers
        [Authorize(Users = "admin@admin.com")]
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }

        // GET: Customers/Details/5
        [Authorize(Users = "admin@admin.com")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        [Authorize(Users = "admin@admin.com")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "admin@admin.com")]
        public ActionResult Create([Bind(Include = "UserName,Name,Address")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserName,Name,Address,Phone")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                TempData["confirm"] = "User " + customer.Name + " successfully edited!";
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Home/Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        [Authorize(Users = "admin@admin.com")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            try
            {
                Cart cart = db.Carts.First(c => (c.Customer.UserName == customer.UserName));
            }
            catch (Exception)
            {
                try
                {
                    Review review = db.Reviews.First(c => (c.CustomerUserName == customer.UserName));
                }
                catch (Exception)
                {
                    try
                    {
                        Wishlist wishlist = db.Wishlists.First(c => (c.CustomerUserName == customer.UserName));
                    }
                    catch (Exception)
                    {
                        try
                        {
                            History history = db.Histories.First(c => (c.CustomerUserName == customer.UserName));
                        }
                        catch (Exception)
                        {
                            return View(customer);
                        }
                    }
                }
            }
            ViewBag.error = "This customer still have a cart, review, wishlist, or history record, it cannot be deleted.";
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "admin@admin.com")]
        public ActionResult DeleteConfirmed(string id)
        {
            Customer customer = db.Customers.Find(id);
            TempData["confirm"] = "User " + customer.Name + " successfully deleted!";
            db.Customers.Remove(customer);
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
