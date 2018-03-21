using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;
using Newtonsoft.Json;

/// <summary>
/// Summary description for DogsInOrder
/// </summary>
namespace WebApiMTModel.Models.Models.View
{
    
    public class DogsInOrderView : DogDetailsView
    {

        public DogsInOrderView()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        [JsonProperty("Training")]
        public bool Training { get; set; } //אילוף? כן/לא
        [JsonProperty("Pension")]
        public bool Pension { get; set; }
        [JsonProperty("FromDate")]
        public DateTime FromDate { get; set; }   //תאריך השהייה בפנסיון
        [JsonProperty("ToDate")]
        public DateTime ToDate { get; set; }
        [JsonProperty("Price")]
        public decimal Price { get; set; }
        [JsonProperty("HomeFood")]
        public bool HomeFood { get; set; }  //אוכל בית/פנסיון?
        [JsonProperty("ShiftNumberFrom")]
        public int ShiftNumberFrom { get; set; }// 1 - בוקר 7-12
        [JsonProperty("ShiftNumberTo")]
        public int ShiftNumberTo { get; set; } // 2 - ערב 16-19
    }
}