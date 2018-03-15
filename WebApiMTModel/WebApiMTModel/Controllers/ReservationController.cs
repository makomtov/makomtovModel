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
        public IQueryable GetOrders()
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

        public int CreateOrder(OrderDetailsView orderDetailsView)
        {
            //אם מחזיר -1 חסרים פרטים על המשתמש בטבלת משתמשים
            //Userservice userservice = new Userservice();
            //bool ok = userservice.CheckUserDetails(orderDetailsView.User.UserID);
            //if (!ok) return -1;
            OrderService orderService = new OrderService();
            return orderService.createOrder(orderDetailsView);
        }
        // api/Reservation/1
        [Route("{userID}")]
        public UserDetailsView GetUserOrders(int userID)
        {
            Userservice userservice = new Userservice();
            return userservice.GetUserOrders(userID);
        }
    }
}
