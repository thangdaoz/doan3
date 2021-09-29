using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using STATIONERY_MANAGE.Models;

namespace STATIONERY_MANAGE.Controllers
{
    public class RegisterController : ApiController
    {
        private Stationery_managementEntities db = new Stationery_managementEntities();

        public HttpResponseMessage Post([FromBody] user user)
        {
            try
            {
                var check = db.users.FirstOrDefault(s => s.email == user.email);
                if (check == null)
                {
                    db.users.Add(user);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.Created);


                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
