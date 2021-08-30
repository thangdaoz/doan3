using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using STATIONERY_MANAGE.Models;

namespace STATIONERY_MANAGE.Controllers
{
  
    public class CompanyController : Controller
    {
        // GET: Company
        Stationery_managementEntities db = new Stationery_managementEntities();
        public ActionResult Index(int? id )
        {
            if (Session["idUser"] != null)
            {
                company company = db.companies.Find(id = 1);
                return View(company);
            }
            
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult index([Bind(Include = "service_charge_value,vat_charge_value,adress,phone,country,messege")] company company)
        {
            if (ModelState.IsValid)
            {
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                
            }
            return View(company);
        }
    }
}
