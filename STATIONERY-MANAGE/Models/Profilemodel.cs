using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STATIONERY_MANAGE.Models
{
    public class Profilemodel
    {
        public user user { get; set; }
        public users_roles users_roles { get; set;}

        public role role { get; set; } 

    }
}