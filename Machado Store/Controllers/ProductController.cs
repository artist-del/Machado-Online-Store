using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Machado_Store.Models;
using System.IO;
using Newtonsoft.Json;

namespace Machado_Store.Controllers
{
    public class ProductController : Controller
    {
        machadoEntities db = new machadoEntities();
        // GET: Product
        public ActionResult Index()
        {
            var list = db.tbl_product.ToList();
            return View(list);
        }

        public JsonResult Create(tbl_product prod)
        {
            try
            {
                string filename = Path.GetFileNameWithoutExtension(prod.ImageUpload.FileName);
                string extension = Path.GetExtension(prod.ImageUpload.FileName);
                prod.product_img = prod.tbl_productName + extension;
                prod.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFile"), prod.product_img));

                db.tbl_product.Add(prod);
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
                    status = false
                });
            }
            
        }

        public JsonResult GetProductId(int id)
        {
            var result = db.tbl_product.Where(x => x.product_Id == id).SingleOrDefault();

            string value = string.Empty;

            value = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Update(tbl_product pd)
        {
            try
            {
                var entity = db.tbl_product.Where(x => x.product_Id == pd.product_Id).FirstOrDefault();

                if(entity != null)
                {
                    if(pd.ImageUpload != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(pd.ImageUpload.FileName);
                        string extension = Path.GetExtension(pd.ImageUpload.FileName);
                        pd.product_img = pd.tbl_productName + extension;
                        pd.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFile"), pd.product_img));

                        entity.tbl_productName = pd.tbl_productName;
                        entity.tbl_price = pd.tbl_price;
                        entity.tbl_description = pd.tbl_description;
                        entity.tbl_quantity = pd.tbl_quantity;
                        entity.product_img = pd.product_img;
                        entity.tbl_wholesale = pd.tbl_wholesale;

                        db.SaveChanges();

                        return Json(new
                        {
                            status = true
                        });
                    }
                    entity.tbl_productName = pd.tbl_productName;
                    entity.tbl_price = pd.tbl_price;
                    entity.tbl_description = pd.tbl_description;
                    entity.tbl_quantity = pd.tbl_quantity;
                    entity.tbl_wholesale = pd.tbl_wholesale;

                    db.SaveChanges();
                    return Json(new
                    {
                        status = true
                    });
                }
                return Json(new
                {
                    status = false
                });
            }
            catch(Exception ex)
            {
                return Json(new
                {
                    status = false
                });
            }
        }

        public JsonResult Delete(int id)
        {
            try
            {
                var del = db.tbl_product.Where(x => x.product_Id == id).FirstOrDefault();

                if (del != null)
                {
                    db.tbl_product.Remove(del);
                    db.SaveChanges();

                    return Json(new
                    {
                        status = true
                    });
                }
                return Json(new
                {
                    status = false
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
    }
}