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
        //[JsonProperty("DogNumber")]
        //public int DogNumber { get; set; }
        [JsonProperty("OrderNumber")]
        public int OrderNumber { get; set; }
        [JsonProperty("Training")]
        public bool Training { get; set; } //אילוף? כן/לא
        [JsonProperty("Pension")]
        public bool Pension { get; set; }
        [JsonProperty("HomeFood")]
        public bool HomeFood { get; set; }  //אוכל בית/פנסיון?
        [JsonProperty("Status")]
        public int Status { get; set; }   //( ב21 - בהזמנה, 23-לא הגיע/בוטל)
    }
}