using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;



namespace WebApiMTModel.Models.Models.View
{
    public class ValidateModelStateFilter : ActionFilterAttribute
    {



        //public override void OnActionExecuting(HttpActionContext actionContext)
        //{


        //    if (actionContext.ModelState.IsValid) return;



        //    if (actionContext.ModelState.IsValid == false)
        //    {
              
        //        //var error = new
        //        //{
        //        //    status = false,
        //        //    message = "The request is invalid.",
        //        //    error = actionContext.ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
        //        //};
        //       List<string> errorlist = new List<string>();
        //            foreach (var value in actionContext.ModelState.Values)
        //            {
        //            foreach (var error in value.Errors)
        //                errorlist.Add(error.ErrorMessage);
        //                //errorlist.Add(value.Errors);
        //            }
        //        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errorlist);
        //       // throw response;
        //        }
        //       // actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest,new Exception(error.ToString()));
        //        //List<string> errorlist = new List<string>();
        //        //foreach (var value in actionContext.ModelState.Values)
        //        //{
        //        //    foreach (var error in value.Errors)
        //        //        errorlist.Add(error.Exception.ToString());
        //        //    //errorlist.Add(value.Errors);
        //        //}
        //        //HttpResponseMessage response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errorlist);
        //    }
            //var message = actionContext.ModelState
            //.SelectMany(f => f.Value.Errors);
           // .Aggregate("", (current, error) => error.ErrorMessage);

            //actionContext.Response = actionContext.Request
            //    .CreateResponse(HttpStatusCode.BadRequest, message);

            //    var errors = actionContext.ModelState
            //                              .Values
            //                              .SelectMany(m => m.Errors
            //                                                .Select(e => e.ErrorMessage));

            //actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
            //   actionContext.Response = actionContext.Request.CreateErrorResponse(new { Error = true, Message = "Token is invalid" });
            //    actionContext.Response.ReasonPhrase += errors;


            //if (!actionContext.ModelState.IsValid)
            //{
            //   actionContext.Response = actionContext.Request.CreateErrorResponse (HttpStatusCode.BadRequest, actionContext.ModelState);


            //}
            // base.OnActionExecuting(actionContext);
        //}
        }
    }//OnActionExecuting

