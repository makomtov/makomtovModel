using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using WebApiMTModel.Models;
using WebApiMTModel.Models.Models.View;
using System.Web.Http.Results;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http.Description;
using System.Threading.Tasks;
using System.Text;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;
using Newtonsoft.Json.Linq;

namespace WebApiMTModel.Controllers
{
    [System.Web.Http.RoutePrefix("api/Users")]
    public class

        UsersController : ApiController
    {
        //api/Users/GetUsers

        [System.Web.Http.Route("")]
        public IEnumerable<UserDetailsView> GetUsers()
        {
            Userservice userservice = new Userservice();
            IEnumerable<UserDetailsView> users = userservice.GetUsers();
            return users;
        }



        // /api/Users/aaa@gmail.com/1234
        [System.Web.Http.Route("{usereMail}/{password}")]

        public UserDetailsView GetUser(string usereMail, string password)
        {
            Userservice userservice = new Userservice();
            return userservice.GetUser(usereMail, password);
           
        }
        // /api/Users/1
        [System.Web.Http.Route("{userid}")]

        public UserDetailsView GetUser(int userid)
        {
            Userservice userservice = new Userservice();
            return userservice.GetUser(userid);

        }

        // /api/Users/GetUserDogs/1
        [System.Web.Http.Route("GetUserDogs/{userid}")]
        public List<DogDetailsView> GetUserDogs(int userid)

        {
            Userservice userservice = new Userservice();
            return userservice.GetUserDogs(userid);

        }
       
        // /api/Users/InsertUserDetails
        [System.Web.Http.Route("InsertUserDetails")]
        [System.Web.Http.HttpPut]
        public void InsertUserDetails(HttpRequestMessage userNew)
        {
            var jsonString = userNew.Content.ReadAsStringAsync().Result;

            UserDetailsView user = JsonConvert.DeserializeObject<UserDetailsView>(jsonString);
            Userservice userservice = new Userservice();
            userservice.InsertUserDetails(user);
        
        }

        [System.Web.Http.Route("UpdateUserDogsByManager")]
        [System.Web.Http.HttpPut]
        public void UpdateUserDogsByManager(HttpRequestMessage userDogs)
        {
            var jsonString = userDogs.Content.ReadAsStringAsync().Result;
            UserDetailsView user = JsonConvert.DeserializeObject<UserDetailsView>(jsonString);
            //  UserDetailsView user = JsonConvert.DeserializeObject<UserDetailsView>(jsonString);
            Userservice userservice = new Userservice();
            userservice.UpdateDogsByManager(user);
        }
        //// public void InsertUserDetails(JObject juser)
        //public void InsertUserDetails([FromBody] UserDetailsView user)
        //{
        //    // UserDetailsView user = JsonConvert.DeserializeObject<UserDetailsView>(juser.ToString());
        //    Userservice userservice = new Userservice();
        //    userservice.InsertUserDetails(user);
        //}

        /// <summary>
        /// add dogs for user
        /// </summary>
        /// <param name="userDetails"></param>
        /// 
        //// /api/Users/user/AddDogs
        //[System.Web.Http.HttpPost]
        //[System.Web.Http.Route("{user/AddDogs}")]
        //public void AddDogsForUser(UserDetailsView userDetails)
        //{
        //    Userservice userservice = new Userservice();
        //    userservice.InsertUserDetails(userDetails);
        //}
    }
}

