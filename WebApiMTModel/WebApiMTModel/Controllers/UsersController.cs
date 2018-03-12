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

namespace WebApplicationMTController.Controllers
{
    public class

        UsersController : ApiController
    {
        //api/Users/GetUsers
        // public IEnumerable<UserDetailsView> GetUsers()

        public IEnumerable<UserDetailsView> GetUsers()
        {
            Userservice userservice = new Userservice();
            IEnumerable<UserDetailsView> users = userservice.GetUsers();
            return users;
        }



        //api/Users/GetUser?usereMail=a&password=b

        public UserDetailsView GetUser(string usereMail, string password)
        {
            Userservice userservice = new Userservice();
            return userservice.GetUser(usereMail, password);
        }
        /// POST: /api/Users/PostUsersTbl/3
        //  [ResponseType(typeof(UserDetailsView))]
        //   public void PostUsersTbl(UserDetailsView usersTbl)

        public void PostUsersTbl(int id)
        {
            UserDetailsView user = new UserDetailsView();
            user.UserAddress = "הרצל 5"; ;
            user.UserCityName = "תל אביב";
            user.UserComments = "";
            user.UserEmail = "bbb@gmail.com";
            user.UserFirstName = "דני";
            user.UserLastName = "כהן";
            user.UserName = "bbb";
            user.UserPaswrd = "1111";
            user.UserPhone1 = "23213213";
            user.UserPhone2 = "12321321";
            Userservice userservice = new Userservice();
            userservice.InsertUserDetails(user);

          //  return CreatedAtRoute("DefaultApi", new { id = usersTbl.UserID }, usersTbl);
        }
        public void InsertUserDetails(int id)
        // public void InsertUserDetails(UserDetailsView user)
        {
            UserDetailsView user = new UserDetailsView();
            user.UserAddress = "הרצל 5"; ;
            user.UserCityName = "תל אביב";
            user.UserComments = "";
            user.UserEmail = "bbb@gmail.com";
            user.UserFirstName = "דני";
            user.UserLastName = "כהן";
            user.UserName = "bbb";
            user.UserPaswrd = "1111";
            user.UserPhone1 = "23213213";
            user.UserPhone2 = "12321321";
            Userservice userservice = new Userservice();
            userservice.InsertUserDetails(user);
        }
    }
}

