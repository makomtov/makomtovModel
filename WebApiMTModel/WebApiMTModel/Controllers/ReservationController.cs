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
    [RoutePrefix("api/Reservation")]
    public class ReservationController : ApiController
    {
        //api/Reservation
        [Route("")]
        public List<OrderDetailsView> GetOrders()
        {

            OrderService orderService = new OrderService();
            return orderService.GetOrders();

        }
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

        // /api/Reservation/CreateOrder
        [Route("CreateOrder")]
        [HttpGet]
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
        // /api/Reservation/GetUserOrders/1
        [Route("GetUserOrders")]
       
        public OrdersForUserView GetUserOrdersList(int userID)
        {
            OrderService orderService  = new OrderService();
            return orderService.GetUserOrdersList(userID);
        }


        // /api/Reservation/1
        [Route("{userID}")]
        public UserDetailsView GetUserOrders(int UserID)
        {
            OrderService orderService = new OrderService();
            return orderService.GetUserOrders(UserID);
        }
        // /api/Reservation/GetAllOrdersAndDogs
        [Route("GetAllOrdersAndDogs")]
        public List<OrderDetailsView> GetAllOrdersAndDogs()
        {
            OrderService orderService = new OrderService();
            List<OrderDetailsView> list= orderService.GetAllOrdersAndDogs();
            return list;
        }

    // /api/Reservation/GetAllOrdersAndDogsManager
    [Route("GetAllOrdersAndDogsManager")]
    public List<OrderDetailsViewManager> GetAllOrdersAndDogsManager()
    {
        OrderService orderService = new OrderService();
        return orderService.GetAllOrdersAndDogsManager();
    }
    // /api/Reservation/UpdateOrdersByManager/Manager


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
