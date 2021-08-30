using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace STATIONERY_MANAGE.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult edit()
        {
            return View();
        }
        public ActionResult create()
        {
            return View();
        }
    }
}