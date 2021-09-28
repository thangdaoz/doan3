using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using STATIONERY_MANAGE.Models;

namespace STATIONERY_MANAGE.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private Stationery_managementEntities db = new Stationery_managementEntities();
        // GET: Role
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult Getdata(int id)
        {
            var result = db.roles.ToList();
            return Json(new { Data = result, TotalItems = result.Count }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Getid(int id)
        {
            role item = db.roles.Find(id);
            return Json(new { data = item }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult create(role role)
        {
            db.roles.Add(role);
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
        [HttpPost]
        public ActionResult update(role role)
        {

            db.Entry(role).State = EntityState.Modified;
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