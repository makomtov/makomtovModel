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
using FluentValidation;

namespace WebApiMTModel.Controllers
{
   
    [System.Web.Http.RoutePrefix("api/Users")]
    public class  UsersController : ApiController
    {

        //private readonly IValidator<UserDetailsView> userViewModelValidator;

        //public UsersController(IValidator<UserDetailsView> userViewModelValidator)
        //{
        //    this.userViewModelValidator = userViewModelValidator;
        //}
        //api/Users/GetUsers

        [System.Web.Http.Route("")]
        public IEnumerable<UserDetailsView> GetUsers()
        {
            Userservice userservice = new Userservice();
            IEnumerable<UserDetailsView> users = userservice.GetUsers();
            return users;
        }

        /// <summary>
        /// שליפת משתמש
        /// </summary>
        /// <param name="usereMail"></param>
        /// <param name="password"></param>
        /// <returns></returns>

        // /api/Users/GetLogInUser/ziris248@gmail.com/iris1234
        [System.Web.Http.Route("GetUser")]
        [System.Web.Http.HttpPost]
       
         public IHttpActionResult GetUser([FromBody]LoginView loginView)
     
        {
            try
            {
                
           
            Userservice userservice = new Userservice();
                UserDetailsView userDetailsView = userservice.GetUser(loginView.UserEmail, loginView.UserPassword);
            return Ok(userDetailsView);
        }
        
             catch
            {
               bool x= ModelState.IsValid;
                return InternalServerError();
    }

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
        [System.Web.Http.HttpPost]
        public IHttpActionResult InsertUserDetails(HttpRequestMessage userNew)
        {

            try
            {
                var jsonString = userNew.Content.ReadAsStringAsync().Result;
                UserDetailsView user = JsonConvert.DeserializeObject<UserDetailsView>(jsonString);
                Userservice userservice = new Userservice();
                userservice.InsertUserDetails(user);
                return Ok();
            }

            catch
            {
                return InternalServerError();
            }

        }

        [System.Web.Http.Route("UpdateUserDogsByManager")]
        [System.Web.Http.HttpPut]
        public void UpdateUserDogsByManager(HttpRequestMessage userDogs)
        {
            if (ModelState.IsValid)
            {
                var jsonString = userDogs.Content.ReadAsStringAsync().Result;
                UserDetailsView user = JsonConvert.DeserializeObject<UserDetailsView>(jsonString);
                //  UserDetailsView user = JsonConvert.DeserializeObject<UserDetailsView>(jsonString);
                Userservice userservice = new Userservice();
                userservice.UpdateDogsByManager(user);
            }
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
        private void ThrowResponseException(HttpStatusCode statusCode, string message)
        {
            var errorResponse = Request.CreateErrorResponse(statusCode, message);
            throw new HttpResponseException(errorResponse);
        }
    }
}

