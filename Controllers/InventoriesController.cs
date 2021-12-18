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
    public class InventoriesController : Controller
    {
        private MobilesDBEntities db = new MobilesDBEntities();

        // GET: Inventories
        public ActionResult Index()
        {
            var inventories = db.Inventories.Include(i => i.Product);
            return View(inventories.ToList());
        }

        // GET: Inventories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = db.Inventories.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }

        // GET: Inventories/Create
        public ActionResult Create()
        {
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name");
            return View();
        }

        // POST: Inventories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProductId,Color,Amount")] Inventory inventory)
        {
            var inv = db.Inventories.Select(i => i);
            inv = inv.Where(i => i.ProductId == inventory.ProductId && i.Color == inventory.Color);
            if (inv.Count() != 0)
            {
                ViewBag.error = "Duplicate Records";
            }
            else if (ModelState.IsValid)
            {
                db.Inventories.Add(inventory);
                db.SaveChanges();
                return RedirectToAction("../Admin/Index");
            }

            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", inventory.ProductId);
            return View(inventory);
        }

        // GET: Inventories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = db.Inventories.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", inventory.ProductId);
            return View(inventory);
        }

        // POST: Inventories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProductId,Color,Amount")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                TempData["confirm"] = "Inventory item successfully edited!";
                db.Entry(inventory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Admin/Index");
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", inventory.ProductId);
            return View(inventory);
        }

        // GET: Inventories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = db.Inventories.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }

        // POST: Inventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TempData["confirm"] = "Inventory item successfully deleted!";
            Inventory inventory = db.Inventories.Find(id);
            db.Inventories.Remove(inventory);
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
