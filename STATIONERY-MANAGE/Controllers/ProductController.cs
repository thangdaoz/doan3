using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using STATIONERY_MANAGE.Models;
using System.Net;

using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace STATIONERY_MANAGE.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        Stationery_managementEntities db = new Stationery_managementEntities();
        // GET: Product
        public ActionResult Index()
        {
            var product = db.products.Include(c => c.category).Include(b => b.store);
            return View(product.ToList());
        }
        public ActionResult edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            product product = db.products.Find(id);
            
            TempData["image"] = product.image;
            if (product == null)
            {
                return HttpNotFound();
            }
            categoriesDropDownList(product.category_id);
            storeDropDownList(product.store_id);
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult edit([Bind(Include = "id , name , sku, price, qty, description, store_id, availability, category_id")] product product, HttpPostedFileBase images)
        {
            if (ModelState.IsValid)
            {
                if (images!=null)
                {
                    string path = Server.MapPath("/Content/images/product_image");
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    images.SaveAs(path + "/" + images.FileName);
                    product.image = "Content/images/product_image/" + images.FileName;

                    db.Entry(product).State = EntityState.Modified;


                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    product.image = TempData["image"].ToString();
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("index");
                }



            }
            return View(product);
        }
        public ActionResult create()
        {
            categoriesDropDownList();
            storeDropDownList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult create([Bind(Include = "id , name , sku, price, qty, description, store_id, availability, category_id")] product product , HttpPostedFileBase images)
        {
            
            try
            {
                if (ModelState.IsValid)
                {
                    string path = Server.MapPath("/Content/images/product_image");
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    images.SaveAs(path + "/" + images.FileName);
                    product.image = "Content/images/product_image/" + images.FileName;


                    db.products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            categoriesDropDownList(product.category_id);
            storeDropDownList(product.store_id);
            return View(product);
        }
        

        public ActionResult addimage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadMulti(List<HttpPostedFileBase> uploadFile, product_image product_Image , int product_id)
        {
            if (ModelState.IsValid)
            {
                string path = Server.MapPath("/Content/images/product_image");
                foreach (var item in uploadFile)
                {
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    item.SaveAs(path + "/" + item.FileName);
                    product_Image.image_product = "Content/images/product_image/" + item.FileName;
                    product_Image.product_id = product_id;
                    db.product_image.Add(product_Image);
                }
                return RedirectToAction("index");
            }
            return View();
        }
        private void categoriesDropDownList(object selectedcategories = null)
        {
            var categories = from d in db.categories
                       orderby d.name 
                       select d;
            ViewBag.category_id = new SelectList(categories, "id", "name", selectedcategories);
        }
        private void storeDropDownList(object selectedstore = null)
        {
            var stores = from d in db.stores
                         orderby d.name
                         select d;
            ViewBag.store_id = new SelectList(stores, "id", "name", selectedstore);
        }

    }
}