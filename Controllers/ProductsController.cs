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
    public class ProductsController : Controller
    {
        private MobilesDBEntities db = new MobilesDBEntities();

        // GET: Products
        public ActionResult Index(string productName, string productBrand, string productType, string productOs, string productRes, string productRam, string productStorage, string productCamera, string productDisplay, int? id)
        {
            var products = db.Products.AsQueryable();
            Product product = db.Products.Find(id);
            //search by brand dropdown
            var brands = db.Suppliers.Select(s => s.BrandName);
            ViewBag.productBrand = new SelectList(brands);

            //search by categories dropdowns
            var types = db.Categories.Select(c => c.Type).Distinct();
            var os = db.Categories.Select(c => c.OperatingSystem).Distinct();
            var res = db.Categories.Select(c => c.Resolution).Distinct();
            ViewBag.productType = types.ToList();
            ViewBag.productOs = os.ToList();
            ViewBag.productRes = res.ToList();

            //search by specs dropdowns
            var ram = db.Specs.Select(s => s.Ram).Distinct();
            var storage = db.Specs.Select(s => s.Storage).Distinct();
            var camera = db.Specs.Select(s => s.Camera).Distinct();
            var display = db.Specs.Select(s => s.Display).Distinct();
            ViewBag.productRam = ram.ToList();
            ViewBag.productStorage = storage.ToList();
            ViewBag.productCamera = camera.ToList();
            ViewBag.productDisplay = display.ToList();

            List<string> filters = new List<string>();
            if (!String.IsNullOrEmpty(productName))
            {
                filters.Add(productName);
                products = products.Where(p => p.Name.Contains(productName));
            }
            if (!String.IsNullOrEmpty(productBrand))
            {
                filters.Add(productBrand);
                products = products.Where(p => p.Supplier.BrandName == productBrand);
            }

            if (!String.IsNullOrEmpty(productType))
            {
                filters.Add(productType);
                products = products.Where(p => p.Category.Type == productType);
            }
            if (!String.IsNullOrEmpty(productOs))
            {
                filters.Add(productOs);
                products = products.Where(p => p.Category.OperatingSystem == productOs);
            }
            if (!String.IsNullOrEmpty(productRes))
            {
                filters.Add(productRes);
                products = products.Where(p => p.Category.Resolution == productRes);
            }

            if (!String.IsNullOrEmpty(productRam))
            {
                filters.Add(productRam + " RAM");
                products = products.Where(p => p.Spec.Ram == productRam);
            }
            if (!String.IsNullOrEmpty(productStorage))
            {
                filters.Add(productStorage + " GB");
                products = products.Where(p => p.Spec.Storage == productStorage);
            }
            if (!String.IsNullOrEmpty(productCamera))
            {
                filters.Add(productCamera);
                products = products.Where(p => p.Spec.Camera == productCamera);
            }
            if (!String.IsNullOrEmpty(productDisplay))
            {
                filters.Add(productDisplay);
                products = products.Where(p => p.Spec.Display == productDisplay);
            }

            if (product != null)
            {
                Wishlist wishlisti = new Wishlist
                {
                    CustomerUserName = User.Identity.Name,
                    ProductId = product.Id
                };
                string username = User.Identity.Name;
                var wishlistx = db.Wishlists.Where(d => d.CustomerUserName == username && d.ProductId == product.Id);
                if (wishlistx.Count() == 0)
                {
                    db.Wishlists.Add(wishlisti);
                    //ViewBag.prod = db.Products.Find(id);
                    db.SaveChanges();
                    TempData["confirm"] = product.Name + " added to wishlist!";
                    return RedirectToAction("Index", "Wishlists");
                }
                else
                {
                    TempData["error"] = product.Name + " is already in your wishlist!";
                    return RedirectToAction("Index", "Wishlists");
                }

            }

            ViewBag.filters = string.Join(", ", filters);

            return View(products);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            var inv = db.Inventories.Where(x => x.ProductId == id);
            List<string> colors = new List<string>();
            foreach (var item in inv)
            {
                colors.Add(item.Color);
            }
            ViewBag.colors = string.Join(", ", colors);

            //search by brand dropdown
            var brands = db.Suppliers.Select(s => s.BrandName);
            ViewBag.productBrand = new SelectList(brands);

            //search by categories dropdowns
            var types = db.Categories.Select(c => c.Type).Distinct();
            var os = db.Categories.Select(c => c.OperatingSystem).Distinct();
            var res = db.Categories.Select(c => c.Resolution).Distinct();
            ViewBag.productType = new SelectList(types);
            ViewBag.productOs = new SelectList(os);
            ViewBag.productRes = new SelectList(res);

            //search by specs dropdowns
            var ram = db.Specs.Select(s => s.Ram).Distinct();
            var storage = db.Specs.Select(s => s.Storage).Distinct();
            var camera = db.Specs.Select(s => s.Camera).Distinct();
            var display = db.Specs.Select(s => s.Display).Distinct();
            ViewBag.productRam = new SelectList(ram);
            ViewBag.productStorage = new SelectList(storage);
            ViewBag.productCamera = new SelectList(camera);
            ViewBag.productDisplay = new SelectList(display);

            if (User.Identity.IsAuthenticated && User.Identity.Name != "admin@admin.com")
            {
                History history = new History
                {
                    CustomerUserName = User.Identity.Name,
                    ProductId = product.Id
                };
                var histories = db.Histories.Where(x => x.CustomerUserName == User.Identity.Name && x.ProductId == product.Id);
                if (histories.Count() == 0)
                {
                    db.Histories.Add(history);
                    db.SaveChanges();
                }
            }

            Random random = new Random();
            // get all products under the same brand
            var similar = db.Products.Where(p => p.SupplierId == product.SupplierId && p.Id != product.Id).ToList();

            // shuffle array and take first 5 products
            if (similar.Count() > 5)
            {
                for (int i = similar.Count() - 1; i >= 0; i--)
                {
                    Product temp = similar[i];
                    int randomint = random.Next(i + 1);

                    similar[i] = similar[randomint];
                    similar[randomint] = temp;
                }
                ViewBag.similar = similar.Take(5);
            }
            else
            {
                ViewBag.similar = similar;
            }
            ViewBag.count = similar.Count();

            // get all products under the same os 
            var similar2 = db.Products.Where(p => p.Category.Type == product.Category.Type && p.Id != product.Id).ToList();

            // shuffle array and take first 5 products
            if (similar2.Count() > 5)
            {
                for (int i = similar2.Count() - 1; i >= 0; i--)
                {
                    Product temp = similar2[i];
                    int randomint = random.Next(i + 1);

                    similar2[i] = similar2[randomint];
                    similar2[randomint] = temp;
                }
                ViewBag.similar2 = similar2.Take(5);
            }
            else
            {
                ViewBag.similar2 = similar2;
            }
            ViewBag.count2 = similar2.Count();

            return View(product);
        }

        // GET: Products/Create
        [Authorize(Users = "admin@admin.com")]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Id");
            ViewBag.SpecsId = new SelectList(db.Specs, "Id", "Id");
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "BrandName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "admin@admin.com")]
        public ActionResult Create([Bind(Include = "Id,Name,Price,Image,SpecsId,SupplierId,CategoryId")] Product product)
        {
            var prod = db.Products.Select(i => i);
            prod = prod.Where(i => i.Name == product.Name);
            if (prod.Count() != 0)
            {
                ViewBag.error = "This product already exists!";
            }
            else if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Id", product.CategoryId);
            ViewBag.SpecsId = new SelectList(db.Specs, "Id", "Id", product.SpecsId);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "BrandName", product.SupplierId);
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Users = "admin@admin.com")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Id", product.CategoryId);
            ViewBag.SpecsId = new SelectList(db.Specs, "Id", "Id", product.SpecsId);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "BrandName", product.SupplierId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "admin@admin.com")]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,Image,SpecsId,SupplierId,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                TempData["confirm"] = product.Name + " cart successfully edited!";
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Admin/Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Id", product.CategoryId);
            ViewBag.SpecsId = new SelectList(db.Specs, "Id", "Id", product.SpecsId);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "BrandName", product.SupplierId);
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Users = "admin@admin.com")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            try
            {
                CartItem cartitem = db.CartItems.First(c => (c.ProductId == product.Id));
            }
            catch (Exception)
            {
                try
                {
                    Inventory inventory = db.Inventories.First(c => (c.ProductId == product.Id));
                }
                catch (Exception)
                {
                    return View(product);
                }
            }
            ViewBag.error = "This item either exists in a cartItem record or it already has an inventory record, it cannot be deleted.";
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "admin@admin.com")]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            TempData["confirm"] = product.Name + " succesfully deleted!";
            db.Products.Remove(product);
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
