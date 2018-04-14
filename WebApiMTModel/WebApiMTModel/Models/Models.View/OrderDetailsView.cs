using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

/// <summary>
/// Summary description for OrderDetails
/// </summary>
/// 
namespace WebApiMTModel.Models.Models.View
{
    public class OrderDetailsView
    {

        public OrderDetailsView()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        [JsonProperty("OrderNumber")]
        public int OrderNumber { get; set; }
        [JsonProperty("OrderDate")]
        public DateTime OrderDate { get; set; }
        [JsonProperty("User")]
        public UserDetailsView User { get; set; }
        [JsonProperty("Userid")]
        public int Userid { get; set; }
        [JsonProperty("userFirstName")]
        public string userFirstName { get; set; }
        [JsonProperty("userLastName")]
        public string userLastName { get; set; }
        [JsonProperty("Price")]
        public decimal Price { get; set; }
        [JsonProperty("OrderconfirmationNumber")]
        public string OrderconfirmationNumber { get; set; }
        [JsonProperty("OrderStatus")]
        public int OrderStatus { get; set; }
        [JsonProperty("OrderStatusName")]
        public string OrderStatusName { get; set; }
        [JsonProperty("OrderType")]
        public int OrderType { get; set; }
        [JsonProperty("OrderTypeName")]
        public string OrderTypeName { get; set; }
        [JsonProperty("FromDate")]
        public DateTime FromDate { get; set; }
        [JsonProperty("ShiftNumberFrom")]
        public int ShiftNumberFrom { get; set; }
        [JsonProperty("ToDate")]
        public DateTime ToDate { get; set; }
        [JsonProperty("ShiftNumberTo")]
        public int ShiftNumberTo { get; set; }
        [JsonProperty(propertyName: "Discount")]
        public decimal Discount { get; set; }
        [JsonProperty("mDogs")]
        public List<DogsInOrderView> mDogs { get; set; }
    }
}