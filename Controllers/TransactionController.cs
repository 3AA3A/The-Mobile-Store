using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobilesProject.Models;

namespace MobilesProject.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private MobilesDBEntities db = new MobilesDBEntities();
        // GET: Transaction
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CheckOut(int? id)
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

            if (cart.CartItems.Count() > 0)
            {
                return RedirectToAction("Create", "Payments", new { id = cart.Id });
                //cart.Status = "paid";
                //db.SaveChanges();
            }
            else
            {
                TempData["error"] = "Your cart is currently empty!";
            }
            return RedirectToAction("Details", "Carts", new { id = cart.Id });
        }

        public ActionResult Buy(int? id)
        {
            // copy error checking code from Product Details method
            // is there an id?
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);

            // did we find a product with that id?
            if (product == null)
            {
                return HttpNotFound();
            }

            // get unpaid Cart belonging to Customer with that username
            Cart cart = GetUsersCart();
            // search CartItems for existing CartItem for that cart and product
            
            try
            {
                // if found, increment quantity
                CartItem cartitem = db.CartItems.First(c => (c.CartId == cart.Id &&
               c.ProductId == product.Id));
                cartitem.Quantity++;
            }
            catch (Exception)
            {
                // if not found, create it and add it
                CartItem cartitem = new CartItem
                {
                    ProductId = product.Id,
                    CartId = cart.Id,
                    Quantity = 1
                };
                db.CartItems.Add(cartitem);
            }

            try
            {
                var inven = db.Inventories.First(x => x.ProductId == product.Id);
                inven.Amount--;
            }
            catch (Exception)
            {
                //CartItem cartitem = new CartItem
                //{
                //    ProductId = product.Id,
                //    CartId = cart.Id,
                //    Quantity = 1
                //};
                //db.CartItems.Add(cartitem);
            }

            db.SaveChanges();

            Supplier supplier = product.Supplier;

            // send user to Cart detail view
            return RedirectToAction("Details", "Carts", new { id = cart.Id, suppid = supplier.Id });
        }

        private Cart GetUsersCart()
        {
            // 1 -- get logged-in user's username
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

            Cart cart = null;
            try
            {
                // 3 -- do we have an unpaid Cart belonging to Customer with that username?
                cart = db.Carts.First(c => c.CustomerName.Equals(username) &&
               c.Status.Equals("unpaid"));
            }
            catch (Exception)
            {
                // no we don't -- create one
                cart = new Cart { CustomerName = username, Status = "unpaid" };
                db.Carts.Add(cart);
                db.SaveChanges();
            }

            return cart;
        }

        public ActionResult MyCart()
        {
            // get user logged-in id + cart
            Cart cart = GetUsersCart();
            // go to that user's shopping cart page
            return RedirectToAction("Details", "Carts", new { id = cart.Id });
        }
    }
}