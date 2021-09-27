using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STATIONERY_MANAGE.Models
{
    public class ProductViewmodel
    {
        public int id { get; set; }

        public string name { get; set; }

         public string sku { get; set; }
        public string price { get; set; }
    }
}