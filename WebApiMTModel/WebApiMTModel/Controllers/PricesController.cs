using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiMTModel.Models.Models.View;

namespace WebApiMTModel.Controllers
{/// <summary>
 /// פעולות על הזמנות
 /// </summary>
    [System.Web.Http.Authorize]
    [RoutePrefix("api/Prices")]
    public class PricesController : ApiController
    {
        /// <summary>
        ///  שליפת המחירון
        /// </summary>
        /// <returns></returns>
        //api/Reservation/GetFutureOrders/8/orders/3

        [Route("GetPrices")]
        [System.Web.Http.Authorize(Roles = "admin")]
        [HttpGet]
        // [AllowAnonymous]
        public HttpResponseMessage GetPrices()
        {

            PriceService priceService= new PriceService();
            List<PricesView> list = priceService.GetPrices();
            if (list == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, list);
            }
        }
        /// <summary>
        /// עדכון מחירון על ידי הכלביה
        /// </summary>
        /// <param name="Orders"></param>
        [System.Web.Http.Authorize(Roles = "admin")]
        [Route("UpdatePricesByManager")]
        [HttpPut]
        //public void UpdateOrdersByManager(HttpRequestMessage Orders)
        public HttpResponseMessage UpdatePricesByManager([FromBody] List<PricesView> listPrices)
        {
            //if (ModelState.IsValid)
            //{
            //var jsonString = Orders.Content.ReadAsStringAsync().Result;
            //OrdersForManagetView list = JsonConvert.DeserializeObject<OrdersForManagetView>(jsonString);
            //  UserDetailsView user = JsonConvert.DeserializeObject<UserDetailsView>(jsonString);
            try
            {

                PriceService priceService = new PriceService();
                priceService.UpdatePrices(listPrices);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.NotModified);
            }

            //}
        }

    }
}
