﻿using FluentValidation.Attributes;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebApiMTModel.Models.Models.View
{
    [Validator(typeof(UserValidator))]
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
        [JsonProperty("UserName")]
        public string UserName { get; set; }
        [JsonProperty("UserEmail")]
        public string UserEmail { get; set; }
        [JsonProperty("UserAddress")]
        public string UserAddress { get; set; }
        [JsonProperty("UserStatus")]
        public string UserStatus { get; set; }
        [JsonProperty("UserStatusCode")]
        public int UserStatusCode { get; set; }
        [JsonProperty("UserComments")]
        public string UserComments { get; set; }
        [JsonProperty("UserCity")]
        public string UserCity { get; set; }
        [JsonProperty("UserFirstName")]
        public string UserFirstName { get; set; }
        [JsonProperty("UserLastName")]
        public string UserLastName { get; set; }
        [JsonProperty("UserPaswrd")]
        public string UserPaswrd { get; set; }
        [JsonProperty("UserPhone1")]
        public string UserPhone1 { get; set; }
        [JsonProperty("UserPhone2")]
        public string UserPhone2 { get; set; }
        [JsonProperty("DaysSumForDiscount")]
        public int DaysSumForDiscount { get; set; }
        [JsonProperty("Acceptmessages")]
        public bool Acceptmessages { get; set; }
        [JsonProperty("UserVeterinarId")]
        public int UserVeterinarId { get; set; }
        [JsonProperty("VeterinarName")]
        public string VeterinarName { get; set; }
        [JsonProperty("VeterinarEmail")]
        public string VeterinarEmail { get; set; }
        [JsonProperty("VeterinarAddress")]
        public string VeterinarAddress { get; set; }
        [JsonProperty("VeterinarCity")]
        public string VeterinarCity { get; set; }
        [JsonProperty("VeterinarPhone1")]
        public string VeterinarPhone1 { get; set; }
      
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

