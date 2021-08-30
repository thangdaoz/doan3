using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using STATIONERY_MANAGE.Models;

namespace STATIONERY_MANAGE.Controllers
{
    public class StoreController : Controller
    {
        Stationery_managementEntities db = new Stationery_managementEntities();
        // GET: Store
        public ActionResult Index()
        {
            return View(db.stores.ToList());
        }

    }
}