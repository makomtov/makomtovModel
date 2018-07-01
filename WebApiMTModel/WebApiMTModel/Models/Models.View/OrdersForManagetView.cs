using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiMTModel.Models.Models.View
{
    public class OrdersForManagetView
    {
        //list of orders
        [JsonProperty("UserReservations")]
        public List<OrderDetailsViewManager> UserReservations { get; set; }

        public OrdersForManagetView()
        {
            UserReservations = new List<OrderDetailsViewManager>();
        }
    }
}