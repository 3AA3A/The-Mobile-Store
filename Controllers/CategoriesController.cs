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
    public class CategoriesController : Controller
    {
        private MobilesDBEntities db = new MobilesDBEntities();

        // GET: Categories
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Type,OperatingSystem,Resolution")] Category category)
        {
            var cat = db.Categories.Select(i => i);
            cat = cat.Where(i => i.Type == category.Type && i.OperatingSystem == category.OperatingSystem && i.Resolution == category.Resolution);
            if (cat.Count() != 0)
            {
                ViewBag.error = "This category already exists!";
            }
            else if(ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("../Admin/Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Type,OperatingSystem,Resolution")] Category category)
        {
            if (ModelState.IsValid)
            {
                TempData["confirm"] = "Category successfully edited!";
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Admin/Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            try
            {
                Product product = db.Products.First(c => (c.CategoryId == category.Id));
            }
            catch (Exception)
            {
                return View(category);
            }
            ViewBag.error = "This category is being used by a product, it cannot be deleted.";
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            TempData["confirm"] = "Category " + category.Type + " successfully deleted!";
            db.Categories.Remove(category);
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
