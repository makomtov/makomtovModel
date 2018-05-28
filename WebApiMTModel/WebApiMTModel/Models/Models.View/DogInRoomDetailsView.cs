using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiMTModel.Models.Models.View
{
    public class DogInRoomDetailsView:DogsInOrderView
    {
       

        public int id { get; set; }  // מספר זיהוי
        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }
        public string UserPhone1 { get; set; }
     
        public string UserPhone2 { get; set; }
       
        public string DogFood { get; set; } //מזון מועדף
              
        public string VeterinarName { get; set; } //שם וטרינר
        public string VeterinarPhone1 { get; set; } //טלפון וטרינר
        public DateTime FromDateInPension { get; set; } //תאריך כניסה לפנסיון
        public DateTime ToDateInPension { get; set; } //תאריך עזיבת הפנסיון
        public DateTime FromDateInRoom { get; set; } //תאריך כניסה לחדר 
        public System.DateTime ToDateInRoom { get; set; } //תאריך עזיבת החדר
        public string Comments { get; set; } //הערות על הכלב בחדר
        public int DogorderNumber { get; set; }
        public int RoomNumberDB { get; set; } //מספר החדר בבסיס הנתונים.אם הכלב הועבר, כל עוד לא עודכן בבסיס הנתונים שדה זה לא מתעדכן
        public string ManagerComments { get; set; } //הערות מנהל הכלביה
    }
}