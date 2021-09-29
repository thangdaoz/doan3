using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using STATIONERY_MANAGE.Models;

namespace STATIONERY_MANAGE.Controllers
{
    public class ProductsController : ApiController
    {
        private Stationery_managementEntities db = new Stationery_managementEntities();
        public HttpResponseMessage Get()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return Request.CreateResponse(HttpStatusCode.OK , db.products.ToList());
        }
        public HttpResponseMessage Get(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            if (id == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            return Request.CreateResponse(HttpStatusCode.OK, product);
        }
        public HttpResponseMessage getimage(int id)
        {
            if (id == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var product_image = db.product_image.Where(x => x.product_id == id);
            return Request.CreateResponse(HttpStatusCode.OK, product_image);
        }

     }
}
