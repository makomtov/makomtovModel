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
                //  List<UserDetailsView> list = new List<UserDetailsView>();

                List<UserDetailsView> list = context.UsersTbl.
                      Join(context.veterinarTbl,
                      u => u.UserVeterinarId, v => v.VeterinarId,
                      (u, v) => new UserDetailsView
                      {
                          Acceptmessages = u.Acceptmessages,
                          DaysSumForDiscount = u.DaysSumForDiscount,
                          DogsNumber = u.UserDogs.Count,
                          UserAddress = u.UserAddress,
                          UserCityName = u.UserCity,
                          UserEmail = u.UserEmail,
                          UserComments = u.UserComments,
                          UserFirstName = u.UserFirstName,
                          UserName = u.UserName,
                          UserLastName = u.UserLastName,
                          UserID = u.UserID,
                          UserStatusCode = u.UserStatusCode,
                         // UserStatus=u.UserStatus,
                          UserPhone1 = u.UserPhone1,
                          UserPhone2 = u.UserPhone2,
                          UserVeterinarId = v.VeterinarId,
                          VeterinarAddress = v.VeterinarAddress,
                          VeterinarEmail = v.VeterinarEmail,
                          VeterinarCity = v.VeterinarCity,
                          VeterinarName = v.VeterinarName,
                          VeterinarPhone1 = v.VeterinarPhone1,
                          
                          ReservationsNumber=u.OrdersTbl.Count,

                      }).Where(users => users.UserStatusCode == 21).ToList();

               
               //List <UsersTbl> users = context.UsersTbl.ToList();
               // foreach (UsersTbl user in users)
               // {
               //     if (user.UserStatus == 21)
               //     {
               //         UserDetailsView ud = new UserDetailsView();
               //         ud.UserAddress = user.UserAddress;
               //         ud.UserCityName = user.UserCity;
               //         ud.UserComments = user.UserComments;
               //         ud.UserEmail = user.UserEmail;
               //         ud.UserFirstName = user.UserFirstName;
               //         ud.UserID = user.UserID;
               //         ud.UserLastName = user.UserLastName;
               //         ud.UserName = user.UserName;
               //         ud.UserPhone1 = user.UserPhone1;
               //         ud.UserPhone2 = user.UserPhone2;
               //         ud.UserComments = user.UserComments;
               //         
               //         var dogs = context.UserDogs
               //                      .Where(userDog => userDog.DogUserID == user.UserID).Count();
               //         ud.DogsNumber = dogs;

                      //         var reservations = context.OrdersTbl
                      //                      .Where(userres => userres.OrderUserId == user.UserID).Count();
                      //         ud.ReservationsNumber = reservations;
                      //         list.Add(ud);
                      //     }
                      // }
                return list;
                //}

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        //שליפת משתמש
        public UserDetailsView GetUser(string usereMail, string password)
        {

            try
            {
                UserDetailsView userDetails = null;

                DatabaseEntitiesMT context = new DatabaseEntitiesMT();


                var User = context.UsersTbl
              .Where(user => user.UserEmail == usereMail && user.UserPaswrd == password && user.UserStatusCode != 22).FirstOrDefault();


                if (User != null)
                {
                    userDetails = getUserDetails(context, User);

                }
                return userDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

       /// <summary>
       /// שליפת משתמש לפי מייל
       /// </summary>
       /// <param name="usereMail"></param>
       /// <returns>מחזיר אמת אם המשתמש קיים אחרת מחזיר שקר</returns>
        public bool GetUserByMail(string usereMail)
        {

            try
            {
                // UserDetailsView userDetails = null;
               
                DatabaseEntitiesMT context = new DatabaseEntitiesMT();


                var User = context.UsersTbl
              .Where(user => user.UserEmail == usereMail && user.UserStatusCode != 22).FirstOrDefault();

                return User != null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //שליפת משתמש לפי מייל ו FACEBOOK ID
        public UserDetailsView GetUserFB(string usereMail, string FBid)
        {

            try
            {
                UserDetailsView userDetails = null;

                DatabaseEntitiesMT context = new DatabaseEntitiesMT();


                var User = context.UsersTbl
              .Where(user => user.UserEmail == usereMail && user.UserName == FBid && user.UserStatusCode != 22).FirstOrDefault();


                if (User != null)
                {
                    userDetails = getUserDetails(context, User);

                }
                return userDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private static UserDetailsView getUserDetails(DatabaseEntitiesMT context, UsersTbl User)
        {
            try
            {
                UserDetailsView userDetails = new UserDetailsView();
                userDetails.DaysSumForDiscount = User.DaysSumForDiscount;
                userDetails.Acceptmessages = User.Acceptmessages;
                userDetails.UserFirstName = User.UserFirstName;
                userDetails.UserLastName = User.UserLastName;
                userDetails.UserEmail = User.UserEmail;
                userDetails.UserID = User.UserID;
                userDetails.UserPhone2 = User.UserPhone2;
                userDetails.UserPhone1 = User.UserPhone1;
                userDetails.UserComments = User.UserComments;
                userDetails.UserCityName = User.UserCity;
                userDetails.UserAddress = User.UserAddress;
                userDetails.UserName = User.UserName;
                userDetails.UserStatusCode = User.UserStatusCode;
                var vet = context.veterinarTbl
                    .Where(v => v.VeterinarId == User.UserVeterinarId).FirstOrDefault();
                userDetails.UserVeterinarId = User.UserVeterinarId;
                userDetails.VeterinarAddress = vet.VeterinarAddress;
                userDetails.VeterinarCity = vet.VeterinarCity;
                userDetails.VeterinarEmail = vet.VeterinarEmail;
                userDetails.VeterinarName = vet.VeterinarName;
                userDetails.VeterinarPhone1 = vet.VeterinarPhone1;

                var dogs = context.UserDogs
                                .Where(userDog => userDog.DogUserID == User.UserID && userDog.DogStatus == 21).Count();
                userDetails.DogsNumber = dogs;

                var reservations = context.OrdersTbl
                             .Where(userres => userres.OrderUserId == User.UserID).Count();
                userDetails.ReservationsNumber = context.OrdersTbl.Count();

                //HttpContext.Current.Session["userDetails"] = userDetails;
                return userDetails;
            }
            catch (Exception ex)
            { throw ex; }
        }

        //+כלבים שלו שליפת משתמש
        public UserDetailsView GetUserWithDogs(string usereMail, string password)
        {



            //  DatabaseEntitiesMT context = new DatabaseEntitiesMT();


            //  var User = context.UsersTbl
            //.Where(user => user.UserEmail == usereMail && user.UserPaswrd == password).FirstOrDefault();


            //  if (User != null)
            //  {
            //      userDetails = new UserDetailsView();

            //      userDetails.UserFirstName = User.UserFirstName;
            //      userDetails.UserLastName = User.UserLastName;
            //      userDetails.UserEmail = User.UserEmail;
            //      userDetails.UserID = User.UserID;
            //      userDetails.UserPhone2 = User.UserPhone2;
            //      userDetails.UserPhone1 = User.UserPhone1;
            //      userDetails.UserComments = User.UserComments;
            //      userDetails.UserCityName = User.UserCity;
            //      userDetails.UserAddress = User.UserAddress;
            //      var dogs = context.UserDogs
            //                      .Where(userDog => userDog.DogUserID == User.UserID).Count();
            //      userDetails.DogsNumber = dogs;

            //      var reservations = context.OrdersTbl
            //                   .Where(userres => userres.OrderUserId == User.UserID).Count();
            //      userDetails.ReservationsNumber = context.OrdersTbl.Count();
            try
            {
                UserDetailsView userDetails = GetUser(usereMail, password);
               
                GetUserDogs(userDetails); //שליפת כלבים למשתמש
                   
                 //   HttpContext.Current.Session["userDetails"] = userDetails;


             
                return userDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //שליפת משתמש
        public UserDetailsView GetUser(int userid)
        {

            try
            {
                UserDetailsView userDetails = null;

                DatabaseEntitiesMT context = new DatabaseEntitiesMT();


                var User = context.UsersTbl
              .Where(user => user.UserID == userid && user.UserStatusCode!=22).FirstOrDefault();


                if (User != null)
                {
                    userDetails = new UserDetailsView();
                    userDetails.Acceptmessages = User.Acceptmessages;
                    userDetails.DaysSumForDiscount = User.DaysSumForDiscount;
                    userDetails.UserFirstName = User.UserFirstName;
                    userDetails.UserLastName = User.UserLastName;
                    userDetails.UserEmail = User.UserEmail;
                    userDetails.UserID = User.UserID;
                    userDetails.UserPhone2 = User.UserPhone2;
                    userDetails.UserPhone1 = User.UserPhone1;
                    userDetails.UserComments = User.UserComments;
                    userDetails.UserCityName = User.UserCity;
                    userDetails.UserAddress = User.UserAddress;
                    var dogs = context.UserDogs
                                    .Where(userDog => userDog.DogUserID == User.UserID).Count();
                    userDetails.DogsNumber = dogs;

                    var reservations = context.OrdersTbl
                                 .Where(userres => userres.OrderUserId == User.UserID).Count();
                    userDetails.ReservationsNumber = context.OrdersTbl.Count();
                    userDetails.UserName = User.UserName;
                    userDetails.UserStatusCode = User.UserStatusCode;
                    var vet = context.veterinarTbl
                        .Where(v => v.VeterinarId == User.UserVeterinarId).FirstOrDefault();
                    userDetails.UserVeterinarId = User.UserVeterinarId;
                    userDetails.VeterinarAddress = vet.VeterinarAddress;
                    userDetails.VeterinarCity = vet.VeterinarCity;
                    userDetails.VeterinarEmail = vet.VeterinarEmail;
                    userDetails.VeterinarName = vet.VeterinarName;
                    userDetails.VeterinarPhone1 = vet.VeterinarPhone1;
                    
                    //  GetUserDogs(userDetails); //שליפת כלבים למשתמש
                    //   OrderService orderService = new OrderService();
                    //    orderService.GetUserOrders(userDetails);//שליפת הזמנות למשתמש
                    HttpContext.Current.Session["userDetails"] = userDetails;


                }
                return userDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<DogDetailsView> GetUserDogs(int userid)
        
        {
            try
            {
                UserDetailsView userDetailsView = new UserDetailsView(userid);
                GetUserDogs(userDetailsView);

                return userDetailsView.UserarrayDogs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        //שליפת כלבים למשתמש
        private void GetUserDogs(UserDetailsView userDetails)


        {
            try
            {
                DatabaseEntitiesMT context = new DatabaseEntitiesMT();
                var dogs = context.UserDogs.Where(p => p.DogUserID == userDetails.UserID && p.DogStatus == 21);
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
                        dogDetails.DogRabiesVaccine = dog.DogRabiesVaccine;
                        dogDetails.DogNeuter = dog.DogNeuter;
                        dogDetails.DogJump = (bool)dog.DogJump;
                        dogDetails.DogDig = (bool)dog.DogDig;
                        dogDetails.DogGender = dog.DogGender;
                        dogDetails.DogFriendlyWith =(int)dog.DogFriendlyWith;
                        
                        userDetails.UserarrayDogs.Add(dogDetails);

                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DogsForManagerView GetUserDogsForManager(int userid)
        {

            DogsForManagerView dogsForManagerView = new DogsForManagerView();
            try
            {
                DatabaseEntitiesMT context = new DatabaseEntitiesMT();
        var dogs = context.UserDogs.Where(p => p.DogUserID == userid && p.DogStatus==21);
                if (dogs != null)
                {

                    foreach (var dog in dogs)
                    {
                        DogDetailsViewManager dogDetails = new DogDetailsViewManager();
                        dogDetails.DogNumber = dog.DogNumber;
                        dogDetails.DogName = dog.DogName;
                        dogDetails.DogImage = dog.DogImage;
                        dogDetails.DogShvav = dog.DogShvav;
                        dogDetails.DogType = dog.DogType;
                        dogDetails.DogStatus = dog.DogStatus;
                        dogDetails.DogComments = dog.DogComments;
                        dogDetails.DogUserID = dog.DogUserID;
                        dogDetails.DogBirthDate = dog.DogBirthDate;
                        dogDetails.DogRabiesVaccine = dog.DogRabiesVaccine;
                        dogDetails.DogNeuter = dog.DogNeuter;
                        dogDetails.DogJump = (bool)dog.DogJump;
                        dogDetails.DogDig = (bool)dog.DogDig;
                        dogDetails.DogGender = dog.DogGender;
                        dogDetails.DogFriendlyWith = (int)dog.DogFriendlyWith;
                        dogDetails.ManagerComments = dog.ManagerComments;

                        dogsForManagerView.UserDogs.Add(dogDetails);

                    }
                }
                return dogsForManagerView;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       


        //עדכון משתמש
        public void UpdateUserDetails(UserDetailsView userDetails)

        {
            try
            {
                using (DatabaseEntitiesMT context = new DatabaseEntitiesMT())
                {
                    var user = context.Set<UsersTbl>().Find(userDetails.UserID);
                    userDetails.UserPaswrd = user.UserPaswrd;
                    if (user.UserVeterinarId != userDetails.UserVeterinarId) //שינה את הוטרינר
                    {
                        veterinarService veterinarService = new veterinarService();
                        VeterinarDetailsView vet = veterinarService.GetVet(userDetails.VeterinarName, userDetails.VeterinarPhone1);
                        if (vet == null) //אם עדין אין במאגר וטרינר כזה
                        {
                            veterinarTbl veterinarTbl = new veterinarTbl();
                            veterinarTbl.VeterinarAddress = userDetails.VeterinarAddress;
                            veterinarTbl.VeterinarCity = userDetails.VeterinarCity;
                            veterinarTbl.VeterinarEmail = userDetails.VeterinarEmail;
                            veterinarTbl.VeterinarName = userDetails.VeterinarName;
                            veterinarTbl.VeterinarPhone1 = userDetails.VeterinarPhone1;

                            context.veterinarTbl.Add(veterinarTbl);
                            context.SaveChanges();

                        }
                        user.UserVeterinarId = vet.VeterinarId;
                    }
                    
                   
                    context.Entry(user).CurrentValues.SetValues(userDetails);
                    context.SaveChanges();

                }

            }
            catch (SqlException ex)
            { throw ex; }
            finally
            {  }
        }
   
        //הוספת משתמש
        public void InsertUserDetails(UserDetailsView userDetails)

        {
            try
            {
                using (DatabaseEntitiesMT context = new DatabaseEntitiesMT())
                {
                    UsersTbl usersTbl = new UsersTbl();
                    usersTbl.UserAddress = userDetails.UserAddress;
                    usersTbl.UserCity = userDetails.UserCityName;
                    usersTbl.UserComments = userDetails.UserComments;
                    usersTbl.UserEmail = userDetails.UserEmail;
                    usersTbl.UserFirstName = userDetails.UserFirstName;
                    usersTbl.UserLastName = userDetails.UserLastName;
                    usersTbl.UserPaswrd = userDetails.UserPaswrd;
                    usersTbl.UserPhone1 = userDetails.UserPhone1;
                    usersTbl.UserPhone2 = userDetails.UserPhone2;
                    usersTbl.UserStatusCode= 21;
                    usersTbl.UserName = userDetails.UserName;
                    usersTbl.Acceptmessages = userDetails.Acceptmessages;
                    usersTbl.DaysSumForDiscount = 0;
                    //int vet = GetVetID(userDetails.VeterinarName, userDetails.VeterinarPhone1);
                    //if (vet == 0) //אם עדין אין במאגר וטרינר כזה
                    //{
                    //    veterinarTbl veterinarTbl = new veterinarTbl();
                    //    veterinarTbl.VeterinarAddress = userDetails.VeterinarAddress;
                    //    veterinarTbl.VeterinarCity = userDetails.VeterinarCity;
                    //    veterinarTbl.VeterinarEmail = userDetails.VeterinarEmail;
                    //    veterinarTbl.VeterinarName = userDetails.VeterinarName;
                    //    veterinarTbl.VeterinarPhone1 = userDetails.VeterinarPhone1;

                    //    context.veterinarTbl.Add(veterinarTbl);
                    //    context.SaveChanges();

                    //}
                    veterinarService veterinarService = new veterinarService();

                    VeterinarDetailsView vet = veterinarService.GetVet(userDetails.VeterinarName, userDetails.VeterinarPhone1);
                    if (vet != null)
                    {
                        usersTbl.UserVeterinarId = vet.VeterinarId;
                    }
                    else
                    {
                        veterinarTbl veterinarTbl = new veterinarTbl();
                        veterinarTbl.VeterinarAddress = userDetails.VeterinarAddress;
                        veterinarTbl.VeterinarCity = userDetails.VeterinarCity;
                        veterinarTbl.VeterinarEmail = userDetails.VeterinarEmail;
                        veterinarTbl.VeterinarName = userDetails.VeterinarName;
                        veterinarTbl.VeterinarPhone1 = userDetails.VeterinarPhone1;
                        
                        context.veterinarTbl.Add(veterinarTbl);
                        context.SaveChanges();
                        vet = veterinarService.GetVet(userDetails.VeterinarName, userDetails.VeterinarPhone1);
                        usersTbl.UserVeterinarId = vet.VeterinarId;

                    }
                    context.UsersTbl.Add(usersTbl);
                    context.SaveChanges();

                }

            }
            catch (SqlException ex)
            { throw ex; }
        }
        
        //הוספת כלבים למשתמש
        public void AddDogsForUser(UserDetailsView userDetails)
        {
            try
            {
                DatabaseEntitiesMT context = new DatabaseEntitiesMT();
                
               for (int i = 0; i < userDetails.UserarrayDogs.Count; i++)
                    {
                    AddOneDogForUser(userDetails.UserarrayDogs[i]);
                        //UserDogs userDogs  = new UserDogs();
                        //userDogs.DogBirthDate = userDetails.UserarrayDogs[i].DogBirthDate;
                        //userDogs.DogComments = userDetails.UserarrayDogs[i].DogComments;
                        //userDogs.DogDig  = userDetails.UserarrayDogs[i].DogDig;
                        //userDogs.DogFriendlyWith = userDetails.UserarrayDogs[i].DogFriendlyWith;
                        //userDogs.DogGender = userDetails.UserarrayDogs[i].DogGender;
                        //userDogs.DogImage = userDetails.UserarrayDogs[i].DogImage;
                        //userDogs.DogJump = userDetails.UserarrayDogs[i].DogJump;
                        //userDogs.DogName = userDetails.UserarrayDogs[i].DogName;
                        //userDogs.DogNeuter = userDetails.UserarrayDogs[i].DogNeuter;
                        //userDogs.DogRabiesVaccine = userDetails.UserarrayDogs[i].DogRabiesVaccine;
                        //userDogs.DogShvav = userDetails.UserarrayDogs[i].DogShvav;
                        //userDogs.DogStatus = 21;
                        //userDogs.DogType = userDetails.UserarrayDogs[i].DogType;
                        //userDogs.DogUserID = userDetails.UserID;
                        //context.UserDogs.Add(userDogs);
                        //context.SaveChanges();


                    }
                


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// הוספת כלב למשתמש
        /// </summary>
        /// <param name="dog"></param>
        public void AddOneDogForUser(DogDetailsView  dog)
        {
            try
            {
                DatabaseEntitiesMT context = new DatabaseEntitiesMT();
                UserDogs userDogs = new UserDogs();
                userDogs.DogBirthDate = dog.DogBirthDate;
                userDogs.DogComments = dog.DogComments;
                userDogs.DogDig = dog.DogDig;
                userDogs.DogFriendlyWith = dog.DogFriendlyWith;
                userDogs.DogGender = dog.DogGender;
                userDogs.DogImage = dog.DogImage;
                userDogs.DogJump = dog.DogJump;
                userDogs.DogName = dog.DogName;
                userDogs.DogNeuter = dog.DogNeuter;
                userDogs.DogRabiesVaccine = dog.DogRabiesVaccine;
                userDogs.DogShvav = dog.DogShvav;
                userDogs.DogStatus = 21;
                userDogs.DogType = dog.DogType;
                userDogs.DogUserID = dog.DogUserID;
                context.UserDogs.Add(userDogs);
                context.SaveChanges();


            }




           
            catch (Exception ex)
            {
                throw ex;
            }
}
        
        /// <summary>
        /// עדכון/הוספת כלבים למשתמש קיים
        /// </summary>
        /// <param name="OrdersList"></param>
            public void UpdateDogsByManager(List<DogDetailsViewManager> list)
        {
            try
            {
                using (DatabaseEntitiesMT context = new DatabaseEntitiesMT())
                {
                    foreach (DogDetailsViewManager dog in list)
                    {
                        var dogt = context.Set<UserDogs>().Find(dog.DogNumber);

                        if (dog != null)
                        {

                            context.Entry(dogt).CurrentValues.SetValues(dog);
                            context.SaveChanges();
                        }
                        else
                        {

                            throw new Exception();
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
        /// <summary>
        /// עדכון פרטי כלב
        /// </summary>
        /// <param name="dog"></param>
        public void UpdateDog(DogDetailsView dog)
        {
            try
            {
                using (DatabaseEntitiesMT context = new DatabaseEntitiesMT())
                {
                    
                        var dogt = context.Set<UserDogs>().Find(dog.DogNumber);

                        if (dog != null)
                        {

                            context.Entry(dogt).CurrentValues.SetValues(dog);
                            context.SaveChanges();
                        }
                    else
                    {

                        throw new Exception();
                    }

                    context.Dispose();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void DeleteDog(DogDetailsView dog)
        {
            try
            {
                using (DatabaseEntitiesMT context = new DatabaseEntitiesMT())
                {

                    var dogt = context.Set<UserDogs>().Find(dog.DogNumber);
                    

                    if (dog != null)
                    {
                        dog.DogStatus = 22;
                        context.Entry(dogt).CurrentValues.SetValues(dog);
                        context.SaveChanges();
                    }
                    else
                    {

                        throw new Exception();
                    }

                    context.Dispose();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}





