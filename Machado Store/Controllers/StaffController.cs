using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machado_Store.Models;
using Newtonsoft.Json;

namespace Machado_Store.Controllers
{
    public class StaffController : Controller
    {
        machadoEntities db = new machadoEntities();
        // GET: Staff
        public ActionResult Index()
        {
            var list = db.tbl_staff.ToList();
            return View(list);
        }

        public JsonResult Create(tbl_staff ts)
        {
            try
            {
                db.tbl_staff.Add(ts);
                db.SaveChanges();

                return Json(new
                {
                    status = true
                });
            }
            catch(Exception ex)
            {
                return Json(new
                {
                    status = false,
                    msg = ex
                });
            }
            
        }

        public JsonResult GetStaffId(int id)
        {
            try
            {
                var entity = db.tbl_staff.Where(x => x.Id == id).SingleOrDefault();

                string value = string.Empty;

                value = JsonConvert.SerializeObject(entity, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                return Json(value, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new
                {
                    status = false,
                    msg = ex
                });
            }
        }

        [HttpPost]
        public JsonResult Update(tbl_staff ts)
        {
            var entity = db.tbl_staff.Where(x => x.Id == ts.Id).SingleOrDefault();

            if(entity != null)
            {
                entity.tbl_fname = ts.tbl_fname;
                entity.tbl_mname = ts.tbl_mname;
                entity.tbl_lname = ts.tbl_lname;
                entity.tbl_email = ts.tbl_email;
                entity.tbl_username = ts.tbl_username;
                entity.tbl_password = ts.tbl_password;
                db.SaveChanges();

                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var del = db.tbl_staff.Where(x => x.Id == id).FirstOrDefault();

            if(del != null)
            {
                db.tbl_staff.Remove(del);
                db.SaveChanges();

                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
        }
    }
}