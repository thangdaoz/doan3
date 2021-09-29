using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using STATIONERY_MANAGE.Models;

namespace STATIONERY_MANAGE.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        // GET: Company
        Stationery_managementEntities db = new Stationery_managementEntities();
        [Authorize(Roles = "admin")]
        public ActionResult Index(int? id )
        {
            
            
                company company = db.companies.Find(id = 1);
                return View(company);
            
           
           
            
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult index([Bind(Include = "id,service_charge_value,vat_charge_value,address,phone,country,message")] company company)
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
