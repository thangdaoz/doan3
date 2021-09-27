using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data;
using STATIONERY_MANAGE.Models;
using System.Web.Security;
using System.Net;

namespace STATIONERY_MANAGE.Controllers
{
    

    public class UserController : Controller
    {
        Stationery_managementEntities db = new Stationery_managementEntities();
        // GET: User
        public ActionResult Index()
        {
            return View(db.users.ToList());
        }

        public ActionResult Create()
        {
            List<role> roles = db.roles.ToList();
            ViewBag.getroles = roles;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id , email , password, firstname, lastname, phone")] user user, int role_id)
        {
            if (ModelState.IsValid)
            {
                db.users.Add(user);
                db.SaveChanges();
                users_roles users_Roles = new users_roles();
                users_Roles.users_id = user.id;
                users_Roles.roles_id = role_id;
                db.users_roles.Add(users_Roles);
                db.SaveChanges();
                return RedirectToAction("Index", "User");
            }
            return View(user);
        }
        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            user user = db.users.Find(id);
            db.users.Remove(user);
            users_roles users_Roles = db.users_roles.Where(x => x.users_id == id).FirstOrDefault();
            db.users_roles.Remove(users_Roles);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult login(user user)
        {
            using(Stationery_managementEntities db = new Stationery_managementEntities())
            {
                bool isvalid = db.users.Any(x => x.email == user.email && x.password == user.password);
                var data = db.users.Where(x => x.email == user.email && x.password == user.password).ToList();
                if (isvalid)
                {
                    
                    FormsAuthentication.SetAuthCookie(user.email, false);
                    Session["idUser"] = data.FirstOrDefault().id;
                    return RedirectToAction("Index", "Category");
                }
            }    
            return View();

        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.users.Find(id);
            var userrole = db.users_roles.Where(x => x.users_id == id).FirstOrDefault();
            UserView model = new UserView()
            {
                userid = user.id,
                email = user.email,
                firstname = user.firstname,
                lastname = user.lastname,
                password = user.password,
                phone = user.phone,
                user_role_id = userrole.id,
                roles_id = userrole.roles_id

            };


            List<role> roles = db.roles.ToList();
            ViewBag.getroles = roles;
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit([Bind(Include = "id , email , password, firstname, lastname, phone")] user user, int role_id, int user_roleid)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                users_roles users_Roles = new users_roles();
                users_Roles.id = user_roleid;
                users_Roles.users_id = user.id;
                users_Roles.roles_id = role_id;
                db.Entry(users_Roles).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "User");
            }
            return View();
        }
        public ActionResult profile()
        {
            string stringid = Session["idUser"].ToString();
            int id = Int32.Parse(stringid);
            var model = new UserView()
            {

            };
            user user = db.users.Find(id);
            return View(user);
        }
        public ActionResult setting()
        {
            string stringid = Session["idUser"].ToString();
            int id = Int32.Parse(stringid);
            user user = db.users.Find(id);
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult setting([Bind(Include = "id , email , password , firstname, lastname, phone")] user user)
        {
            if (ModelState.IsValid) {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");

            }

            return View(user);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("login", "User");
        }
    }
}