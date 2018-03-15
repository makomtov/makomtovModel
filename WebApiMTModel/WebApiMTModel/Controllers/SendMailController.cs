using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiMTModel.Models.Models.View;

namespace WebApiMTModel.Controllers
{
    [RoutePrefix("api/SendMail")]
    public class SendMailController : ApiController
    {
        [Route("")]
        [HttpPost]
        public IHttpActionResult SendEmail(string mailModel)
        {
            try
            {
                SendMailService sendMailService = new SendMailService();
                sendMailService.SendMail(mailModel);
                return Ok("Mail Sent");
            }
            catch (ObjectDisposedException ex)
            {
                throw ex;

            }
        }
    }
}
