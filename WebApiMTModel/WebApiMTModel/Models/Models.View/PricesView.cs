using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiMTModel.Models.Models.View
{
    public class PricesView
    {
        public int Id { get; set; }
        public int Days { get; set; }
        public int Dogs { get; set; }
        public decimal Price { get; set; }
    }
}