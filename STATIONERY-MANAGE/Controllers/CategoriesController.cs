using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using STATIONERY_MANAGE.Models;
namespace STATIONERY_MANAGE.Controllers
{
    public class CategoriesController : ApiController
    {
        private Stationery_managementEntities db = new Stationery_managementEntities();


        public HttpResponseMessage Get()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return Request.CreateResponse(HttpStatusCode.OK, db.categories.ToList());
        }
        public HttpResponseMessage Get(int id)
        {


            var product = db.products.Where(x => x.category_id == id);
            return Request.CreateResponse(HttpStatusCode.OK, product );
        }

    }
}
