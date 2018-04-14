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
    [RoutePrefix("api/Reservation")]
    public class ReservationController : ApiController
    {
        /// <summary>
        /// שליפת כל ההזמנות
        /// </summary>
        /// <returns></returns>
        //api/Reservation
        [Route("")]
        public List<OrderDetailsView> GetOrders()
        {

            OrderService orderService = new OrderService();
            return orderService.GetOrders();

        }

        /// <summary>
        /// שליפת כל הסטטוסים להזמנה
        /// </summary>
        /// <returns></returns>
        //  /api/Reservation/OrderStatusList
        [Route("OrderStatusList")]
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
        [HttpPost]
      //  public void CreateOrder()
         public int CreateOrder(OrderDetailsView orderDetailsView)
        {
            //אם מחזיר -1 חסרים פרטים על המשתמש בטבלת משתמשים
            //Userservice userservice = new Userservice();
            //bool ok = userservice.CheckUserDetails(orderDetailsView.User.UserID);
            //if (!ok) return -1;
            OrderService orderService = new OrderService();
            //  orderService.createOrder(orderDetailsView);
             return orderService.createOrder(orderDetailsView);
           // return orderService.CreateOrder();
        }
        /// <summary>
        /// שליפת כל ההזמנות למשתמש
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        // /api/Reservation/GetUserOrders/1
        [Route("GetUserOrders")]
       
        public OrdersForUserView GetUserOrdersList(int userID)
        {
            OrderService orderService  = new OrderService();
            return orderService.GetUserOrdersList(userID);
        }

        /// <summary>
        /// שליפת כל ההזמנות למשתמש
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        // /api/Reservation/1
        [Route("{userID}")]
        public UserDetailsView GetUserOrders(int UserID)
        {
            OrderService orderService = new OrderService();
            return orderService.GetUserOrders(UserID);
        }

        /// <summary>
        /// שליפת כל ההזמנות 
        /// </summary>
        /// <returns></returns>
        // /api/Reservation/GetAllOrdersAndDogs
        [Route("GetAllOrdersAndDogs")]
        public List<OrderDetailsView> GetAllOrdersAndDogs()
        {
            OrderService orderService = new OrderService();
            List<OrderDetailsView> list= orderService.GetAllOrdersAndDogs();
            return list;
        }

        /// <summary>
        /// שליפת כל ההזמנות למנהלי הכלביה
        /// </summary>
        /// <returns></returns>
    // /api/Reservation/GetAllOrdersAndDogsManager
    [Route("GetAllOrdersAndDogsManager")]
    public List<OrderDetailsViewManager> GetAllOrdersAndDogsManager()
    {
        OrderService orderService = new OrderService();
        return orderService.GetAllOrdersAndDogsManager();
    }
    // /api/Reservation/UpdateOrdersByManager/Manager

            /// <summary>
            /// עדכון הזמנות על ידי הכלביה
            /// </summary>
            /// <param name="Orders"></param>

    [Route("UpdateOrdersByManager")]
        [HttpPut]
         public void UpdateOrdersByManager(HttpRequestMessage Orders)
        {
            var jsonString = Orders.Content.ReadAsStringAsync().Result;
            OrdersForManagetView  list = JsonConvert.DeserializeObject<OrdersForManagetView>(jsonString);
            //  UserDetailsView user = JsonConvert.DeserializeObject<UserDetailsView>(jsonString);
            OrderService orderService = new OrderService();
            orderService.UpdateOrdersByManager(list.UserReservations);
        }
    }
}
