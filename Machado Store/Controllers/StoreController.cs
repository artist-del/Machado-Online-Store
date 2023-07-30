using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machado_Store.Models;

namespace Machado_Store.Controllers
{
    public class StoreController : Controller
    {
        machadoEntities db = new machadoEntities();
        // GET: Store
        public ActionResult Index()
        {
            ViewBag.list = db.tbl_product.ToList();
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(tbl_customer cus)
        {
            var entity = db.tbl_customer.Where(x => x.tbl_email.Equals(cus.tbl_email) && x.tbl_password.Equals(cus.tbl_password)).FirstOrDefault();

            if (entity != null)
            {
                Session["customerId"] = entity.customer_Id;
                Session["customerName"] = entity.tbl_username;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.msg = "error";
                return View();
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(tbl_customer cus)
        {
            try
            {
                db.tbl_customer.Add(cus);
                db.SaveChanges();

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ViewBag.error = ex;
                return View();
            }

        }

        public ActionResult Process(int? id)
        {
            var entity = db.tbl_product.Where(x => x.product_Id == id).FirstOrDefault();

            if (entity != null)
            {
                ViewBag.product_Id = entity.product_Id;
                ViewBag.productName = entity.tbl_productName;
                ViewBag.description = entity.tbl_description;
                ViewBag.quantity = 1;
                ViewBag.retail = entity.tbl_price;
                ViewBag.whole = entity.tbl_wholesale;
            }

            return View();
        }

        [HttpPost]
        public ActionResult Process(order_list list)
        {
            var entity = db.tbl_product.Where(x => x.product_Id == list.product_Id).FirstOrDefault();

            if(list.tbl_quantity > entity.tbl_quantity)
            {
                ViewBag.msg = "error";
                return View();
            }
            else
            {
                entity.tbl_quantity -= list.tbl_quantity;
                list.tbl_price *= list.tbl_quantity;

                db.order_list.Add(list);
                db.SaveChanges();
                return RedirectToAction("Order");
            }

            
        }

        public ActionResult Order()
        {
            if (Session["customerId"] == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                int id = (int)Session["customerId"];
                var list = db.order_list.Where(x => x.customer_Id == id).ToList();

                return View(list);
            }

        }

        public ActionResult Cancel(int id, int product, int quantity)
        {
            var entity = db.order_list.Where(x => x.Id == id).FirstOrDefault();

            entity.tbl_product.tbl_quantity += quantity;

            db.order_list.Remove(entity);
            db.SaveChanges();

            return RedirectToAction("Order");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}