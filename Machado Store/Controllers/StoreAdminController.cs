using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machado_Store.Models;
using Newtonsoft.Json;

namespace Machado_Store.Controllers
{
    public class StoreAdminController : Controller
    {
        machadoEntities db = new machadoEntities();
        // GET: StoreAdmin
        public ActionResult Index()
        {
            if (Session["storeId"] == null)
            {
                return RedirectToAction("Login");
            }
            ViewBag.staff = db.tbl_staff.Count();
            ViewBag.orders = db.order_list.Count();
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(tbl_admin admin)
        {
            var entity = db.tbl_admin.Where(x => x.tbl_username == admin.tbl_username && x.tbl_password == admin.tbl_password).FirstOrDefault();

            if(entity != null)
            {
                Session["storeId"] = entity.tbl_fname;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.error = "error";
                return View();
            }
            
        }

        public ActionResult OrderList()
        {
            if (Session["storeId"] == null)
            {
                return RedirectToAction("Login");
            }
            var list = db.order_list.GroupBy(x => x.customer_Id).Select(group => group.FirstOrDefault());

            return View(list);
        }

        public ActionResult viewOrder(int id)
        {
            var list = db.order_list.Where(x => x.customer_Id == id).ToList();

            return View(list);
        }

        public ActionResult Delete(int id)
        {
            var entity = db.order_list.Where(x => x.Id == id).FirstOrDefault();

            if (entity != null)
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

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        public ActionResult Report(report rp)
        {
            if (Session["storeId"] == null)
            {
                return RedirectToAction("Login");
            }

            if (rp.month != null)
            {
                if(rp.select == "year")
                {
                    List<order_list> list2 = db.order_list.Where(x => x.tbl_date.Value.Year.Equals(rp.month.Value.Year)).ToList();

                    return View(list2);
                }
                List<order_list> list1 = db.order_list.Where(x => x.tbl_date.Value.Month.Equals(rp.month.Value.Month)).ToList();

                return View(list1);
            }

            List<order_list> list = db.order_list.ToList();
            return View(list);
        }

        public ActionResult Print()
        {

            var list = db.tbl_product.ToList();
            return View(list);
        }
    }
}