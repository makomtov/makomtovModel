using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Collections;

namespace WebApiMTModel.Models.Models.View
{
    public class OrderService
    {

        DatabaseEntitiesMT context;


        public OrderService()
        {

            context = new DatabaseEntitiesMT();
        }
        //שליפת כל ההזמנות 
        public IQueryable GetOrders()
        {
            try
            {
                DatabaseEntitiesMT context = new DatabaseEntitiesMT();

                var orderslist = context.OrdersTbl.
                    Join(context.OrderTypes,
                    o => o.OrderType, ot => ot.OrderTypeId,
                    (o, ot) => new
                    {
                        orderNumber = o.OrderNumber,
                        orderCreateDate = o.OrderCreateDate,
                        price = o.Price,
                        orderconfirmationNumber = o.OrderconfirmationNumber,
                        orderStatus = o.OrderStatus,
                        orderUserId = o.OrderUserId,
                        orderTypeName = ot.OrderTypeName
                    }).Join(context.StatusTbl,
                            a => a.orderStatus, s => s.StatusId,
                            (a, s) => new
                            {
                                OrderNumber = a.orderNumber,
                                OrderCreateDate = a.orderCreateDate,
                                Price = a.price,
                                OrderconfirmationNumber = a.orderconfirmationNumber,
                                OrderStatus = a.orderStatus,
                                OrderStatusName = s.StatusName,
                                OrderTypeName = a.orderTypeName,
                                OrderUserId = a.orderUserId
                            }).Join(context.UsersTbl,
                            o => o.OrderUserId, u => u.UserID,
                            (o, u) => new
                            {
                                orderNumber = o.OrderNumber,
                                orderCreateDate = o.OrderCreateDate,
                                price = o.Price,
                                orderconfirmationNumber = o.OrderconfirmationNumber,
                                orderStatus = o.OrderStatus,
                                orderStatusName = o.OrderStatusName,
                                orderTypeName = o.OrderTypeName,
                                orderUserId = u.UserID,
                                orderUserName = u.UserFirstName + " " + u.UserLastName,



                            });


                //DatabaseEntitiesMT context = new DatabaseEntitiesMT();
                //List<OrderDetailsView> list = new List<OrderDetailsView>();
                //List<OrdersTbl> orders = context.OrdersTbl.ToList();
                //foreach (OrdersTbl order in orders)
                //{

                //         OrderDetailsView orderDetails = new OrderDetailsView();
                //    orderDetails.OrderDate = order.OrderCreateDate;
                //    orderDetails.OrderNumber = order.OrderNumber;
                //    orderDetails.Price =(decimal)order.Price;
                //   // orderDetails.User = 

                //        list.Add(orderDetails);


                return orderslist;
               

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        //הכנסת הזמנה
        public int createOrder(OrderDetailsView orderDetailsView)
        {
            Userservice userservice = new Userservice();
            
            OrdersTbl ordersTbl = new OrdersTbl();
            ordersTbl.OrderStatus = 11;
            ordersTbl.OrderUserId = 1;
            ordersTbl.OrderType = 1;
            ordersTbl.Price = orderDetailsView.Price;
            for (int i = 0; i < orderDetailsView.mDogs.Count; i++)
            {
                if (orderDetailsView.mDogs[i].Pension == true || orderDetailsView.mDogs[i].Training == true)

                {
                    DogsInOrder dogsInOrder = new DogsInOrder();
                    dogsInOrder.DogTraining = orderDetailsView.mDogs[i].Training;
                    dogsInOrder.DogNumber = orderDetailsView.mDogs[i].DogNumber;
                    dogsInOrder.FromDate = orderDetailsView.mDogs[i].FromDate;
                    dogsInOrder.ToDate = orderDetailsView.mDogs[i].ToDate;
                    dogsInOrder.HomeFood = orderDetailsView.mDogs[i].HomeFood;

                    dogsInOrder.Status = 11;
                    ordersTbl.DogsInOrder.Add(dogsInOrder);
                }

            }
            //   string x = form["item.training"].ToString();
            // orderViewDetailsCreate.mDogs=form["mDogs"]



            context.OrdersTbl.Add(ordersTbl);
            context.SaveChanges();
            //שליפת מספר ההזמנה שנוצרה
            int orderID = GetLastOrder(orderDetailsView.User.UserID);
            return orderID;

        }



        private int GetLastOrder(int CustomerID)
        {
            try
            {
                using (DatabaseEntitiesMT context = new DatabaseEntitiesMT())
                {
                    var orderNumber =
                        from p in context.OrdersTbl
                        where p.OrderUserId == CustomerID
                        select p.OrderNumber;
                    return orderNumber.Max();


                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable GetOrderStatusList()
        {
             DatabaseEntitiesMT context = new DatabaseEntitiesMT();

            var data = context.StatusTbl
            .Select(o => new StatusView
            {
                StatusGroup = o.StatusGroup,
                StatusId = o.StatusId,
                StatusName = o.StatusName,

            }).Where(o => o.StatusGroup == 1);
            return data;
        }
           






           
    
        //חישוב ההזמנה
        public decimal CalculsteOrderPrice(OrderDetailsView order)
        {
            Decimal price = 0;
            List<PricesTbl> prices = context.PricesTbl
            .OrderBy(p => p.Days).ToList();

            if (order.mDogs.Count == 1)
            {
                var prices1 = prices.Where(p => p.Dogs == 1);
                int days = order.mDogs[0].ToDate.Subtract(order.mDogs[0].FromDate).Days;
                bool exit = false;
                for (int i = 0; i < prices.Count() && !exit; i++)
                {
                    if (days < prices[i].Days)
                    {
                        price = prices[i].Price * days;
                        exit = true;
                    }
                }

            }

            //לטפל ב 2 כלבים עם תאריכים שונים או שווים

            return price;

        }
        public List<PricesTbl> GetPrices()
        {
            return context.PricesTbl.ToList();
        }
    }
}
