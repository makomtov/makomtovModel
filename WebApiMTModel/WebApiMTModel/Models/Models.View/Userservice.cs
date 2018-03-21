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
        





        //שליפת משתמשים+כלבים


        //עדכון משתמש
        public void UpdateUserDetails(UserDetailsView userDetails)

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
            objParam.Value = userDetails.UserID;
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
            objParam.Value = userDetails.UserEmail;


            objParam = objCmd.Parameters.Add("@address", SqlDbType.Char);
            objParam.Direction = ParameterDirection.Input;
            objParam.Value = userDetails.UserAddress;
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
                    usersTbl.UserStatus = 21;
                    
                    usersTbl.UserName = userDetails.UserName;

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
                       
                        UserDogs userDogs  = new UserDogs();
                        userDogs.DogBirthDate = userDetails.UserarrayDogs[i].DogBirthDate;
                        userDogs.DogComments = userDetails.UserarrayDogs[i].DogComments;
                        userDogs.DogDig  = userDetails.UserarrayDogs[i].DogDig;
                        userDogs.DogFriendlyWith = userDetails.UserarrayDogs[i].DogFriendlyWith;
                        userDogs.DogGender = userDetails.UserarrayDogs[i].DogGender;
                        userDogs.DogImage = userDetails.UserarrayDogs[i].DogImage;
                        userDogs.DogJump = userDetails.UserarrayDogs[i].DogJump;
                        userDogs.DogName = userDetails.UserarrayDogs[i].DogName;
                        userDogs.DogNeuter = userDetails.UserarrayDogs[i].DogNeuter;
                        userDogs.DogRabiesVaccine = userDetails.UserarrayDogs[i].DogRabiesVaccine;
                        userDogs.DogShvav = userDetails.UserarrayDogs[i].DogShvav;
                        userDogs.DogStatus = 21;
                        userDogs.DogType = userDetails.UserarrayDogs[i].DogType;
                        userDogs.DogUserID = userDetails.UserID;
                        context.UserDogs.Add(userDogs);
                        context.SaveChanges();


                    }
                


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}





