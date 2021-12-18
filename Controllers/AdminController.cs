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
    [Authorize(Users ="admin@admin.com")]
    public class AdminController : Controller
    {
        private MobilesDBEntities db = new MobilesDBEntities();
        // GET: Admin
        public ActionResult Index(string prodname, string prodbrand, string prodtype, string prodos, string prodres, string prodram, string prodstorage, string prodcamera, string proddisplay, string cartname, string cartstat, string customername, string customeradd, string categtype, string categos, string categres, string coupcode, string coupcustom, int? coupfrom, int? coupto, string coupstat, string deltype, string delstat, string delcustom, string delcar, string faqcateg, string faqquest, string histoprod, string histocust, string invenprod, string invencolor, int? invenfrom, int? invento, string paytype, int? payamountfrom, int? payamountto, int? paydiscfrom, int? paydiscto, int? paytaxfrom, int? paytaxto, int? paytotalfrom, int? paytotalto, DateTime? paydatefrom, DateTime? paydateto, string paycustom, int? paycart, DateTime? revdatefrom, DateTime? revdateto, string revcustom, string revtitle, string revfeed, string shipcar, string shiploc, string shipstat, DateTime? shipexpfrom, DateTime? shipexpto, DateTime? shiparrfrom, DateTime? shiparrto, string specram, string specstorage, string speccamera, string specdisplay, string suppbrand, string wishprod, string wishcust)
        {
            var prod = db.Products.Select(x => x);
            var carts = db.Carts.Include(c => c.Customer);
            var customers = db.Customers.Select(x => x);
            var categories = db.Categories.Select(x => x);
            var coupons = db.Coupons.Select(x => x);
            var deliveries = db.Deliveries.Include(d => d.Cart).Include(d => d.Shipping);
            var faqs = db.Faqs.Select(x => x);
            var histories = db.Histories.Include(h => h.Customer).Include(h => h.Product);
            var inventories = db.Inventories.Include(i => i.Product);
            var payments = db.Payments.Include(p => p.Cart);
            var reviews = db.Reviews.Include(r => r.Customer);
            var shippings = db.Shippings.Select(x => x);
            var specs = db.Specs.Select(x => x);
            var suppliers = db.Suppliers.Select(x => x);
            var wishlists = db.Wishlists.Include(w => w.Customer).Include(w => w.Product);

            // Dropdown Lists
            var prodbrands = db.Suppliers.Select(s => s.BrandName);
            var prodtypes = db.Categories.Select(c => c.Type).Distinct();
            var prodoss = db.Categories.Select(c => c.OperatingSystem).Distinct();
            var prodress = db.Categories.Select(c => c.Resolution).Distinct();
            var prodrams = db.Specs.Select(s => s.Ram).Distinct();
            var prodstorages = db.Specs.Select(s => s.Storage).Distinct();
            var prodcameras = db.Specs.Select(s => s.Camera).Distinct();
            var proddisplays = db.Specs.Select(s => s.Display).Distinct();
            var delcarriers = db.Shippings.Select(s => s.ShippingCarrier).Distinct();
            var faqcategs = db.Faqs.Select(s => s.Category).Distinct();
            var invencolors = db.Inventories.Select(s => s.Color).Distinct();
            ViewBag.prodbrand = new SelectList(prodbrands);
            ViewBag.prodtype = new SelectList(prodtypes);
            ViewBag.prodos = new SelectList(prodoss); 
            ViewBag.prodres = new SelectList(prodress); 
            ViewBag.prodram = new SelectList(prodrams);
            ViewBag.prodstorage = new SelectList(prodstorages); 
            ViewBag.prodcamera = new SelectList(prodcameras); 
            ViewBag.proddisplay = new SelectList(proddisplays);
            ViewBag.categtype = new SelectList(prodtypes);
            ViewBag.categos = new SelectList(prodoss);
            ViewBag.categres = new SelectList(prodress);
            ViewBag.delcar = new SelectList(delcarriers);
            ViewBag.faqcateg = new SelectList(faqcategs);
            ViewBag.invencolor = new SelectList(invencolors);
            ViewBag.shipcar = new SelectList(delcarriers);
            ViewBag.specram = new SelectList(prodrams);
            ViewBag.specstorage = new SelectList(prodstorages);
            ViewBag.speccamera = new SelectList(prodcameras);
            ViewBag.specdisplay = new SelectList(proddisplays);

            if (!String.IsNullOrEmpty(prodname))
            {
                prod = prod.Where(p => p.Name.Contains(prodname));
            }
            if (!String.IsNullOrEmpty(prodbrand))
            {
                prod = prod.Where(p => p.Supplier.BrandName == prodbrand);
            }
            if (!String.IsNullOrEmpty(prodtype))
            {
                prod = prod.Where(p => p.Category.Type == prodtype);
            }
            if (!String.IsNullOrEmpty(prodos))
            {
                prod = prod.Where(p => p.Category.OperatingSystem == prodos);
            }
            if (!String.IsNullOrEmpty(prodres))
            {
                prod = prod.Where(p => p.Category.Resolution == prodres);
            }
            if (!String.IsNullOrEmpty(prodram))
            {
                prod = prod.Where(p => p.Spec.Ram == prodram);
            }
            if (!String.IsNullOrEmpty(prodstorage))
            {
                prod = prod.Where(p => p.Spec.Storage == prodstorage);
            }
            if (!String.IsNullOrEmpty(prodcamera))
            {
                prod = prod.Where(p => p.Spec.Camera == prodcamera);
            }
            if (!String.IsNullOrEmpty(proddisplay))
            {
                prod = prod.Where(p => p.Spec.Display == proddisplay);
            }
            ViewBag.Products = prod.ToList();

            if (!String.IsNullOrEmpty(cartname))
            {
                carts = carts.Where(p => p.Customer.Name.Contains(cartname));
            }
            if (!String.IsNullOrEmpty(cartstat))
            {
                carts = carts.Where(p => p.Status == cartstat);
            }
            ViewBag.Carts = carts.ToList();

            if (!String.IsNullOrEmpty(customername))
            {
                customers = customers.Where(p => p.Name.Contains(customername));
            }
            if (!String.IsNullOrEmpty(customeradd))
            {
                customers = customers.Where(p => p.Address.Contains(customeradd));
            }
            ViewBag.Customers = customers.ToList();

            if (!String.IsNullOrEmpty(categtype))
            {
                categories = categories.Where(p => p.Type == categtype);
            }
            if (!String.IsNullOrEmpty(categos))
            {
                categories = categories.Where(p => p.OperatingSystem == categos);
            }
            if (!String.IsNullOrEmpty(categres))
            {
                categories = categories.Where(p => p.Resolution == categres);
            }
            ViewBag.Categories = categories.ToList();

            if (!String.IsNullOrEmpty(coupcode))
            {
                coupons = coupons.Where(p => p.Code.Contains(coupcode));
            }
            if (!String.IsNullOrEmpty(coupcustom))
            {
                coupons = coupons.Where(p => p.CustomerName.Contains(coupcustom));
            }
            if (coupfrom != null)
            {
                coupons = coupons.Where(p => p.Discount >= coupfrom);
            }
            if (coupto != null)
            {
                coupons = coupons.Where(p => p.Discount <= coupto);
            }
            if (!String.IsNullOrEmpty(coupstat))
            {
                coupons = coupons.Where(p => p.Status == coupstat);
            }
            ViewBag.Coupons = coupons.ToList();

            if (!String.IsNullOrEmpty(deltype))
            {
                deliveries = deliveries.Where(p => p.Type == deltype);
            }
            if (!String.IsNullOrEmpty(delstat))
            {
                deliveries = deliveries.Where(p => p.Status == delstat);
            }
            if (!String.IsNullOrEmpty(delcustom))
            {
                deliveries = deliveries.Where(p => p.Cart.Customer.Name.Contains(delcustom));
            }
            if (!String.IsNullOrEmpty(delcar))
            {
                deliveries = deliveries.Where(p => p.Shipping.ShippingCarrier == delcar);
            }
            ViewBag.Deliveries = deliveries.ToList();

            if (!String.IsNullOrEmpty(faqcateg))
            {
                faqs = faqs.Where(p => p.Category == faqcateg);
            }
            if (!String.IsNullOrEmpty(faqquest))
            {
                faqs = faqs.Where(p => p.Question.Contains(faqquest));
            }
            ViewBag.Faqs = faqs.ToList();

            if (!String.IsNullOrEmpty(histoprod))
            {
                histories = histories.Where(p => p.Product.Name.Contains(histoprod));
            }
            if (!String.IsNullOrEmpty(histocust))
            {
                histories = histories.Where(p => p.Customer.Name.Contains(histocust));
            }
            ViewBag.Histories = histories.ToList();

            if (!String.IsNullOrEmpty(invenprod))
            {
                inventories = inventories.Where(p => p.Product.Name.Contains(invenprod));
            }
            if (!String.IsNullOrEmpty(invencolor))
            {
                inventories = inventories.Where(p => p.Color == invencolor);
            }
            if (invenfrom != null)
            {
                inventories = inventories.Where(p => p.Amount >= invenfrom);
            }
            if (invento != null)
            {
                inventories = inventories.Where(p => p.Amount <= invento);
            }
            ViewBag.Inventories = inventories.ToList();

            if (!String.IsNullOrEmpty(paytype))
            {
                payments = payments.Where(p => p.Type == paytype);
            }
            if (payamountfrom != null)
            {
                payments = payments.Where(p => p.Amount >= payamountfrom);
            }
            if (payamountto != null)
            {
                payments = payments.Where(p => p.Amount <= payamountto);
            }
            if (paydiscfrom != null)
            {
                payments = payments.Where(p => p.Discount >= paydiscfrom);
            }
            if (paydiscto != null)
            {
                payments = payments.Where(p => p.Discount <= paydiscto);
            }
            if (paytaxfrom != null)
            {
                payments = payments.Where(p => p.Tax >= paytaxfrom);
            }
            if (paytaxto != null)
            {
                payments = payments.Where(p => p.Tax <= paytaxto);
            }
            if (paytotalfrom != null)
            {
                payments = payments.Where(p => p.Total >= paytotalfrom);
            }
            if (paytotalto != null)
            {
                payments = payments.Where(p => p.Total <= paytotalto);
            }
            if (paydatefrom != null)
            {
                payments = payments.Where(p => p.PaymentDate >= paydatefrom);
            }
            if (paydateto != null)
            {
                payments = payments.Where(p => p.PaymentDate <= paydateto);
            }
            if (!String.IsNullOrEmpty(paycustom))
            {
                payments = payments.Where(p => p.Cart.Customer.Name.Contains(paycustom));
            }
            if (paycart != null)
            {
                payments = payments.Where(p => p.CartId == paycart);
            }
            ViewBag.Payments = payments.ToList();

            if (revdatefrom != null)
            {
                reviews = reviews.Where(p => p.Date >= revdatefrom);
            }
            if (revdateto != null)
            {
                reviews = reviews.Where(p => p.Date <= revdateto);
            }
            if (!String.IsNullOrEmpty(revcustom))
            {
                reviews = reviews.Where(p => p.Customer.Name.Contains(revcustom));
            }
            if (!String.IsNullOrEmpty(revtitle))
            {
                reviews = reviews.Where(p => p.Title.Contains(revtitle));
            }
            if (!String.IsNullOrEmpty(revfeed))
            {
                reviews = reviews.Where(p => p.Feedback.Contains(revfeed));
            }
            ViewBag.Reviews = reviews.ToList();

            if (!String.IsNullOrEmpty(shipcar))
            {
                shippings = shippings.Where(p => p.ShippingCarrier == shipcar);
            }
            if (!String.IsNullOrEmpty(shiploc))
            {
                shippings = shippings.Where(p => p.Location.Contains(shiploc));
            }
            if (!String.IsNullOrEmpty(shipstat))
            {
                shippings = shippings.Where(p => p.Status == shipstat);
            }
            if (shipexpfrom != null)
            {
                shippings = shippings.Where(p => p.ExpectedDate >= shipexpfrom);
            }
            if (shipexpto != null)
            {
                shippings = shippings.Where(p => p.ExpectedDate <= shipexpto);
            }
            if (shiparrfrom != null)
            {
                shippings = shippings.Where(p => p.ArrivalDate >= shiparrfrom);
            }
            if (shiparrto != null)
            {
                shippings = shippings.Where(p => p.ArrivalDate <= shiparrto);
            }
            ViewBag.Shippings = shippings.ToList();

            if (!String.IsNullOrEmpty(specram))
            {
                specs = specs.Where(p => p.Ram == specram);
            }
            if (!String.IsNullOrEmpty(specstorage))
            {
                specs = specs.Where(p => p.Storage == specstorage);
            }
            if (!String.IsNullOrEmpty(speccamera))
            {
                specs = specs.Where(p => p.Camera == speccamera);
            }
            if (!String.IsNullOrEmpty(specdisplay))
            {
                specs = specs.Where(p => p.Display == specdisplay);
            }
            ViewBag.Specs = specs.ToList();

            if (!String.IsNullOrEmpty(suppbrand))
            {
                suppliers = suppliers.Where(p => p.BrandName.Contains(suppbrand));
            }
            ViewBag.Suppliers = suppliers.ToList();

            if (!String.IsNullOrEmpty(wishprod))
            {
                wishlists = wishlists.Where(p => p.Product.Name.Contains(wishprod));
            }
            if (!String.IsNullOrEmpty(wishcust))
            {
                wishlists = wishlists.Where(p => p.Customer.Name.Contains(wishcust));
            }
            ViewBag.WishLists = wishlists.ToList();

            return View();
        }
    }
}