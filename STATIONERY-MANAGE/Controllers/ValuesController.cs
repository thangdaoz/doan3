using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using STATIONERY_MANAGE.Models;


using System.Security.Claims;

namespace STATIONERY_MANAGE.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<product> Get()
        {
            using (Stationery_managementEntities db = new Stationery_managementEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                return db.products.ToList();
            }
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
