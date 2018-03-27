using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiMTModel.Models.Models.View
{
    public class OrderDetailsViewManager:OrderDetailsView
    {
        [JsonProperty("ManagerComments")]
        public string ManagerComments { get; set; } //הערות מנהל הכלביה
    }
}