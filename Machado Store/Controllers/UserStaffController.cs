using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machado_Store.Models;

namespace Machado_Store.Controllers
{
    public class UserStaffController : Controller
    {
        machadoEntities db = new machadoEntities();
        // GET: UserSfaff
        public ActionResult Index()
        {
            if(Session["staff"] == null)
            {
                return RedirectToAction("Login");
            }
            var count = db.tbl_product.ToList();
            ViewBag.product = count.Count();
            
            ViewBag.orders = db.order_list.Count();

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var entity = db.tbl_staff.Where(x => x.tbl_username == username && x.tbl_password == password).FirstOrDefault();
             if(entity != null)
            {
                Session["staff"] = entity.tbl_fname;
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Product()
        {
            if (Session["staff"] == null)
            {
                return RedirectToAction("Login");
            }
            var list = db.tbl_product.ToList();
            return View(list);
        }

        public ActionResult OrderList()
        {
            if (Session["staff"] == null)
            {
                return RedirectToAction("Login");
            }
            var list = db.order_list.GroupBy(x => x.customer_Id).Select(group => group.FirstOrDefault());
            

            return View(list);
        }

        public ActionResult viewOrder(int id)
        {
            if (Session["staff"] == null)
            {
                return RedirectToAction("Login");
            }
            var list = db.order_list.Where(x => x.customer_Id == id).ToList();

            return View(list);
        }

        public ActionResult Delete(int id)
        {
            var entity = db.order_list.Where(x => x.Id == id).SingleOrDefault();

            if(entity != null)
            {
                db.order_list.Remove(entity);
                db.SaveChanges();
            }

            return RedirectToAction("OrderList");
        }

        public ActionResult Confirm(int id)
        {
            var confirm = db.order_list.Where(x => x.Id == id).FirstOrDefault();

            if (confirm != null)
            {
                confirm.tbl_status = "Confirmed";
                db.SaveChanges();

                return RedirectToAction("viewOrder/" + confirm.customer_Id);
            }
            else
            {
                return RedirectToAction("OrderList");
            }
        }

        public ActionResult UnConfirm(int id)
        {
            var confirm = db.order_list.Where(x => x.Id == id).FirstOrDefault();

            if (confirm != null)
            {
                confirm.tbl_status = "Pending";
                db.SaveChanges();

                return RedirectToAction("viewOrder/" + confirm.customer_Id);
            }
            else
            {
                return RedirectToAction("OrderList");
            }
        }

        public ActionResult Print()
        {
           
            var list = db.tbl_product.ToList();
            return View(list);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}