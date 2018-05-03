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
        [System.Web.Http.Authorize(Roles = "admin")]

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
        [System.Web.Http.AllowAnonymous]
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
                     userDetailsView = userservice.GetUserWithDogs(loginView.UserEmail, loginView.UserPassword);
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
                       
                        errorlist.Add(value.ErrorCode);
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
                {// return Request.CreateResponse(HttpStatusCode.BadRequest, errorlist); 
                    return Request.CreateResponse(code);
                }

            }

            catch
            {
                bool x = ModelState.IsValid;
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // /api/Users/1
        [System.Web.Http.Route("{userid}")]
        [System.Web.Http.Authorize(Roles = "admin,user")]
        public HttpResponseMessage GetUser(int userid)
        {
            Userservice userservice = new Userservice();
            UserDetailsView user= userservice.GetUser(userid);
            if(user==null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, user);
            }

        }

        // /api/Users/GetUserDogs/1
        [System.Web.Http.Route("GetUserDogs/{userid}")]
        [System.Web.Http.Authorize(Roles = "admin,user")]
        public HttpResponseMessage GetUserDogs(int userid)

        {
            Userservice userservice = new Userservice();
            List<DogDetailsView> list= userservice.GetUserDogs(userid);
            if (list.Count==0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, list);
            }

        }
        // /api/Users/InsertUserDetails
        [System.Web.Http.Authorize(Roles = "admin")]
        [System.Web.Http.Route("GetUserDogsByManager")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage GetUserDogsByManager([FromBody]int userid)

        {
            Userservice userservice = new Userservice();
            DogsForManagerView dogsForManagerView= userservice.GetUserDogsForManager(userid);
            if (dogsForManagerView==null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, dogsForManagerView);
            }

        }
        // /api/Users/InsertUserDetails
        [System.Web.Http.Route("InsertUserDetails")]
        [System.Web.Http.HttpPost]

        public HttpResponseMessage InsertUserDetails([FromBody] UserDetailsView user)
        {
           
            //HttpStatusCodeResult httpStatusCodeResult = null;
            //int code =(int) HttpStatusCode.OK;
            try
            {
                //var jsonString = userNew.Content.ReadAsStringAsync().Result;
                // UserDetailsView user = JsonConvert.DeserializeObject<UserDetailsView>(userNew);
                UserValidator validator = new UserValidator();
                ValidationResult results = validator.Validate(user);
                if (results.IsValid)
                {

                    Userservice userservice = new Userservice();
                    userservice.InsertUserDetails(user);
                    return Request.CreateResponse(HttpStatusCode.OK);

                    //  return Ok();
                }
                else
                {
                    //List<string> errorlist = new List<string>();
                    //foreach (var value in results.Errors)
                    //{
                    //    errorlist.Add(value.ErrorMessage);
                    //}
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            catch (HttpRequestException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
                // throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest,
                //                           badInputValidationException.Result));
                //Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }
        /// <summary>
        /// עדכון פרטי משתמש
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [System.Web.Http.Route("UpdateUserDetails")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage UpdateUserDetails([FromBody] UserDetailsView user)
            {
            try
            {
                //var jsonString = userNew.Content.ReadAsStringAsync().Result;
                // UserDetailsView user = JsonConvert.DeserializeObject<UserDetailsView>(userNew);
                UserValidator validator = new UserValidator();
                ValidationResult results = validator.Validate(user);
                if (results.IsValid)
                {

                    Userservice userservice = new Userservice();
                    userservice.UpdateUserDetails(user);
                    return Request.CreateResponse(HttpStatusCode.OK);

                    //  return Ok();
                }
                else
                {
                    //List<string> errorlist = new List<string>();
                    //foreach (var value in results.Errors)
                    //{
                    //    errorlist.Add(value.ErrorMessage);
                    //}
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            catch (HttpRequestException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
                // throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest,
                //                           badInputValidationException.Result));
                //Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }
        //code = (int)HttpStatusCode.BadRequest;
        //        List<string> errorlist = new List<string>();
        //        foreach (var value in results.Errors)
        //        {
        //            if (value.ErrorCode == "112")
        //            {
        //                // httpStatusCodeResult = new HttpStatusCodeResult(int.Parse(value.ErrorCode));
        //                //code = (int)httpStatusCodeResult.StatusCode;

        //                var message = string.Format(" כפילות במייל ");
        //                throw new HttpResponseException(
        //                    Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
        //            }
        //            errorlist.Add(value.ErrorMessage);
        //            //errorlist.Add(value.Errors);
        //        }
        //        //actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errorlist);
        //        //IList<ValidationFailure> failures = results.Errors;
        //        //List<string> errorlist = new List<string>();
        //        //foreach (ValidationFailure _error in failures)
        //        //{
        //        //    ModelState.AddModelError(_error.PropertyName, _error.ErrorMessage);
        //        //    //foreach (var error in value.Errors)
        //        //    //  errorlist.Add(value.ErrorMessage);
        //        //    //errorlist.Add(value.Errors);
        //        //}
        //        //   return 
        //        //  return new System.Web.Http.Controllers.HttpActionContext()
        //        //HttpActionContext actionContext = new HttpActionContext();
        //        return Request.CreateResponse(httpStatusCodeResult.StatusCode);

        // ThrowResponseException(HttpStatusCode.NotAcceptable, errorlist);
        //    }
        //}

        //catch
        //{
        //    bool x = ModelState.IsValid;
        //    return Request.CreateResponse(code);
        //}



        [System.Web.Http.Authorize(Roles = "admin")]
        [System.Web.Http.Route("UpdateUserDogsByManager")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage UpdateUserDogsByManager([FromBody]DogsForManagerView userDogs)
        {
            try { 
            UserdogsValidator userdogsValidator = new UserdogsValidator();
            ValidationResult results = userdogsValidator.Validate(userDogs);

            if (results.IsValid)
            {
                //var jsonString = userDogs.Content.ReadAsStringAsync().Result;
                //DogsForManagerView list = JsonConvert.DeserializeObject<DogsForManagerView>(jsonString);
                //  UserDetailsView user = JsonConvert.DeserializeObject<UserDetailsView>(jsonString);
                Userservice userservice = new Userservice();
                userservice.UpdateDogsByManager(userDogs.UserDogs);

                return Request.CreateResponse(HttpStatusCode.OK);
            }

                else
                {
                   
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
}
            catch (HttpRequestException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
                // throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest,
                //                           badInputValidationException.Result));
                //Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
            
        }
        /// <summary>
        /// עדכון פרטי כלב
        /// </summary>
        /// <param name="userDog"></param>
        /// <returns></returns>
        [System.Web.Http.Route("UpdateUserDog")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage UpdateUserDog([FromBody]DogDetailsView userDog)
        {
            try
            {
                dogValidator userdogValidator = new dogValidator();
                ValidationResult results = userdogValidator.Validate(userDog);

                if (results.IsValid)
                {
                    //var jsonString = userDogs.Content.ReadAsStringAsync().Result;
                    //DogsForManagerView list = JsonConvert.DeserializeObject<DogsForManagerView>(jsonString);
                    //  UserDetailsView user = JsonConvert.DeserializeObject<UserDetailsView>(jsonString);
                    Userservice userservice = new Userservice();
                    userservice.UpdateDog(userDog);

                    return Request.CreateResponse(HttpStatusCode.OK);
                }

                else
                {

                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            catch (HttpRequestException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
                // throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest,
                //                           badInputValidationException.Result));
                //Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

        }
        /// <summary>
        /// הוספת כלב למשתמש
        /// </summary>
        /// <param name="userDog"></param>
        /// <returns></returns>
        [System.Web.Http.Route("AddOneUserDog")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage AddOneUserDog([FromBody]DogDetailsView userDog)
        {
            try
            {
                dogValidator userdogValidator = new dogValidator();
                ValidationResult results = userdogValidator.Validate(userDog);

                if (results.IsValid)
                {
                    //var jsonString = userDogs.Content.ReadAsStringAsync().Result;
                    //DogsForManagerView list = JsonConvert.DeserializeObject<DogsForManagerView>(jsonString);
                    //  UserDetailsView user = JsonConvert.DeserializeObject<UserDetailsView>(jsonString);
                    Userservice userservice = new Userservice();
                    userservice.AddOneDogForUser(userDog);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }

                else
                {

                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            catch (HttpRequestException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
                // throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest,
                //                           badInputValidationException.Result));
                //Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

        }

        [System.Web.Http.Route("DeleteOneUserDog")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage DeleteOneUserDog([FromBody]DogDetailsView userDog)
        {
            try
            {
                Userservice userservice = new Userservice();
                userservice.DeleteDog(userDog);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (HttpRequestException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
                // throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest,
                //                           badInputValidationException.Result));
                //Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
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

