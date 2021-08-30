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
            if (Session["idUser"] != null)
            {
                return View(db.categories.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }

        }
        //get: create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create([Bind(Include = " name, active")] category category)
        {

            db.categories.Add(category);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        public ActionResult Edit(int? id)
        {
            
            category category = db.categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id, name, active")] category category)
        {
            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult delete(int id)
        {
            category category = db.categories.Find(id);
            db.categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
