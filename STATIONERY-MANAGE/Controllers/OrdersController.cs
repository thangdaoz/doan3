using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using STATIONERY_MANAGE.Models;
using System.Net;
using System.Data.Entity;

namespace STATIONERY_MANAGE.Controllers
{
    public class OrdersController : Controller
    {
        Stationery_managementEntities db = new Stationery_managementEntities();
        // GET: Orders
        public ActionResult Index()
        {
            return View(db.orders.ToList());
        }

        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id, bill_no,customer_name,customer_address, customer_phone,date_time,gross_amount, sevice_charge_rate, service_charge,vat_charge_rate, vat_charge, vat_amount, discount,paid_status, user_id")] order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(order);
        }
        public ActionResult create()
        {
            List<product> productlist = db.products.Where(x => x.availability == 1).ToList();
            ViewBag.listproduct = productlist;
            
            return View();
        }
        [HttpPost]
        public JsonResult createorder(List<orders_item> orders_items, order order)
        {
            order.date_time = DateTime.Now.ToString();
            order.bill_no = "zxc";
            order.paid_status = 1;
            order.user_id = 1;
            order.company_id = 1;
            db.orders.Add(order);

            db.SaveChanges();

            foreach (orders_item orders_item in orders_items)
            {
                orders_item.order_id = order.id;
                db.orders_item.Add(orders_item);
                db.SaveChanges();
            }


            return Json(new { messege = "OK" });
        }
        public void createOrderItem(List<orders_item> orders_items, int id)
        {
            foreach (orders_item orders_item in orders_items)
            {
                orders_item.order_id = id;
                db.orders_item.Add(orders_item);
                db.SaveChanges();
            }
        }



       public JsonResult Getid (int id)
        {
            product product = db.products.Find(id);

            ProductViewmodel item = new ProductViewmodel();
            item.id = id;
            item.name = product.name;
            item.price = product.price;
            item.sku = product.sku;
            
            return Json(new { data = item }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Getcompany()
        {
            
            company company = db.companies.Find(1);
            feeview item = new feeview();
            item.service_charge_value = company.service_charge_value;
            item.vat_charge_value = company.vat_charge_value;

            return Json(new { data = item }, JsonRequestBehavior.AllowGet);
        }
        
    }
}