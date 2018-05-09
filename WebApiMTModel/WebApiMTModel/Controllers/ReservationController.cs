using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApiMTModel.Models;
using WebApiMTModel.Models.Models.View;

namespace WebApiMTModel.Controllers
{
    /// <summary>
    /// פעולות על הזמנות
    /// </summary>
    [System.Web.Http.Authorize]
    [RoutePrefix("api/Reservation")]
    public class ReservationController : ApiController
    {
        public ReservationController()
        { }
        /// <summary>
        /// שליפת כל ההזמנות
        /// </summary>
        /// <returns></returns>
        //api/Reservation
        [Route("")]
        [System.Web.Http.Authorize(Roles = "admin")]
        public List<OrderDetailsView> GetOrders()
        {

            OrderService orderService = new OrderService();
            return orderService.GetOrders();

        }
        /// <summary>
        /// get open hours
        /// </summary>
        /// <returns></returns>
        [Route("OpenHoursList")]
       [AllowAnonymous]
        public List<ShiftView> GetOpenHoursList()
        {
            OrderService orderService = new OrderService();
            return orderService.GetOpenHoursList();
        }

        /// <summary>
        /// שליפת כל הסטטוסים להזמנה
        /// </summary>
        /// <returns></returns>
        //  /api/Reservation/OrderStatusList
        [Route("OrderStatusList")]
        [System.Web.Http.Authorize(Roles = "admin")]
        public IQueryable GetOrderStatusList()
        {
            OrderService orderService = new OrderService();
            return orderService.GetOrderStatusList();
        }

        //  /api/Reservation/CalculateOrderPrice/calc
        //[Route("CalculateOrderPrice/calc")]
        //[HttpGet]
        //public decimal CalculateOrderPrice()
        //{
        //    OrderService orderService = new OrderService();
        //    return orderService.CalculateOrderPrice();
        //}

        /// <summary>
        ///  יצירת הזמנה חדשה
        /// </summary>
        /// <param name="orderDetailsView"></param>
        /// <returns></returns>
        // /api/Reservation/CreateOrder
        [Route("CreateOrder")]
        [System.Web.Http.Authorize(Roles = "admin,user")]
        [HttpPost]
        //  public void CreateOrder()
        public HttpResponseMessage CreateOrder([FromBody] OrderDetailsView orderDetailsView)
        {
            string message="";
            int orderNum = 0;
            try
            {
                //אם מחזיר -1 חסרים פרטים על המשתמש בטבלת משתמשים
                //Userservice userservice = new Userservice();
                //bool ok = userservice.CheckUserDetails(orderDetailsView.User.UserID);
                OrderService orderService = new OrderService();
                 orderNum = orderService.createOrder(orderDetailsView);
                //if (!ok) return -1;
                if (orderNum == -997)
                {
                     message = string.Format(" כפילות בהזמנות -  יש כלב משותף בהזמנה הנוכחית ןבהזמנות קודמות באותם תאריכים ");

                    HttpError httpError = new HttpError(message);
                    return Request.CreateErrorResponse(HttpStatusCode.Conflict, httpError);

                }
                else
                {

                    return Request.CreateResponse(HttpStatusCode.OK, orderNum);

                }
            }
            catch (HttpRequestException  ex)
            {
                throw ex;
                // throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest,
                //                           badInputValidationException.Result));
                //Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }


        }
        /// <summary>
        /// שליפת כל ההזמנות למשתמש
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        // /api/Reservation/GetUserOrders/1
        //[Route("GetUserOrders")]

        //public OrdersForUserView GetUserOrdersList(int userID)
        //{
        //    OrderService orderService  = new OrderService();
        //    return orderService.GetUserOrdersList(userID);
        //}
        [Route("GetUserOrders")]
        [System.Web.Http.Authorize(Roles = "admin,user")]
        public HttpResponseMessage GetUserOrdersList(int userID)
        {
            OrderService orderService = new OrderService();
          OrdersForUserView ordersForUserView= orderService.GetUserOrdersList(userID);
            if (ordersForUserView == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, ordersForUserView);
            }
        }
        /// <summary>
        /// שליפת כל ההזמנות למשתמש
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        // /api/Reservation/1
        //[Route("{userID}")]
        //public UserDetailsView GetUserOrders(int UserID)
        //{
        //    OrderService orderService = new OrderService();
        //    return orderService.GetUserOrders(UserID);
        //}

        /// <summary>
        /// שליפת כל ההזמנות 
        /// </summary>
        /// <returns></returns>
        // /api/Reservation/GetAllOrdersAndDogs
        [Route("GetAllOrdersAndDogs")]
        [System.Web.Http.Authorize(Roles = "admin")]
        public HttpResponseMessage  GetAllOrdersAndDogs()
        //public List<OrderDetailsView> GetAllOrdersAndDogs()
        {
            OrderService orderService = new OrderService();
            List<OrderDetailsView> list= orderService.GetAllOrdersAndDogs();
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
        /// שליפת כל ההזמנות למנהלי הכלביה
        /// </summary>
        /// <returns></returns>
    // /api/Reservation/GetAllOrdersAndDogsManager
    [Route("GetAllOrdersAndDogsManager")]
        [System.Web.Http.Authorize(Roles = "admin")]
        public HttpResponseMessage GetAllOrdersAndDogsManager()
        //  public List<OrderDetailsViewManager> GetAllOrdersAndDogsManager()
        {
        OrderService orderService = new OrderService();
        List<OrderDetailsViewManager> list= orderService.GetAllOrdersAndDogsManager();
            if (list == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, list);
            }
        }
        // /api/Reservation/UpdateOrdersByManager/Manager

        /// <summary>
        /// עדכון הזמנות על ידי הכלביה
        /// </summary>
        /// <param name="Orders"></param>
        [System.Web.Http.Authorize(Roles = "admin")]
        [Route("UpdateOrdersByManager")]
        [HttpPut]
        //public void UpdateOrdersByManager(HttpRequestMessage Orders)
        public HttpResponseMessage UpdateOrdersByManager([FromBody] List<OrderDetailsViewManager> listOrder)
        {
            //if (ModelState.IsValid)
            //{
            //var jsonString = Orders.Content.ReadAsStringAsync().Result;
            //OrdersForManagetView list = JsonConvert.DeserializeObject<OrdersForManagetView>(jsonString);
            //  UserDetailsView user = JsonConvert.DeserializeObject<UserDetailsView>(jsonString);
            try
            {

                OrderService orderService = new OrderService();
                orderService.UpdateOrdersByManager(listOrder);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.NotModified);
            }

            //}
        }

        /// <summary>
        /// עדכון הזמנה על ידי משתמש
        /// </summary>
        /// <param name="Orders"></param>
        [System.Web.Http.Authorize(Roles = "admin,user")]
        [Route("UpdateOrder")]
        [HttpPut]
        //public void UpdateOrdersByManager(HttpRequestMessage Orders)
        public HttpResponseMessage UpdateOrder([FromBody] OrderDetailsView order)
        {
            //if (ModelState.IsValid)
            //{
            //var jsonString = Orders.Content.ReadAsStringAsync().Result;
            //OrdersForManagetView list = JsonConvert.DeserializeObject<OrdersForManagetView>(jsonString);
            //  UserDetailsView user = JsonConvert.DeserializeObject<UserDetailsView>(jsonString);
            try
            {

                OrderService orderService = new OrderService();
                orderService.UpdateOrderDetails(order);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            
                catch (Exception ex)
            {
              //  ThrowResponseException(HttpStatusCode.NotModified, ex.Message);
                return Request.CreateResponse(HttpStatusCode.NotModified);
            }
           
           

            //}
        }
        private void ThrowResponseException(HttpStatusCode statusCode, string message)
        {
            var errorResponse = Request.CreateErrorResponse(statusCode, message);
            throw new HttpResponseException(errorResponse);
        }
    }
}
