using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiMTModel.Models.Models.View
{
    public class DogsForManagerView
    {
        //list of dogs
        [JsonProperty("UserDogs")]
        public List<DogDetailsViewManager> UserDogs { get; set; }

        public DogsForManagerView()
        {
            UserDogs = new List<DogDetailsViewManager>();
        }
    }
}