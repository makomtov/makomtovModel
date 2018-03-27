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
        //����� �� ������� �� �� ��������
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
                           ot.OrderTypeName,
                           o.ManagerComments

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
                                   a.OrderType,
                                   a.OrderTypeName,
                                   a.ManagerComments

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
                                   OrderType = o.OrderType,
                                   OrderTypeName = o.OrderTypeName,
                                   ManagerComments = o.ManagerComments

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

        //����� �� ������� 
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

        //����� �� ������� ��� ������ �������
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


                        //����� �� ������ ������ �����  DogsInOrder
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

        // ����� �� ������� ��� ������ �������- ����
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


                        //����� �� ������ ������ �����  DogsInOrder
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
        //����� �����
         public int createOrder(OrderDetailsView orderDetailsView)
       
        {
            Userservice userservice = new Userservice();
            //OrderDetailsView orderDetailsView = new OrderDetailsView();
            //orderDetailsView.Userid = 1;
            //orderDetailsView.FromDate = new DateTime(2018, 3, 28);
            //orderDetailsView.ToDate = new DateTime(2018, 4, 3);
            //orderDetailsView.mDogs = new List<DogsInOrderView>();
            //orderDetailsView.mDogs.Add(new DogsInOrderView());
          //  orderDetailsView.mDogs.Add(new DogsInOrderView());
            //orderDetailsView.mDogs[0].DogNumber = 1;
         //   orderDetailsView.mDogs[1].DogNumber = 8;
            OrdersTbl ordersTbl = new OrdersTbl();
            if (orderDetailsView.mDogs.Count > 2)
                ordersTbl.Price = -999;  //���� � 2 �����. ���� ���� ������ �� ����
           //if(orderDetailsView.mDogs.Count==2)
           //     ordersTbl.Price = CalculateOrderPrice(orderDetailsView);

            decimal result = checkForAnotherParallelOrder(orderDetailsView);
           
            ordersTbl.OrderStatus = 11;
            ordersTbl.OrderUserId = ((UserDetailsView)HttpContext.Current.Session["userDetails"]).UserID;
            ordersTbl.OrderType = 1;
            // List<OrderDetailsView> list = checkForAnotherParallelOrder(orderDetailsView);

           
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


            context.OrdersTbl.Add(ordersTbl);
            context.SaveChanges();
            //����� ���� ������ ������
            int orderID = GetLastOrder(orderDetailsView.User.UserID);
            //����� ���� ������
            SendMailService sendMailService = new SendMailService();
            SendMailRequest mailRequest = new SendMailRequest();
            mailRequest.recipient = orderDetailsView.User.UserEmail;
            mailRequest.subject = "����� ����� - " + orderID + "���� ���- ���� ������";
            mailRequest.body = " ������ �����";
            sendMailService.SendMail(mailRequest);
            return orderID;

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
                    List<OrdersTbl> orders = context.OrdersTbl.Where(p => p.OrderUserId == orderDetailsView.Userid
                                                                      && (orderDetailsView.FromDate >= p.FromDate && orderDetailsView.FromDate <= orderDetailsView.ToDate)
                                                                       || (orderDetailsView.ToDate >= p.FromDate && orderDetailsView.ToDate <= orderDetailsView.ToDate)
                                                                      || (orderDetailsView.FromDate <= p.FromDate && orderDetailsView.ToDate > orderDetailsView.ToDate)
                                                                     ).ToList();

                    if (orders != null) //�� ������ ������
                    {
                        if (orderDetailsView.mDogs.Count == 2)
                            return -998; // ���� ����� ������ = 2 ��� ������ ������.
                        OrderDetailsView orderDetailsViewDB = new OrderDetailsView();
                        foreach (var order in orders)
                        {
                            orderDetailsViewDB.mDogs = GetDogsForOrder(order.OrderNumber); //����� ����� ������ ������
                            List<int> nums = new List<int>();
                            foreach (DogsInOrderView d in orderDetailsViewDB.mDogs)
                            {
                                nums.Add(d.DogNumber);
                            }
                           
                           
                            foreach (DogsInOrderView dog in orderDetailsView.mDogs)
                            {
                                var found = nums.Where(d => d == dog.DogNumber); //����� �� �� ��� ����� ������ ������� �������� ������
                                if (found.Count()>0)  //   �� �� ����� �����
                                    return 3;
                               

                            }

                           
                            orderDetailsViewDB.FromDate = (DateTime)order.FromDate;
                            orderDetailsViewDB.OrderNumber = order.OrderNumber;
                            orderDetailsViewDB.ShiftNumberFrom = (int)order.ShiftNumberFrom;
                            orderDetailsViewDB.ShiftNumberTo = (int)order.ShiftNumberTo;
                            
                            orderDetailsViewDB.ToDate = (DateTime)order.ToDate;
                            
                            list.Add(orderDetailsViewDB);
                           

                        }
                        result = CalculateOrderPriceParallel(orderDetailsView, list);
                    }
                    result = CalculateOrderPrice(orderDetailsView); //����� ����

                }
               ; //����� ����� ������� ����� ����� �� ����� �����
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            //return list;
        }
        //����� ����� ������ ������
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




        //����� ������� �� �����
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




        //����� ������
        // public decimal CalculateOrderPrice(OrderDetailsView order)
        public decimal CalculateOrderPrice(OrderDetailsView order)
        {
            // ����� ������ ��� ������ ����� ������
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
                if (order.ShiftNumberTo == 2)  //�� ���� �� ������ ���� - ����� ��� ���� ������
                    days++;
                List<PricesView> p = prices.Where(o => o.Dogs == order.mDogs.Count).ToList();
                price = Calculte(days, p);

            //}
            return price;
        }

        public decimal CalculateOrderPriceParallel(OrderDetailsView order,List<OrderDetailsView>  orderDetailsViewDBList)

        {// ����� ���� ������ ������
            int days = 0;
            decimal price = 0;
            days = order.ToDate.Subtract(order.FromDate).Days;
            if (order.ShiftNumberTo == 2)  //�� ���� �� ������ ���� - ����� ��� ���� ������
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
                   orderDetailsViewDBList[i].ToDate.AddDays(dbshift).CompareTo(order.FromDate) >= 0 && orderDetailsViewDBList[i].ToDate.AddDays(dbshift).CompareTo(order.ToDate.AddDays(orderShift)) <= 0)  // ����� ����� ������ ����� ������� ���� ���� ����� ������ �����
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
                if (arr[i] > 2) return -997;  //����� ������ + ������- ���� � 2 �����
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

            //����� ������ ������ ����- �� ������� �� ���� ������
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


            //if (dogs2 > 0) //����� ����� ��� ������ 2 ����� ��� �������� �� ������ ���� �� ���� ������ ��� 1
            //{
            //    //����� ���� ����� � 2 �����
            //    p = prices.Where(o => o.Dogs == 2).ToList();
            //    price += Calculte(dogs2, p);
            //}
            //else
            //    dogs2 *= -1;

            //int dogs1 = arr.Length - dogs2;
            ////����� ���� ����� ���� 1
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
        //����� ������ ������

        public OrdersForUserView GetUserOrdersList(int userid)
        {


            //   ����� �� ������� �����OrdersTbl

          
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
                             d.FromDate,
                             d.ToDate,
                             d.UserDogs.DogImage,
                             d.ShiftNumberFrom,
                             d.ShiftNumberTo,
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
                    dogsInOrderV.FromDate = dog.FromDate;
                    dogsInOrderV.ToDate = dog.ToDate;
                    dogsInOrderV.DogImage = dog.DogImage;
                    dogsInOrderV.ShiftNumberFrom = dog.ShiftNumberFrom;
                    dogsInOrderV.ShiftNumberTo = dog.ShiftNumberTo;
                    dogsInOrderV.DogBirthDate = dog.DogBirthDate;

                    dogsInOrderV.DogComments = dog.DogComments;
                    dogsInOrderV.DogDig = (bool)dog.DogDig;
                    dogsInOrderV.DogFriendlyWith = (int)dog.DogFriendlyWith;
                    dogsInOrderV.DogGender = dog.DogGender;
                    dogsInOrderV.DogJump = (bool)dog.DogJump;
                    dogsInOrderV.DogNeuter = dog.DogNeuter;
                    dogsInOrderV.DogShvav = dog.DogShvav;
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
           
                //   ����� �� ������� �����OrdersTbl
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

