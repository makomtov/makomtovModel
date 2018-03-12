using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebApiMTModel.Models.Models.View
{
    public class UserDetailsView
    {
        int userID;
       
        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }
       
        public string UserName { get; set; }
       
        public string UserEmail { get; set; }
       
        public string UserAddress { get; set; }

      
        public string UserStatus { get; set; }
      
        public string UserComments { get; set; }
      
        public string UserCityName { get; set; }
       
        public string UserFirstName { get; set; }
      
        public string UserLastName { get; set; }
       
        public string UserPaswrd { get; set; }
      
        public string UserPhone1 { get; set; }
        
        public string UserPhone2 { get; set; }
     
        public List<DogDetailsView> UserarrayDogs { get; set; }

        //  public ArrayList UserarrayDogs { get; set; }


    }
}
