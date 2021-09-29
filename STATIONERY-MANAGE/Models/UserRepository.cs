using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STATIONERY_MANAGE.Models
{
    public class UserRepository : IDisposable
    {
        Stationery_managementEntities db = new Stationery_managementEntities();
        public user ValidateUser(string username ,string password)
        {
            return db.users.FirstOrDefault(user => user.email.Equals(username, StringComparison.OrdinalIgnoreCase)
            && user.password == password
            );

        }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}