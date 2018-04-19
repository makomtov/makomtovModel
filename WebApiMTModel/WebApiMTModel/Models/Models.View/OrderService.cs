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
        //שליפת כל ההזמנות של כל המשתמשים
        private List<OrderDetailsView> GetOrdersFromDB()
        {


            List<OrderDetailsViewManager> orderslistManager = context.OrdersTbl.
                       Join(context.OrderTypes,
                       o => o.OrderType, ot => ot.OrderTypeId,
                       (o, ot) => new
                       {
                           o.OrderNumber,
                           OrderDate = o.OrderCreateDate,
                           Price = (decimal)o.Price,
                           o.OrderconfirmationNumber,
                           o.OrderStatus,
                           Userid = o.UsersTbl.UserID,
                           OrderType = ot.OrderTypeId,
                           o.ShiftNumberFrom,
                           o.ShiftNumberTo,
                           o.FromDate,
                           o.ToDate,
                           ot.OrderTypeName,
                           o.ManagerComments,
                           o.discount



                       }).Join(context.StatusTbl,
                               a => a.OrderStatus, s => s.StatusId,
                               (a, s) => new
                               {
                                   a.OrderNumber,
                                   a.OrderDate,
                                   Price = (decimal)a.Price,
                                   a.OrderconfirmationNumber,
                                   a.OrderStatus,
                                   a.Userid,
                                   OrderStatusName = s.StatusName,
                                   a.ShiftNumberFrom,
                                   a.ShiftNumberTo,
                                   a.FromDate,
                                   a.ToDate,
                                   a.OrderType,
                                   a.OrderTypeName,
                                   a.ManagerComments,
                                   a.discount

                               }).Join(context.UsersTbl,
                               o => o.Userid, u => u.UserID,
                               (o, u) => new OrderDetailsViewManager
                               {
                                   OrderNumber = o.OrderNumber,
                                   OrderDate = o.OrderDate,
                                   Price = (decimal)o.Price,
                                   OrderconfirmationNumber = o.OrderconfirmationNumber,
                                   OrderStatus = o.OrderStatus,
                                   Userid = o.Userid,
                                   userFirstName = u.UserFirstName,
                                   userLastName = u.UserLastName,
                                   OrderStatusName = o.OrderStatusName,
                                   FromDate = (DateTime)o.FromDate,
                                   ToDate = (DateTime)o.ToDate,
                                   ShiftNumberFrom = (int)o.ShiftNumberFrom,
                                   ShiftNumberTo = (int)o.ShiftNumberTo,
                                   OrderType = o.OrderType,
                                   OrderTypeName = o.OrderTypeName,
                                   ManagerComments = o.ManagerComments,
                                   Discount=(decimal)o.discount
                               }

                               ).Distinct()
                         .ToList();


            List<OrderDetailsView> orderslist = new List<OrderDetailsView>();
            for (int i = 0; i < orderslistManager.Count; i++)
            {
                OrderDetailsView detailsView = orderslistManager[i];
                orderslist.Add(detailsView);
            }
            return orderslist;
        }

        private List<OrderDetailsViewManager> GetOrdersFromDBManager()
        {
            List<OrderDetailsView> list = GetOrdersFromDB();

            List<OrderDetailsViewManager> listManager = new List<OrderDetailsViewManager>();
            for (int i = 0; i < list.Count; i++)
            {
                OrderDetailsViewManager lm = (OrderDetailsViewManager)list[i];

                listManager.Add(lm);


            }
            return listManager;
        }

        //שליפת כל ההזמנות 
        public List<OrderDetailsView> GetOrders()

        {
            try
            {
                DatabaseEntitiesMT context = new DatabaseEntitiesMT();
                List<OrderDetailsView> orderslist = GetOrdersFromDB();


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

            try
            {

                DatabaseEntitiesMT context = new DatabaseEntitiesMT();
                List<OrderDetailsView> orderslist = GetOrdersFromDB();
                if (orderslist != null)
                {



                    for (int i = 0; i < orderslist.Count; i++)
                    {


                        //שליפת כל הכלבים להזמנה מטבלת  DogsInOrder
                        orderslist[i].mDogs = GetDogsForOrder(orderslist[i].OrderNumber);


                    }
                }
                return orderslist;
            }


            catch (Exception ex)
            {

                throw ex;
            }

        }

        // שליפת כל ההזמנות וכל הכלבים בהזמנות- מנהל
        public List<OrderDetailsViewManager> GetAllOrdersAndDogsManager()
        {

            try
            {

                DatabaseEntitiesMT context = new DatabaseEntitiesMT();
                List<OrderDetailsViewManager> orderslist = GetOrdersFromDBManager();
                if (orderslist != null)
                {



                    for (int i = 0; i < orderslist.Count; i++)
                    {


                        //שליפת כל הכלבים להזמנה מטבלת  DogsInOrder
                        orderslist[i].mDogs = GetDogsForOrder(orderslist[i].OrderNumber);


                    }
                }
                return orderslist;
            }


            catch (Exception ex)
            {

                throw ex;
            }

        }
        //הכנסת הזמנה
         public int createOrder(OrderDetailsView orderDetailsView)
      //  public int CreateOrder()

        {
            //Userservice userservice = new Userservice();
            //OrderDetailsView orderDetailsView = new OrderDetailsView();
            //orderDetailsView.Userid = 1;
            //orderDetailsView.FromDate = new DateTime(2018, 11, 01);
            //orderDetailsView.ToDate = new DateTime(2018, 11, 30);
            //orderDetailsView.ShiftNumberFrom = 1;
            //orderDetailsView.ShiftNumberTo = 2;
            //orderDetailsView.User = new UserDetailsView();
            //orderDetailsView.User.UserEmail = "ziris248@gmail.com";
            //orderDetailsView.userFirstName = "איריס";
            //orderDetailsView.userLastName = "זרצקי";
            //orderDetailsView.mDogs = new List<DogsInOrderView>();
            //orderDetailsView.mDogs.Add(new DogsInOrderView());
            //orderDetailsView.mDogs.Add(new DogsInOrderView());
            //orderDetailsView.mDogs[0].DogNumber = 4;
            //orderDetailsView.mDogs[0].Training = true;
            //orderDetailsView.mDogs[1].DogNumber = 3;
            OrdersTbl ordersTbl = new OrdersTbl();
           
           //if(orderDetailsView.mDogs.Count==2)
           //     ordersTbl.Price = CalculateOrderPrice(orderDetailsView);
          
            decimal result = checkForAnotherParallelOrder(orderDetailsView);  //אין הזמנות חופפות לאותם כלבים
            if (result > 0)
            {
                
                ordersTbl.OrderStatus = 11;
                ordersTbl.OrderUserId = orderDetailsView.Userid;
                ordersTbl.OrderType = 1;
                ordersTbl.FromDate = orderDetailsView.FromDate;
                ordersTbl.ShiftNumberFrom = orderDetailsView.ShiftNumberFrom;
                ordersTbl.ToDate = orderDetailsView.ToDate;
                ordersTbl.ShiftNumberTo = orderDetailsView.ShiftNumberTo;
                ordersTbl.OrderCreateDate = DateTime.Now;
                if (orderDetailsView.mDogs.Count > 2)
                    ordersTbl.Price = -999;  //יותר מ 2 כלבים. מחיר ינתן בתיאום עם יוסף
                else
                {
                    if (orderDetailsView.Price == 0)
                        ordersTbl.Price = result;
                }
                if (orderDetailsView.Discount > 0)
                    ordersTbl.discount = orderDetailsView.Discount;
                else
                    ordersTbl.discount = 0;
                // List<OrderDetailsView> list = checkForAnotherParallelOrder(orderDetailsView);


                for (int i = 0; i < orderDetailsView.mDogs.Count; i++)
                {
                    //if (orderDetailsView.mDogs[i].Training)

                    //{
                    DogsInOrder dogsInOrder = new DogsInOrder();
                    dogsInOrder.DogTraining = orderDetailsView.mDogs[i].DogTraining;
                    dogsInOrder.DogNumber = orderDetailsView.mDogs[i].DogNumber;

                    dogsInOrder.HomeFood = orderDetailsView.mDogs[i].HomeFood;

                    dogsInOrder.Status = 21; //פעיל בהזמנה, אם יבוטל ישתנה הקוד ל 23
                    ordersTbl.DogsInOrder.Add(dogsInOrder);
                    //}

                }

                
                context.OrdersTbl.Add(ordersTbl);
                context.SaveChanges();
                //שליפת מספר ההזמנה שנוצרה
                result = GetLastOrder(orderDetailsView.Userid);
                //שליחת מייל למשתמש
                SendMailService sendMailService = new SendMailService();
                SendMailRequest mailRequest = new SendMailRequest();
                mailRequest.recipient = orderDetailsView.User.UserEmail;
                mailRequest.subject = "קליטת הזמנה - " + (int)result + "מקום טוב- יוסף טוויטו";
                mailRequest.body = " הזמנתך נקלטה";

                sendMailService.SendMail(mailRequest);
                //שליחת מייל ליוסף
                SendMailService sendMailServiceMT = new SendMailService();
                SendMailRequest mailRequestMT = new SendMailRequest();
                mailRequest.recipient = "makomtovapp@gmail.com";
                mailRequest.subject = " קליטת הזמנה  - " + (int)result + "מקום טוב- יוסף טוויטו";
                mailRequest.body =string.Format(" הזמנה מלקוח {0} נקלטה במערכת ", orderDetailsView.userFirstName+" "+orderDetailsView.userLastName);
                mailRequest.body += "\n";
                mailRequest.body += string.Format("מתאריך {0} , משמרת {1} עד תאריך {2} , משמרת {3}", ((DateTime)ordersTbl.FromDate).ToShortDateString(), ordersTbl.ShiftNumberFrom, ((DateTime)ordersTbl.ToDate).ToShortDateString(), ordersTbl.ShiftNumberTo);
                mailRequest.body += "\n";
                mailRequest.body += string.Format("מספר כלבים בהזמנה - {0}",ordersTbl.DogsInOrder.Count);
                sendMailService.SendMail(mailRequest);

            }
            return (int)result;
        }

        
        private decimal checkForAnotherParallelOrder(OrderDetailsView orderDetailsView)
        //private List<OrderDetailsView> checkForAnotherParallelOrder(OrderDetailsView orderDetailsView)
        {
            decimal result = 1;
            //orderDetailsView.FromDate = new DateTime(2018, 1, 20);
            //orderDetailsView.ToDate = new DateTime(2018, 2, 20);
            List<OrderDetailsView> list = new List<OrderDetailsView>();
            try
            {
                using (DatabaseEntitiesMT context = new DatabaseEntitiesMT())
                {
                    List<OrdersTbl> orders = context.OrdersTbl.Where(p => p.OrderUserId == orderDetailsView.Userid && (p.OrderStatus==12 || p.OrderStatus==11 || p.OrderStatus==15)
                                                                      &&( (orderDetailsView.FromDate >= p.FromDate && orderDetailsView.FromDate <= p.ToDate)
                                                                       || (orderDetailsView.ToDate >= p.FromDate && orderDetailsView.ToDate <= p.ToDate)
                                                                      || (orderDetailsView.FromDate <= p.FromDate && orderDetailsView.ToDate > p.ToDate)
                                                                      )
                                                                     ).ToList();

                    if (orders != null) //יש הזמנות חופפות
                    {
                        //if (orderDetailsView.mDogs.Count == 2)
                        //    return -998; // מספר כלבים בהזמנה = 2 ויש הזמנות חופפות.
                        OrderDetailsView orderDetailsViewDB = new OrderDetailsView();
                        foreach (var order in orders)
                        {
                            orderDetailsViewDB.mDogs = GetDogsForOrder(order.OrderNumber); //שליפת כלבים בהזמנה החופפת
                            List<int> nums = new List<int>();
                            foreach (DogsInOrderView d in orderDetailsViewDB.mDogs)
                            {
                                nums.Add(d.DogNumber);
                            }


                            foreach (DogsInOrderView dog in orderDetailsView.mDogs)
                            {
                                var found = nums.Where(d => d == dog.DogNumber); //בדיקה אם יש כלב משותף בהזמנה הנוכחית ןבהזמנות קודמות
                                if (found.Count()>0)  //   אם כן מחזיר שגיאה
                                    return -997;
                               

                            }

                           
                            //orderDetailsViewDB.FromDate = (DateTime)order.FromDate;
                            //orderDetailsViewDB.OrderNumber = order.OrderNumber;
                            //orderDetailsViewDB.ShiftNumberFrom = (int)order.ShiftNumberFrom;
                            //orderDetailsViewDB.ShiftNumberTo = (int)order.ShiftNumberTo;
                            
                            //orderDetailsViewDB.ToDate = (DateTime)order.ToDate;
                            
                            //list.Add(orderDetailsViewDB);
                           

                        }
                       // result = CalculateOrderPriceParallel(orderDetailsView, list);
                    }
                    result = CalculateOrderPrice(orderDetailsView); //חישוב רגיל

                }
               ; //נמצאה בבסיס הנתונים הזמנה חופפת עם כלבים אחרים
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            //return list;
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
        /// <summary>
        /// עדכון / הוספת הזמנות על-ידי מנהל
        /// </summary>
        /// <param name="OrdersList"></param>
        public void UpdateOrdersByManager(List<OrderDetailsViewManager> OrdersList)
        {
            try
            {
                using (DatabaseEntitiesMT context = new DatabaseEntitiesMT())
                {
                    foreach (OrderDetailsViewManager order in OrdersList)
                    {
                        var ordert = context.Set<OrdersTbl>().Find(order.OrderNumber);

                        if (ordert != null)
                        {

                            context.Entry(ordert).CurrentValues.SetValues(order);
                            foreach (DogsInOrderView dog in order.mDogs)
                            {
                                var dogt = context.Set<DogsInOrder>().Find(order.OrderNumber,dog.DogNumber);
                                context.Entry(dogt).CurrentValues.SetValues(dog);
                            }
                            context.SaveChanges();
                        }
                        else
                        {
                          //  order.User=
                            createOrder(order);
                        }
                    }
                    context.Dispose();
                }
               
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

        //public decimal CalculateOrderPrice()

        //{
        //    OrderDetailsView order = new OrderDetailsView();
        //    order.mDogs = new List<DogsInOrderView>();
        //    order.mDogs.Add(new DogsInOrderView());
        //    order.mDogs.Add(new DogsInOrderView());
        //    order.mDogs[0].FromDate = new DateTime(2018, 1, 1);
        //    order.mDogs[0].ToDate = new DateTime(2018, 1, 15);
        //    order.mDogs[1].FromDate = new DateTime(2018, 1, 20);
        //    order.mDogs[1].ToDate = new DateTime(2018, 2, 20);
        //    DatabaseEntitiesMT context = new DatabaseEntitiesMT();

        //    Decimal price = 0;
        //    var prices = context.PricesTbl
        //    .OrderBy(p => p.Days).Select(np => new PricesView
        //    {
        //        Days = np.Days,
        //        Dogs = np.Dogs,
        //        Price = np.Price
        //    });
        //    int days = 0;
        //    if ((order.mDogs.Count == 2 && order.mDogs[0].FromDate == order.mDogs[1].FromDate && order.mDogs[0].ToDate == order.mDogs[1].ToDate) || order.mDogs.Count == 1)
        //    {
        //        days = order.mDogs[0].ToDate.Subtract(order.mDogs[0].FromDate).Days;
        //        if (order.mDogs[0].ShiftNumberTo == 2)
        //            days++;
        //        List<PricesView> p = prices.Where(o => o.Dogs == order.mDogs.Count).ToList();
        //        price = Calculte(days, p);

        //    }

        //    //לטפל ב 2 כלבים עם תאריכים שונים  
        //    else
        //    {
        //        DateTime dateTimeFrom, dateTimeTo;
        //        if (order.mDogs[0].FromDate.CompareTo(order.mDogs[1].FromDate) < 0)
        //        {
        //            dateTimeFrom = order.mDogs[0].FromDate;
        //        }
        //        else
        //        {
        //            dateTimeFrom = order.mDogs[1].FromDate;
        //        }
        //        if (order.mDogs[0].ToDate.CompareTo(order.mDogs[1].ToDate) < 0)
        //        {
        //            dateTimeTo = order.mDogs[1].ToDate;
        //        }
        //        else
                //{
                //    dateTimeTo = order.mDogs[0].ToDate;
                //}

                //days = dateTimeTo.Subtract(dateTimeFrom).Days;
                //if (order.mDogs[0].ShiftNumberTo == 2)
                //    days++;
                //int[] arr = new int[days];
                //for (int i = 0; i < arr.Length; i++)
                //{
                //    arr[i] = -1;
                //}
                //DateTime date = order.mDogs[0].ToDate;
                ////טיפול בהחזרה במשמרת בוקר- לא מחשיבים את היום לתשלום
                //if (order.mDogs[0].ShiftNumberTo == 1)
                //    date.AddDays(-1);
                //changeArr(arr, order.mDogs[0].FromDate, date, dateTimeFrom);
                //if (order.mDogs[1].ShiftNumberTo == 1)
                //    date.AddDays(-1);
                //changeArr(arr, order.mDogs[1].FromDate, date, dateTimeFrom);

                //int dogs2 = 0;
                //for (int i = 0; i < arr.Length; i++)
                //{
                //    dogs2 += arr[i];
                //}
                //List<PricesView> p;

                //if (dogs2 > 0) //פתרון הבעיה שיש בהזמנה 2 כלבים אבל התאריכים לא חופפים ולכן יש בעצם פעמיים כלב 1
                //{
                //    //חישוב מספר הימים ל 2 כלבים
                //    p = prices.Where(o => o.Dogs == 2).ToList();
                //    price += Calculte(dogs2, p);
                //}
                //else
                //    dogs2 *= -1;
                //int dogs1 = arr.Length - dogs2;
                ////חישוב מספר הימים לכלב 1
                //p = prices.Where(o => o.Dogs == 1).ToList();
                //price = Calculte(dogs1, p);




            //}
            //return price;
        //}
        //private void changeArr(int[] arr, DateTime min, DateTime max, DateTime first)
        //{
        //    int i = 0;
        //    for (DateTime d = min; d <= max; d = d.AddDays(1))
        //    {
        //        i = d.Subtract(first).Days;
        //        arr[i]++;
        //    }
        //}


        //חישוב ההזמנה
        // public decimal CalculateOrderPrice(OrderDetailsView order)
        public decimal CalculateOrderPrice(OrderDetailsView order)
        {
            // חישוב ההזמנה ללא הזמנות אחרות חופפות
            //OrderDetailsView order = new OrderDetailsView();
            //order.mDogs = new List<DogsInOrderView>();
            //order.mDogs.Add(new DogsInOrderView());
            //order.mDogs.Add(new DogsInOrderView());
            //order.mDogs[0].FromDate = new DateTime(2018, 1, 1);
            //order.mDogs[0].ToDate = new DateTime(2018, 1, 15);
            //order.mDogs[1].FromDate = new DateTime(2018, 1, 20);
            //order.mDogs[1].ToDate = new DateTime(2018, 2, 20);
            DatabaseEntitiesMT context = new DatabaseEntitiesMT();

            Decimal price = 0;
            var prices = context.PricesTbl
            .OrderBy(px => px.Days).Select(np => new PricesView
            {
                Days = np.Days,
                Dogs = np.Dogs,
                Price = np.Price
            });
            int days = 0;
            // if ((order.mDogs.Count == 2 && order.mDogs[0].FromDate == order.mDogs[1].FromDate && order.mDogs[0].ToDate == order.mDogs[1].ToDate) || order.mDogs.Count == 1)
            //if ((order.mDogs.Count == 2  || order.mDogs.Count == 1)
            //{

                days = order.ToDate.Subtract(order.FromDate).Days;
                if (order.ShiftNumberTo == 2)  //אם לוקח את הכלבים בערב - מחושב יום נוסף בתשלום
                    days++;
                List<PricesView> p = prices.Where(o => o.Dogs == order.mDogs.Count).ToList();
                price = Calculte(days, p);

            //}
            return price;
        }

        public decimal CalculateOrderPriceParallel(OrderDetailsView order,List<OrderDetailsView>  orderDetailsViewDBList)

        {// חישוב עבור הזמנות חופפות
            int days = 0;
            decimal price = 0;
            days = order.ToDate.Subtract(order.FromDate).Days;
            if (order.ShiftNumberTo == 2)  //אם לוקח את הכלבים בערב - מחושב יום נוסף בתשלום
                days++;
            int orderShift = order.ShiftNumberTo - 1;
            int[] arr = new int[days];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = 1;
            }
            int dbshift;
            for (int i = 0; i < orderDetailsViewDBList.Count; i++)
            {
                 dbshift = orderDetailsViewDBList[i].ShiftNumberTo - 1;
                if (orderDetailsViewDBList[i].FromDate.CompareTo(order.FromDate) >= 0 && orderDetailsViewDBList[i].FromDate.CompareTo(order.ToDate.AddDays(orderShift)) <= 0 &&
                   orderDetailsViewDBList[i].ToDate.AddDays(dbshift).CompareTo(order.FromDate) >= 0 && orderDetailsViewDBList[i].ToDate.AddDays(dbshift).CompareTo(order.ToDate.AddDays(orderShift)) <= 0)  // תאריך תחילת ההזמנה בבסיס הנתונים נופל בתוך תאריך ההזמנה החדשה
                {
                    changeArr(arr, orderDetailsViewDBList[i].FromDate, orderDetailsViewDBList[i].ToDate.AddDays(dbshift), order.FromDate, orderDetailsViewDBList[i].mDogs.Count);
                }

                else
                if (order.FromDate.CompareTo(orderDetailsViewDBList[i].FromDate) >= 0 && order.FromDate.CompareTo(orderDetailsViewDBList[i].ToDate.AddDays(dbshift)) <= 0 &&
                   order.ToDate.AddDays(orderShift).CompareTo(orderDetailsViewDBList[i].FromDate) >= 0 && order.ToDate.AddDays(orderShift).CompareTo(orderDetailsViewDBList[i].ToDate.AddDays(dbshift)) <= 0)
                {
                    changeArr(arr, order.FromDate, order.ToDate.AddDays(orderShift), order.FromDate, orderDetailsViewDBList[i].mDogs.Count);
                }
                else
                if (orderDetailsViewDBList[i].FromDate.CompareTo(order.FromDate) >= 0 && orderDetailsViewDBList[i].FromDate.CompareTo(order.ToDate.AddDays(orderShift)) <= 0)
                { changeArr(arr, orderDetailsViewDBList[i].FromDate, order.ToDate.AddDays(orderShift), order.FromDate, orderDetailsViewDBList[i].mDogs.Count); }
                else
                    if (orderDetailsViewDBList[i].ToDate.AddDays(dbshift).CompareTo(order.FromDate) >= 0 && orderDetailsViewDBList[i].ToDate.AddDays(dbshift).CompareTo(order.ToDate.AddDays(orderShift)) <= 0)
                { changeArr(arr, order.FromDate, orderDetailsViewDBList[i].ToDate.AddDays(dbshift), order.FromDate, orderDetailsViewDBList[i].mDogs.Count); }
            }

            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] > 2) return -997;  //הזמנה נוכחית + קודמות- יותר מ 2 כלבים
            }
            int dog1 = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == 1) dog1++;
            }

            DatabaseEntitiesMT context = new DatabaseEntitiesMT();
            var prices = context.PricesTbl
           .OrderBy(px => px.Days).Select(np => new PricesView
           {
               Days = np.Days,
               Dogs = np.Dogs,
               Price = np.Price
           });
            List<PricesView> p = prices.Where(o => o.Dogs == 1).ToList();
            price += Calculte(dog1, p);
            p= prices.Where(o => o.Dogs == 2).ToList();
            for(int i=0;i<p.Count();i++)
            { p[i].Price = p[i].Price / 2; }
            price += Calculte(arr.Length-dog1, p);
            return price;

            //DateTime dateTimeFrom, dateTimeTo;
            //if (order.mDogs[0].FromDate.CompareTo(order.mDogs[1].FromDate) < 0)
            //{
            //    dateTimeFrom = order.mDogs[0].FromDate;
            //}
            //else
            //{
            //    dateTimeFrom = order.mDogs[1].FromDate;
            //}
            //if (order.mDogs[0].ToDate.CompareTo(order.mDogs[1].ToDate) < 0)
            //{
            //    dateTimeTo = order.mDogs[1].ToDate;
            //}
            //else
            //{
            //    dateTimeTo = order.mDogs[0].ToDate;
            //}

            // days = dateTimeTo.Subtract(dateTimeFrom).Days;
            //if (order.mDogs[0].ShiftNumberTo == 2)
            //    days++;

            //DateTime date = order.mDogs[0].ToDate;

            //טיפול בהחזרה במשמרת בוקר- לא מחשיבים את היום לתשלום
            //if (order.mDogs[0].ShiftNumberTo == 1)
            //    date.AddDays(-1);
            //changeArr(arr, order.mDogs[0].FromDate, date, dateTimeFrom);
            //if (order.mDogs[1].ShiftNumberTo == 1)
            //    date.AddDays(-1);
            //changeArr(arr, order.mDogs[1].FromDate, date, dateTimeFrom);

            //int dogs2 = 0;
            //for (int i = 0; i < arr.Length; i++)
            //{
            //    dogs2 += arr[i];
            //}


            //if (dogs2 > 0) //פתרון הבעיה שיש בהזמנה 2 כלבים אבל התאריכים לא חופפים ולכן יש בעצם פעמיים כלב 1
            //{
            //    //חישוב מספר הימים ל 2 כלבים
            //    p = prices.Where(o => o.Dogs == 2).ToList();
            //    price += Calculte(dogs2, p);
            //}
            //else
            //    dogs2 *= -1;

            //int dogs1 = arr.Length - dogs2;
            ////חישוב מספר הימים לכלב 1
            //p = prices.Where(o => o.Dogs == 1).ToList();
            //price = Calculte(dogs1, p);




        }
           
       

        private void changeArr(int[] arr, DateTime min, DateTime max, DateTime first,int numDogsInDBOrder)
        {
            int i = 0;
            for (DateTime d = min; d <= max; d = d.AddDays(1))
            {
                i = d.Subtract(first).Days;
                arr[i]+=numDogsInDBOrder;
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


            //   שליפת כל ההזמנות מטבלתOrdersTbl

          
            try
            {
                DatabaseEntitiesMT context = new DatabaseEntitiesMT();
               
                    List<OrderDetailsView> orderslist = GetOrdersFromDB();
                    OrdersForUserView ordersForUser = null;
                if (orderslist != null)
                {
                    ordersForUser = new OrdersForUserView();
                    ordersForUser.UserReservations = orderslist.Where(p => p.Userid == userid).ToList();
                    for(int i=0;i< ordersForUser.UserReservations.Count;i++) 
                    {
                      
                        int orderNum = ordersForUser.UserReservations[i].OrderNumber;
                      
                        ordersForUser.UserReservations[i].mDogs = GetDogsForOrder(orderNum);
                       
                    }
                }
                return ordersForUser;

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<DogsInOrderView> GetDogsForOrder(int orderNumber)
        {
            try
            {
                DatabaseEntitiesMT context = new DatabaseEntitiesMT();
                List<DogsInOrderView> dogsInOrderList = new List<DogsInOrderView>();
                var dogsInOrder = context.DogsInOrder.Join(context.UserDogs,
                         d => d.DogNumber, ud => ud.DogNumber,
                         (d, ud) => new
                         {
                             d.OrderNumber,
                             d.DogNumber,
                             d.UserDogs.DogName,
                             d.DogTraining,
                             d.HomeFood,
                             d.UserDogs.DogImage,
                             d.Status,
                             d.UserDogs.DogRabiesVaccine,
                             ud.DogType,
                             ud.DogBirthDate,
                             ud.DogComments,
                             ud.DogDig,
                             ud.DogFriendlyWith,
                             ud.DogGender,
                             ud.DogJump,
                             ud.DogNeuter,
                             ud.DogShvav
                         }).Where(or => orderNumber == or.OrderNumber);
                // ordersForUser.UserReservations[i].mDogs = dogsInOrder.ToList();
                foreach (var dog in dogsInOrder)
                {
                    DogsInOrderView dogsInOrderV = new DogsInOrderView();
                    dogsInOrderV.DogRabiesVaccine = dog.DogRabiesVaccine;
                    dogsInOrderV.DogNumber = dog.DogNumber;
                    dogsInOrderV.DogName = dog.DogName; ;
                    dogsInOrderV.OrderNumber = dog.OrderNumber;
                    dogsInOrderV.DogImage = dog.DogImage;
                    dogsInOrderV.DogTraining =(bool)dog.DogTraining;
                    dogsInOrderV.DogBirthDate = dog.DogBirthDate;
                    dogsInOrderV.DogType = dog.DogType;
                    dogsInOrderV.DogComments = dog.DogComments;
                    dogsInOrderV.DogDig = (bool)dog.DogDig;
                    dogsInOrderV.DogFriendlyWith = (int)dog.DogFriendlyWith;
                    dogsInOrderV.DogGender = dog.DogGender;
                    dogsInOrderV.DogJump = (bool)dog.DogJump;
                    dogsInOrderV.DogNeuter = dog.DogNeuter;
                    dogsInOrderV.DogShvav = dog.DogShvav;
                    dogsInOrderV.Status = dog.Status;
                    dogsInOrderV.Pension = true;
                    dogsInOrderV.HomeFood = dog.HomeFood;
                    dogsInOrderList.Add(dogsInOrderV);
                }
                return dogsInOrderList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserDetailsView GetUserOrders(int UserID)
        {
            UserDetailsView userDetails = new UserDetailsView();
           
                //   שליפת כל ההזמנות מטבלתOrdersTbl
                try
                {
                Userservice userservice = new Userservice();
                userDetails = userservice.GetUser(UserID);
                OrdersForUserView ordersForUserView = GetUserOrdersList(UserID);
                userDetails.UserReservations = ordersForUserView.UserReservations;
                
                return userDetails;
               

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }


    }

    }

