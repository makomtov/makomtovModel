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



        //api/Users/aaa@gmail.com/1234
        [System.Web.Http.Route("{usereMail}/{password}")]
      
        public UserDetailsView GetUser(string usereMail, string password)
        {
            Userservice userservice = new Userservice();
            return userservice.GetUser(usereMail, password);
            //string msg = JsonConvert.SerializeObject(userservice.GetUser(usereMail, password));
            //return msg;
            // return WebOperationContext.Current.CreateTextResponse(msg, "application/json; charset=utf-8", Encoding.UTF8);
        }
        //api/Users/GetUserOrders?id=1
        //[System.Web.Http.Route("Users/{userID}")]
        //

        // POST: /api/Users/PostUsersTbl/3
        [ResponseType(typeof(UserDetailsView))]
        public void PostUsersTbl(UserDetailsView user)
        {
           Userservice userservice = new Userservice();
            userservice.InsertUserDetails(user);
        }
        //public void InsertUserDetails(int id)
        //// public void InsertUserDetails(UserDetailsView user)
        //{
        //    UserDetailsView user = new UserDetailsView();
        //    user.UserAddress = "הרצל 5"; ;
        //    user.UserCityName = "תל אביב";
        //    user.UserComments = "";
        //    user.UserEmail = "bbb@gmail.com";
        //    user.UserFirstName = "דני";
        //    user.UserLastName = "כהן";
        //    user.UserName = "bbb";
        //    user.UserPaswrd = "1111";
        //    user.UserPhone1 = "23213213";
        //    user.UserPhone2 = "12321321";
        //    Userservice userservice = new Userservice();
        //    userservice.InsertUserDetails(user);
        //}
    }
}

