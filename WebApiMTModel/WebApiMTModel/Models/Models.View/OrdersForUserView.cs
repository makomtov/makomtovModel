using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiMTModel.Models.Models.View
{
    public class OrdersForUserView
    {
        //list of orders
        [JsonProperty("UserReservations")]
        public List<OrderDetailsView> UserReservations { get; set; }

        public OrdersForUserView()
        {
            UserReservations = new List<OrderDetailsView>();
        }
    }
}