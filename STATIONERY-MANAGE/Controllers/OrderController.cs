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
    public class OrderController : ApiController
    {
        private Stationery_managementEntities db = new Stationery_managementEntities();
        public IHttpActionResult Post(List<orders_item> orders_items, order order)
        {
            try{
                var identity = (ClaimsIdentity)User.Identity;
                var userid = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                                            .Select(c => c.Value);
                order.date_time = DateTime.Now.ToString();
                order.bill_no = "zxc";
                order.paid_status = 1;

                order.user_id = Int32.Parse(userid.FirstOrDefault());
                order.company_id = 1;
                db.orders.Add(order);

                db.SaveChanges();

                foreach (orders_item orders_item in orders_items)
                {
                    orders_item.order_id = order.id;
                    db.orders_item.Add(orders_item);
                    db.SaveChanges();
                }


                return Ok(order);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
