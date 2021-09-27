using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STATIONERY_MANAGE.Models
{
    public class UserView
    {
        


        public int userid { get; set; }
        public string password { get; set; }

        public string email { get; set; }

        public string firstname { get; set; }

        public string lastname { get; set; }

        public string phone { get; set; }


        public int user_role_id { get; set; }
        public int roles_id { get; set; }
    }
}