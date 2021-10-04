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

    [Authorize]

    public class UserController : Controller
    {
        Stationery_managementEntities db = new Stationery_managementEntities();
        // GET: User
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            var users = db.users_roles.Include(c => c.user).Include(a => a.role);
            return View(users);
        }
        
        public ActionResult Create()
        {
            roleDropDownList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Create([Bind(Include = "id , email , password, comfirmpassword , firstname, lastname, phone")] user user, int RoleID)
        {
            roleDropDownList();
            if (ModelState.IsValid)
            {
                var check = db.users.FirstOrDefault(s => s.email == user.email);
                if (check == null)
                {
                    db.users.Add(user);
                    db.SaveChanges();
                    users_roles users_Roles = new users_roles();
                    users_Roles.users_id = user.id;
                    users_Roles.roles_id = RoleID;
                    db.users_roles.Add(users_Roles);
                    db.SaveChanges();
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ViewBag.error = "email already exist!! use another emmail please";
                    return View();
                }
            }
            return View(user);
        }

        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id)
        {
            
            user user = db.users.Find(id);
            db.users.Remove(user);
            users_roles users_Roles = db.users_roles.Where(x => x.users_id == id).FirstOrDefault();
            db.users_roles.Remove(users_Roles);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [AllowAnonymous]
        public ActionResult login()
        {
            if (Session["idUser"] != null)
            {
                return RedirectToAction("index", "category");

            }
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult login(user user)
        {
            
            
            using (Stationery_managementEntities db = new Stationery_managementEntities())
            {
                bool isvalid = db.users.Any(x => x.email == user.email && x.password == user.password);
                var data = db.users.Where(x => x.email == user.email && x.password == user.password).ToList();
                if (isvalid)
                {
                    
                    FormsAuthentication.SetAuthCookie(user.email, false);
                    Session["idUser"] = data.FirstOrDefault().id;
                    Session["email"] = data.FirstOrDefault().email;
                    return RedirectToAction("Index", "Category");
                }
            }    
            return View();

        }
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.users.Find(id);
            var userrole = db.users_roles.Where(x => x.users_id == id).FirstOrDefault();
            roleDropDownList(userrole.roles_id);
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


            
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Edit([Bind(Include = "id , email , password, firstname, lastname, phone")] user user, int RoleID, int user_role_id)
        {
            if (user != null)
            {
                 roleDropDownList(RoleID);
                 user.comfirmpassword = user.password;
                 db.Entry(user).State = EntityState.Modified;
                 db.SaveChanges();
                 users_roles users_Roles = new users_roles();
                 users_Roles.id = user_role_id;
                 users_Roles.users_id = user.id;
                 users_Roles.roles_id = RoleID;
                 db.Entry(users_Roles).State = EntityState.Modified;
                 db.SaveChanges();
                ViewBag.error = "error";

                return RedirectToAction("index", "user");
            }
            ViewBag.sucess = "sucessfull";
            return RedirectToAction("index", "user");
        }

        public ActionResult profile()
        {
            if (Session["idUser"] != null)
            {
                string stringid = Session["idUser"].ToString();
                int id = Int32.Parse(stringid);

                var userrole = db.users_roles.Where(x => x.users_id == id).Include(c => c.user).Include(a => a.role);

                return View(userrole.ToList());
            }
            return View();
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
        public ActionResult setting([Bind(Include = "id , email , password, comfirmpassword , firstname, lastname, phone")] user user , string newpassword)
        {
            if (ModelState.IsValid) {

                bool isvalid = db.users.Any(x => x.id == user.id && x.password == user.password && x.comfirmpassword == user.comfirmpassword);
                if (isvalid)
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    user.password = newpassword;
                    user.comfirmpassword = newpassword;

                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                
                }
                
                else
                {
                    return View(user);
                }
            }

            return View(user);
        }
        private void roleDropDownList(object selectedrole = null)
        {
            var role = from d in db.roles
                       orderby d.name
                       select d;
            ViewBag.RoleID = new SelectList(role, "id", "name", selectedrole);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("login", "User");
        }
    }
}