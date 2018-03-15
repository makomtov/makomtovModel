using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Configuration;//
using System.Web;

namespace WebApiMTModel.Models.Models.View
{
    public class Userservice
    {
       
        // שליפת כל המשתמשים ללא כלבים
        public List<UserDetailsView> GetUsers()
        {
            try
            {
               
                DatabaseEntitiesMT context = new DatabaseEntitiesMT();
                List<UserDetailsView> list = new List<UserDetailsView>();
                List<UsersTbl> users = context.UsersTbl.ToList();
                    foreach (UsersTbl user in users)
                    {
                        if (user.UserStatus == 21)
                        {
                            UserDetailsView ud = new UserDetailsView();
                            ud.UserAddress = user.UserAddress;
                            ud.UserCityName = user.UserCity;
                            ud.UserComments = user.UserComments;
                            ud.UserEmail = user.UserEmail;
                            ud.UserFirstName = user.UserFirstName;
                            ud.UserID = user.UserID;
                            ud.UserLastName = user.UserLastName;
                            ud.UserName = user.UserName;
                            ud.UserPhone1 = user.UserPhone1;
                            ud.UserPhone2 = user.UserPhone2;
                            list.Add(ud);
                        }
                    }
                    return list;
                //}

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        //שליפת משתמש+ כלבים למשתמש+הזמנות למשתמש
        public UserDetailsView GetUser(string usereMail, string password)
        {

            try
            {
                UserDetailsView userDetails = null;

                DatabaseEntitiesMT context = new DatabaseEntitiesMT();


                var User = context.UsersTbl
              .Where(user => user.UserEmail == usereMail && user.UserPaswrd == password).FirstOrDefault();


                if (User != null)
                {
                    userDetails = new UserDetailsView();

                    userDetails.UserFirstName = User.UserFirstName;
                    userDetails.UserLastName = User.UserLastName;
                    userDetails.UserEmail = User.UserEmail;
                    userDetails.UserID = User.UserID;
                    userDetails.UserPhone2 = User.UserPhone2;
                    userDetails.UserPhone1 = User.UserPhone1;
                    userDetails.UserComments = User.UserComments;
                    userDetails.UserCityName = User.UserCity;
                    userDetails.UserAddress = User.UserAddress;

               //     GetUserDogs(userDetails); //שליפת כלבים למשתמש
               //     GetUserOrders(userDetails);//שליפת הזמנות למשתמש
                    HttpContext.Current.Session["userDetails"] = userDetails;


                }
                return userDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //שליפת הזמנות למשתמש
        
        public void GetUserOrders(UserDetailsView userDetails)
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
                           }).Where(p => p.OrderUserId == userDetails.UserID);

               // var orders = context.OrdersTbl.Where(p => p.OrderUserId == userDetails.UserID);
                if (orderslist != null)
                {
                    userDetails.UserReservations= new List<OrderDetailsView>();
                     
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
                            orderDetails.mDogs.Add(dogsInOrderV);
                        }
                        userDetails.UserReservations.Add(orderDetails);
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //שליפת הזמנות למשתמש

        public UserDetailsView GetUserOrders(int userID)
        {
            UserDetailsView userDetails = new UserDetailsView();
            userDetails.UserID = userID;
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
                          }).Where(p => p.OrderUserId == userID);

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
                            orderDetails.mDogs.Add(dogsInOrderV);
                        }
                        userDetails.UserReservations.Add(orderDetails);
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return userDetails;
        }
        //שליפת כלבים למשתמש
        private void GetUserDogs(UserDetailsView userDetails)


        {
            try
            {
                DatabaseEntitiesMT context = new DatabaseEntitiesMT();
                var dogs = context.UserDogs.Where(p => p.DogUserID == userDetails.UserID);
                if (dogs != null)
                {
                    userDetails.UserarrayDogs = new List<DogDetailsView>();
                    foreach (var dog in dogs)
                    {
                        DogDetailsView dogDetails = new DogDetailsView();
                        dogDetails.DogNumber = dog.DogNumber;
                        dogDetails.DogName = dog.DogName;
                        dogDetails.DogImage = dog.DogImage;
                        dogDetails.DogShvav = dog.DogShvav;
                        dogDetails.DogType = dog.DogType;
                        dogDetails.DogStatus = dog.DogStatus;
                        dogDetails.DogComments = dog.DogComments;
                        dogDetails.DogUserID = dog.DogUserID;
                        dogDetails.DogBirthDate = dog.DogBirthDate;
                        userDetails.UserarrayDogs.Add(dogDetails);
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
//        public void GetUserDogs(UserDetailsView userDetails)
//        {
//            try
//            {
//                DatabaseEntitiesMT context = new DatabaseEntitiesMT();
//        var dogs = context.UserDogs.Where(p => p.DogUserID == userDetails.UserID);
//                if (dogs != null)
//                {
//                    userDetails.UserarrayDogs = new List<DogDetailsView>();
//                    foreach (var dog in dogs)
//                    {
//                        DogDetailsView dogDetails = new DogDetailsView();
//        dogDetails.DogNumber = dog.DogNumber;
//                        dogDetails.DogName = dog.DogName;
//                        dogDetails.DogImage = dog.DogImage;
//                        dogDetails.DogShvav = dog.DogShvav;
//                        dogDetails.DogType = dog.DogType;
//                        dogDetails.DogStatus = dog.DogStatus;
//                        dogDetails.DogComments = dog.DogComments;
//                        dogDetails.DogUserID = dog.DogUserID;
//                        dogDetails.DogBirthDate = dog.DogBirthDate;
//                        userDetails.UserarrayDogs.Add(dogDetails);
//                    }
//}


//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }
        //שליפת משתמשים+כלבים
        //עדכון משתמש
        public void UpdateUserDetails(UserDetailsView user)

        {
            string sql = "UPDATE UsersTbl SET FirstName = [@FirstName], LastName = [@LastName], CityID = [@cityID], address = [@address], state = [@state], zipCode = [@zipCode] WHERE UserID=[@UserID]";
            //

            //string connectionString = Connect.getConnectionString();

            SqlConnection objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand objCmd;
            objCmd = new SqlCommand(sql, objConn);

            //set parameters for storde procedure
            SqlParameter objParam;
            objParam = objCmd.Parameters.Add("@UserID", SqlDbType.Int);
            objParam.Direction = ParameterDirection.Input;
            objParam.Value = user.UserID;
            //objParam = objCmd.Parameters.Add("@FirstName", SqlDbType.Char);
            //objParam.Direction = ParameterDirection.Input;
            //objParam.Value = user.UserFirstName;
            //objParam = objCmd.Parameters.Add("@LastName", SqlDbType.Char);
            //objParam.Direction = ParameterDirection.Input;
            //objParam.Value = user.UserLastName;
            //objParam = objCmd.Parameters.Add("@cityID", SqlDbType.Char);
            //objParam.Direction = ParameterDirection.Input;
            //objParam.Value = user.City;
            objParam = objCmd.Parameters.Add("@UserEMail", SqlDbType.Char);
            objParam.Direction = ParameterDirection.Input;
            objParam.Value = user.UserEmail;


            objParam = objCmd.Parameters.Add("@address", SqlDbType.Char);
            objParam.Direction = ParameterDirection.Input;
            objParam.Value = user.UserAddress;
            //objParam = objCmd.Parameters.Add("@UserPhone1", SqlDbType.Char);
            //objParam.Direction = ParameterDirection.Input;
            //objParam.Value = user.UserPhone;

            //objParam = objCmd.Parameters.Add("@state", SqlDbType.Char);
            //objParam.Direction = ParameterDirection.Input;
            //objParam.Value = user.State;
            //objParam = objCmd.Parameters.Add("@zipcode", SqlDbType.Char);
            //objParam.Direction = ParameterDirection.Input;
            //objParam.Value = user.ZipCode;

            try
            {
                objConn.Open();
                //objCmd.Connection.Open();
                int n = objCmd.ExecuteNonQuery();

            }
            catch (SqlException ex)
            { throw ex; }
            finally
            { objCmd.Connection.Close(); }
        }
        //הוספת משתמש
        public void InsertUserDetails(UserDetailsView user)

        {
            using (DatabaseEntitiesMT context = new DatabaseEntitiesMT())
            {
                UsersTbl usersTbl = new UsersTbl();
                usersTbl.UserAddress = user.UserAddress;
                usersTbl.UserCity = user.UserCityName;
                usersTbl.UserComments = user.UserComments;
                usersTbl.UserEmail = user.UserEmail;
                usersTbl.UserFirstName = user.UserFirstName;
                usersTbl.UserLastName = user.UserLastName;
                usersTbl.UserPaswrd = user.UserPaswrd;
                usersTbl.UserPhone1 = user.UserPhone1;
                usersTbl.UserPhone2 = user.UserPhone2;
                usersTbl.UserStatus = 21;
                usersTbl.UserName = user.UserName;

                context.UsersTbl.Add(usersTbl);
                context.SaveChanges();
            }

        }
        //public bool CheckUserDetails(int userid)
        //{
        //    bool ok = true;
        //    UserDetailsView userDetails = (UserDetailsView)HttpContext.Current.Session["userDetails"];
        //    if (userDetails.UserAddress==""
        //    return ok;
        //}

            //    public UserDogs GetUserDogsL(UserDetails userDetails)
            //    {
            //        DatabaseEntities2 databaseEntities2 = new DatabaseEntities2();

            //        List<UserDogs> UserDogslist = databaseEntities2.UserDogs.ToList();



            //        List<UserDogs> UserDogslist1 = from DogDetails in UserDogslist
            //               select DogDetails;

            //Console.WriteLine("Product Names:");
            //    foreach (var prod in productsQuery)
            //    {
            //        Console.WriteLine(prod.Name);
            //    }
            //}
        }
    }


