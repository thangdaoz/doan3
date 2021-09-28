using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data;
using STATIONERY_MANAGE.Models;

namespace STATIONERY_MANAGE.Controllers
{
    [Authorize]
    public class StoreController : Controller
    {
        Stationery_managementEntities db = new Stationery_managementEntities();
        // GET: Store
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Getdata()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var result = db.stores.ToList();
            return Json(new { Data = result, TotalItems = result.Count }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult create(store store)
        {
            db.stores.Add(store);
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
            store item = db.stores.Find(id);
            return Json(new { data = item }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult update(store store)
        {

            db.Entry(store).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            store store = db.stores.Find(id);
            db.stores.Remove(store);
            var rs = db.SaveChanges();
            if (rs > 0)
            {
                return Json(new { Success = true });
            }

            return Json(new { Success = false });
        }
    }
}