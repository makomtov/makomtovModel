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
using System.Web.Http.Cors;
using FluentValidation.Results;
using System.Web.Http.Controllers;

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
        ///  שליפת משתמש לפי פרמטרי פייסבוק
        /// </summary>
        /// <param name="usereMail"></param>
        /// <param name="password"></param>
        /// <returns></returns>

        // /api/Users/GetUserFB
        
        [System.Web.Http.Route("GetUserFB")]
        [System.Web.Http.HttpPost]
       
         public IHttpActionResult GetUserFB([FromBody]LoginView loginView)
     
        {
            try
            {
                
           
            Userservice userservice = new Userservice();
                UserDetailsView userDetailsView = userservice.GetUserFB(loginView.UserEmail, loginView.UserPassword);
            return Ok(userDetailsView);
        }
        
             catch
            {
               bool x= ModelState.IsValid;
                return InternalServerError();
    }

}
        /// <summary>
        ///  שליפת משתמש לפי שם משתמש וסיסמה
        /// </summary>
        /// <param name="loginView"></param>
        /// <returns></returns>
        // /api/Users/GetLogInUser/ziris248@gmail.com/iris1234
        [System.Web.Http.Route("GetUser")]
        [System.Web.Http.HttpPost]

        public HttpResponseMessage GetUser([FromBody]LoginView loginView)

        {
            List<string> errorlist = null;
            UserDetailsView userDetailsView=null;
            int code = (int)HttpStatusCode.OK;
            try
            {

                LoginValidator validator = new LoginValidator();
                ValidationResult results = validator.Validate(loginView);
                bool validationSucceeded = results.IsValid;
                if (validationSucceeded)
                {
                    Userservice userservice = new Userservice();
                     userDetailsView = userservice.GetUser(loginView.UserEmail, loginView.UserPassword);
                    if (userDetailsView != null)
                        code = (int)HttpStatusCode.OK;
                   else
                        code = (int)HttpStatusCode.BadRequest;
                    //  return Ok();
                }
                else
                {
                    code = (int)HttpStatusCode.BadRequest;
                    errorlist = new List<string>();
                    foreach (var value in results.Errors)
                    {

                        errorlist.Add(value.ErrorMessage);
                        //errorlist.Add(value.Errors);
                    }
                    //actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errorlist);
                    //IList<ValidationFailure> failures = results.Errors;
                    //List<string> errorlist = new List<string>();
                    //foreach (ValidationFailure _error in failures)
                    //{
                    //    ModelState.AddModelError(_error.PropertyName, _error.ErrorMessage);
                    //    //foreach (var error in value.Errors)
                    //    //  errorlist.Add(value.ErrorMessage);
                    //    //errorlist.Add(value.Errors);
                    //}
                    //   return 
                    //  return new System.Web.Http.Controllers.HttpActionContext()
                    //HttpActionContext actionContext = new HttpActionContext();
                   

                    // ThrowResponseException(HttpStatusCode.NotAcceptable, errorlist);
                }
                if((HttpStatusCode)code==HttpStatusCode.OK)
                { return Request.CreateResponse(HttpStatusCode.OK, userDetailsView); }
                else
                { return Request.CreateResponse(HttpStatusCode.BadRequest, errorlist); }

            }

            catch
            {
                bool x = ModelState.IsValid;
                return Request.CreateResponse(HttpStatusCode.BadRequest);
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
       
        public HttpResponseMessage InsertUserDetails([FromBody] UserDetailsView user)
        {

            try
            {
                //var jsonString = userNew.Content.ReadAsStringAsync().Result;
                // UserDetailsView user = JsonConvert.DeserializeObject<UserDetailsView>(userNew);
                UserValidator validator = new UserValidator();
                ValidationResult results = validator.Validate(user);
                bool validationSucceeded = results.IsValid;
                if (validationSucceeded)
                {
                    
                    Userservice userservice = new Userservice();
                    userservice.InsertUserDetails(user);

                    return Request.CreateResponse(HttpStatusCode.OK);
                  //  return Ok();
                }
                else
                {
                    List<string> errorlist = new List<string>();
                    foreach (var value in results.Errors)
                    {
                        
                            errorlist.Add(value.ErrorMessage);
                        //errorlist.Add(value.Errors);
                    }
                    //actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errorlist);
                    //IList<ValidationFailure> failures = results.Errors;
                    //List<string> errorlist = new List<string>();
                    //foreach (ValidationFailure _error in failures)
                    //{
                    //    ModelState.AddModelError(_error.PropertyName, _error.ErrorMessage);
                    //    //foreach (var error in value.Errors)
                    //    //  errorlist.Add(value.ErrorMessage);
                    //    //errorlist.Add(value.Errors);
                    //}
                    //   return 
                    //  return new System.Web.Http.Controllers.HttpActionContext()
                    //HttpActionContext actionContext = new HttpActionContext();
                   return   Request.CreateResponse(HttpStatusCode.BadRequest, errorlist);
                    
                   // ThrowResponseException(HttpStatusCode.NotAcceptable, errorlist);
                }
            }

            catch
            {
                bool x = ModelState.IsValid;
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }


        }

        [System.Web.Http.Route("UpdateUserDogsByManager")]
        [System.Web.Http.HttpPost]
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

