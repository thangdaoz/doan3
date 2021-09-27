using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using STATIONERY_MANAGE.Models;

namespace STATIONERY_MANAGE.Controllers
{

    public class CategoryController : Controller
    {
        private Stationery_managementEntities db = new Stationery_managementEntities();
        // GET: Category
        public ActionResult Index()
        {
            
                return View();
           

        }
        public ActionResult Getdata()
        {
            var result = db.categories.ToList();
            return Json(new { Data = result, TotalItems = result.Count }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult create(category category)
        {
            db.categories.Add(category);
            try
            {
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Json(new { success = false });
            }
        }
        public ActionResult Getid(int id)
        {
            category item = db.categories.Find(id);
            return Json(new { data = item }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult update(category category)
        {

            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { success = true });
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            
            var category = db.categories.Find(id);
            db.categories.Remove(category);
            var rs = db.SaveChanges();
            if (rs > 0)
            {
                
                return Json(new { Success = true });
            }

            return Json(new { Success = false });
        }
    }
}
