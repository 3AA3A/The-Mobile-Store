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
    public class SpecsController : Controller
    {
        private MobilesDBEntities db = new MobilesDBEntities();

        // GET: Specs
        public ActionResult Index()
        {
            return View(db.Specs.ToList());
        }

        // GET: Specs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Spec spec = db.Specs.Find(id);
            if (spec == null)
            {
                return HttpNotFound();
            }
            return View(spec);
        }

        // GET: Specs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Specs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Ram,Storage,Camera,Display")] Spec spec)
        {
            var spc = db.Specs.Select(i => i);
            spc = spc.Where(i => i.Ram == spec.Ram && i.Storage == spec.Storage && i.Camera == spec.Camera && i.Display == spec.Display);
            if (spc.Count() != 0)
            {
                ViewBag.error = "This specs already exists!";
            }
            else if(ModelState.IsValid)
            {
                db.Specs.Add(spec);
                db.SaveChanges();
                return RedirectToAction("../Admin/Index");
            }

            return View(spec);
        }

        // GET: Specs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Spec spec = db.Specs.Find(id);
            if (spec == null)
            {
                return HttpNotFound();
            }
            return View(spec);
        }

        // POST: Specs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Ram,Storage,Camera,Display")] Spec spec)
        {
            if (ModelState.IsValid)
            {
                TempData["confirm"] = "Specs successfully edited!";
                db.Entry(spec).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Admin/Index");
            }
            return View(spec);
        }

        // GET: Specs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Spec spec = db.Specs.Find(id);
            if (spec == null)
            {
                return HttpNotFound();
            }
            try
            {
                Product product = db.Products.First(c => (c.SpecsId == spec.Id));
            }
            catch (Exception)
            {
                return View(spec);
            }
            ViewBag.error = "This spec is being used by a product, it cannot be deleted.";
            return View(spec);
        }

        // POST: Specs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TempData["confirm"] = "Specs successfully deleted!";
            Spec spec = db.Specs.Find(id);
            db.Specs.Remove(spec);
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
