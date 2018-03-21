using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Collections;
using System.Web;
using System.Data.Entity;

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




                return orderslist;


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        //שליפת כל ההזמנות וכל הכלבים בהזמנות
        public List<OrderDetailsView> GetAllOrdersAndDogs()
        {
            List<OrderDetailsView> orders = null;
            //var orderslist = GetOrders();

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


                if (orderslist != null)
                {
                    orders = new List<OrderDetailsView>();

                    foreach (var order in orderslist)
                    {
                        OrderDetailsView orderDetails = new OrderDetailsView();
                        orderDetails.OrderNumber = order.orderNumber;
                        orderDetails.OrderDate = order.orderCreateDate;
                        orderDetails.OrderconfirmationNumber = order.orderconfirmationNumber;
                        orderDetails.Price = (decimal)order.price;
                        orderDetails.OrderStatus = order.orderStatus;
                        orderDetails.OrderStatusName = order.orderStatusName;

                        orderDetails.mDogs = new List<DogsInOrderView>();

                        //שליפת כל הכלבים להזמנה מטבלת  DogsInOrder
                        var dogsInOrder = context.DogsInOrder.Where(u => u.OrderNumber == order.orderNumber);

                        foreach (var dog in dogsInOrder)
                        {
                            DogsInOrderView dogsInOrderV = new DogsInOrderView();
                            dogsInOrderV.DogNumber = dog.DogNumber;
                            dogsInOrderV.DogName = dog.UserDogs.DogName;
                            dogsInOrderV.FromDate = dog.FromDate;
                            dogsInOrderV.ToDate = dog.ToDate;
                            dogsInOrderV.DogImage = dog.UserDogs.DogImage;
                            dogsInOrderV.ShiftNumberFrom = (int)dog.ShiftNumberFrom;
                            dogsInOrderV.ShiftNumberTo = (int)dog.ShiftNumberTo;
                            orderDetails.mDogs.Add(dogsInOrderV);
                        }
                        orders.Add(orderDetails);
                    }
                }
                return orders;
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
            ordersTbl.OrderUserId = ((UserDetailsView)HttpContext.Current.Session["userDetails"]).UserID;
            ordersTbl.OrderType = 1;
            ordersTbl.Price = CalculateOrderPrice();
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


        //שליפת הזמנה אחרונה למשתמש
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

        public void UpdateOrdersByManager(List<OrderDetailsView> OrdersList)
        {
            try
            {
                using (DatabaseEntitiesMT context = new DatabaseEntitiesMT())
                {
                    foreach (OrderDetailsView order in OrdersList)
                    {
                        var ordert = context.Set<OrdersTbl>().Find(order.OrderNumber);

                        
                        context.Entry(ordert).CurrentValues.SetValues(order);
                        context.SaveChanges();
                    }
                }
               context.Dispose();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

           
   
               
            //שליפת סטטוסים של הזמנה
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
        // public decimal CalculateOrderPrice(OrderDetailsView order)
        public decimal CalculateOrderPrice()

        {
            OrderDetailsView order = new OrderDetailsView();
            order.mDogs = new List<DogsInOrderView>();
            order.mDogs.Add(new DogsInOrderView());
            order.mDogs.Add(new DogsInOrderView());
            order.mDogs[0].FromDate = new DateTime(2018, 1, 1);
            order.mDogs[0].ToDate = new DateTime(2018, 1, 15);
            order.mDogs[1].FromDate = new DateTime(2018, 1, 20);
            order.mDogs[1].ToDate = new DateTime(2018, 2, 20);
            DatabaseEntitiesMT context = new DatabaseEntitiesMT();

            Decimal price = 0;
            var prices = context.PricesTbl
            .OrderBy(p => p.Days).Select(np => new PricesView
            {
                Days = np.Days,
                Dogs = np.Dogs,
                Price = np.Price
            });
            int days = 0;
            if ((order.mDogs.Count == 2 && order.mDogs[0].FromDate == order.mDogs[1].FromDate && order.mDogs[0].ToDate == order.mDogs[1].ToDate) || order.mDogs.Count == 1)
            {

                days = order.mDogs[0].ToDate.Subtract(order.mDogs[0].FromDate).Days;
                if (order.mDogs[0].ShiftNumberTo == 2)
                    days++;
                List<PricesView> p = prices.Where(o => o.Dogs == order.mDogs.Count).ToList();
                price = Calculte(days, p);

            }

            //לטפל ב 2 כלבים עם תאריכים שונים  
            else
            {
                DateTime dateTimeFrom, dateTimeTo;
                if (order.mDogs[0].FromDate.CompareTo(order.mDogs[1].FromDate) < 0)
                {
                    dateTimeFrom = order.mDogs[0].FromDate;
                }
                else
                {
                    dateTimeFrom = order.mDogs[1].FromDate;
                }
                if (order.mDogs[0].ToDate.CompareTo(order.mDogs[1].ToDate) < 0)
                {
                    dateTimeTo = order.mDogs[1].ToDate;
                }
                else
                {
                    dateTimeTo = order.mDogs[0].ToDate;
                }

                days = dateTimeTo.Subtract(dateTimeFrom).Days;
                if (order.mDogs[0].ShiftNumberTo == 2)
                    days++;
                int[] arr = new int[days];
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = -1;
                }
                DateTime date = order.mDogs[0].ToDate;

                //טיפול בהחזרה במשמרת בוקר- לא מחשיבים את היום לתשלום
                if (order.mDogs[0].ShiftNumberTo == 1)
                    date.AddDays(-1);
                changeArr(arr, order.mDogs[0].FromDate, date, dateTimeFrom);
                if (order.mDogs[1].ShiftNumberTo == 1)
                    date.AddDays(-1);
                changeArr(arr, order.mDogs[1].FromDate, date, dateTimeFrom);

                int dogs2 = 0;
                for (int i = 0; i < arr.Length; i++)
                {
                    dogs2 += arr[i];
                }
                List<PricesView> p;

                if (dogs2 > 0) //פתרון הבעיה שיש בהזמנה 2 כלבים אבל התאריכים לא חופפים ולכן יש בעצם פעמיים כלב 1
                {
                    //חישוב מספר הימים ל 2 כלבים
                    p = prices.Where(o => o.Dogs == 2).ToList();
                    price += Calculte(dogs2, p);
                }
                else
                    dogs2 *= -1;

                int dogs1 = arr.Length - dogs2;
                //חישוב מספר הימים לכלב 1
                p = prices.Where(o => o.Dogs == 1).ToList();
                price = Calculte(dogs1, p);




            }
            return price;
        }
        private void changeArr(int[] arr, DateTime min, DateTime max, DateTime first)
        {
            int i = 0;
            for (DateTime d = min; d <= max; d = d.AddDays(1))
            {
                i = d.Subtract(first).Days;
                arr[i]++;
            }
        }
        private decimal Calculte(int days, List<PricesView> listPrices)
        {
            decimal price = 0;

            bool exit = false;
            for (int i = 0; i < listPrices.Count() && !exit; i++)
            {
                if (days <= listPrices[i].Days)
                {
                    price = listPrices[i].Price * days;
                    exit = true;
                }
            }

            return price;
        }

        public List<PricesTbl> GetPrices()
        {
            return context.PricesTbl.ToList();
        }
        //שליפת הזמנות למשתמש

        public OrdersForUserView GetUserOrdersList(int userid)
        {
            try
            {
                DatabaseEntitiesMT context = new DatabaseEntitiesMT();
                //   שליפת כל ההזמנות מטבלתOrdersTbl

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
                           }).Where(p => p.OrderUserId == userid);

                // var orders = context.OrdersTbl.Where(p => p.OrderUserId == userDetails.UserID);
                OrdersForUserView ordersForUser = null;
                if (orderslist != null)
                {
                    ordersForUser = new OrdersForUserView();

                    foreach (var order in orderslist)
                    {
                        OrderDetailsView orderDetails = new OrderDetailsView();
                        orderDetails.OrderNumber = order.OrderNumber;
                        orderDetails.OrderDate = order.OrderCreateDate;
                        orderDetails.OrderconfirmationNumber = order.OrderconfirmationNumber;
                        orderDetails.Price = (decimal)order.Price;
                        orderDetails.mDogs = new List<DogsInOrderView>();

                        //שליפת כל הכלבים להזמנה מטבלת  DogsInOrder
                        var dogsInOrder = context.DogsInOrder.Where(u => u.OrderNumber == order.OrderNumber);

                        foreach (var dog in dogsInOrder)
                        {
                            DogsInOrderView dogsInOrderV = new DogsInOrderView();
                            dogsInOrderV.DogNumber = dog.DogNumber;
                            dogsInOrderV.DogName = dog.UserDogs.DogName;
                            dogsInOrderV.FromDate = dog.FromDate;
                            dogsInOrderV.ToDate = dog.ToDate;
                            dogsInOrderV.DogImage = dog.UserDogs.DogImage;
                            dogsInOrderV.ShiftNumberFrom = (int)dog.ShiftNumberFrom;
                            dogsInOrderV.ShiftNumberTo = (int)dog.ShiftNumberTo;
                            orderDetails.mDogs.Add(dogsInOrderV);
                        }
                        ordersForUser.UserReservations.Add(orderDetails);
                    }
                }
                return ordersForUser;

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserDetailsView GetUserOrders(int UserID)
        {
            UserDetailsView userDetails = new UserDetailsView();
            try
            {
                DatabaseEntitiesMT context = new DatabaseEntitiesMT();
                //   שליפת כל ההזמנות מטבלתOrdersTbl

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
                           }).Where(p => p.OrderUserId == UserID);

                // var orders = context.OrdersTbl.Where(p => p.OrderUserId == userDetails.UserID);

                if (orderslist != null)
                {
                    userDetails.UserReservations = new List<OrderDetailsView>();

                    foreach (var order in orderslist)
                    {
                        OrderDetailsView orderDetails = new OrderDetailsView();
                        orderDetails.OrderNumber = order.OrderNumber;
                        orderDetails.OrderDate = order.OrderCreateDate;
                        orderDetails.OrderconfirmationNumber = order.OrderconfirmationNumber;
                        orderDetails.Price = (decimal)order.Price;
                        orderDetails.OrderStatus = order.OrderStatus;
                        orderDetails.OrderStatusName = order.OrderStatusName;

                        orderDetails.mDogs = new List<DogsInOrderView>();

                        //שליפת כל הכלבים להזמנה מטבלת  DogsInOrder
                        var dogsInOrder = context.DogsInOrder.Where(u => u.OrderNumber == order.OrderNumber);

                        foreach (var dog in dogsInOrder)
                        {
                            DogsInOrderView dogsInOrderV = new DogsInOrderView();
                            dogsInOrderV.DogNumber = dog.DogNumber;
                            dogsInOrderV.DogName = dog.UserDogs.DogName;
                            dogsInOrderV.FromDate = dog.FromDate;
                            dogsInOrderV.ToDate = dog.ToDate;
                            dogsInOrderV.DogImage = dog.UserDogs.DogImage;
                            dogsInOrderV.ShiftNumberFrom = (int)dog.ShiftNumberFrom;
                            dogsInOrderV.ShiftNumberTo = (int)dog.ShiftNumberTo;
                            orderDetails.mDogs.Add(dogsInOrderV);
                        }
                        userDetails.UserReservations.Add(orderDetails);
                    }
                }
                return userDetails;

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }


    }

    }

