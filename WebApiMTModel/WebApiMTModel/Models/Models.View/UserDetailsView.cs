using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebApiMTModel.Models.Models.View
{
   
    public class UserDetailsView
    {
        public UserDetailsView()
        { }
        public UserDetailsView(int userid)
        { UserID = userid; }
        public UserDetailsView(int userid,string firstName,string lastName)
        { UserID = userid;
            UserFirstName = firstName;
            UserLastName = lastName;
        }
        //  [JsonProperty("userID")]
        int userID;
        [JsonProperty("UserID")]
        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        [JsonProperty("username")]
        public string UserName { get; set; }
        [JsonProperty("useremail")]
        public string UserEmail { get; set; }
        [JsonProperty("useraddress")]
        public string UserAddress { get; set; }
        [JsonProperty("userstatus")]
        public string UserStatus { get; set; }
        [JsonProperty("usercomments")]
        public string UserComments { get; set; }
        [JsonProperty("usercityname")]
        public string UserCityName { get; set; }
        [JsonProperty("userFirstName")]
        public string UserFirstName { get; set; }
        [JsonProperty("UserLastName")]
        public string UserLastName { get; set; }
        [JsonProperty("UserPaswrd")]
        public string UserPaswrd { get; set; }
        [JsonProperty("UserPhone1")]
        public string UserPhone1 { get; set; }
        [JsonProperty("UserPhone2")]
        public string UserPhone2 { get; set; }
        [JsonProperty("DogsNumber")]
        public int DogsNumber { get; set; }
        [JsonProperty("ReservationsNumber")]
        public int ReservationsNumber { get; set; }
        [JsonProperty("UserarrayDogs")]
        public List<DogDetailsView> UserarrayDogs { get; set; }
        [JsonProperty("UserReservations")]
        public List<OrderDetailsView> UserReservations { get; set; }
    }
        //  public ArrayList UserarrayDogs { get; set; }


    }

