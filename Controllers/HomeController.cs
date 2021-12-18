using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobilesProject.Models;

namespace MobilesProject.Controllers
{
    public class HomeController : Controller
    {
        private MobilesDBEntities db = new MobilesDBEntities();
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
            if(!String.IsNullOrEmpty(productBrand))
            {
                filters.Add(productBrand);
                products = products.Where(p => p.Supplier.BrandName == productBrand);
            }

            if(!String.IsNullOrEmpty(productType))
            {
                filters.Add(productType);
                products = products.Where(p => p.Category.Type == productType); 
            }
            if(!String.IsNullOrEmpty(productOs)) {
                filters.Add(productOs);
                products = products.Where(p => p.Category.OperatingSystem == productOs);
            }
            if(!String.IsNullOrEmpty(productRes))
            {
                filters.Add(productRes);
                products = products.Where(p => p.Category.Resolution == productRes);
            }

            if(!String.IsNullOrEmpty(productRam))
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
                }

            }

            ViewBag.filters = string.Join(", ", filters);

            Random random = new Random();
            // get all products under the same brand
            var featured = db.Products.Select(p => p).ToList();

            // shuffle array and take first 5 products
            if (featured.Count() > 5)
            {
                for (int i = featured.Count() - 1; i >= 0; i--)
                {
                    Product temp = featured[i];
                    int randomint = random.Next(i + 1);

                    featured[i] = featured[randomint];
                    featured[randomint] = temp;
                }
                var featuredtemp = featured.Take(5).ToList();
                ViewBag.feat1 = featuredtemp[0];
                ViewBag.feat2 = featuredtemp[1];
                ViewBag.feat3 = featuredtemp[2];
                ViewBag.feat4 = featuredtemp[3];
                ViewBag.feat5 = featuredtemp[4];
            }
            else
            {
                ViewBag.feat1 = featured[0];
                ViewBag.feat2 = featured[1];
                ViewBag.feat3 = featured[2];
                ViewBag.feat4 = featured[3];
                ViewBag.feat5 = featured[4];
            }

            return View(products);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}