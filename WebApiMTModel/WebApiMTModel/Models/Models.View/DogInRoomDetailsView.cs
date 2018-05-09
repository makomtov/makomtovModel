using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiMTModel.Models.Models.View
{
    public class DogInRoomDetailsView
    {
        public int DogNumber { get; set; } //מספר כלב

        public int DogUserID { get; set; } //קוד בעלים
        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }
        public string UserPhone1 { get; set; }
     
        public string UserPhone2 { get; set; }
        public string DogName { get; set; } //שם

        public string DogComments { get; set; } //הערות
        public string DogShvav { get; set; } //שבב

        public string DogType { get; set; } //גזע
        public string DogImage { get; set; } //תמונה

        public string DogGender { get; set; } //מין

        public bool DogNeuter { get; set; } //מסורס? כן/לא

        public DateTime DogRabiesVaccine { get; set; } //תאריך חיסון כלבת

        public string DogFood { get; set; } //מזון מועדף

        public DateTime DogBirthDate { get; set; } //תאריך לידה

        public int DogFriendlyWith { get; set; } //מסתדר עם: 0- זכרים, 1-נקבות 2- שניהם

        public bool DogDig { get; set; } //חופר?

        public bool DogJump { get; set; } // קופץ?
        public bool DogTraining { get; set; } //אילוף? כן/לא
        
       public bool HomeFood { get; set; }  //אוכל בית/פנסיון?
        public string VeterinarName { get; set; } //שם וטרינר
        public string VeterinarPhone1 { get; set; } //טלפון וטרינר
        public DateTime FromDateInPension { get; set; } //תאריך כניסה לפנסיון
        public DateTime ToDateInPension { get; set; } //תאריך עזיבת הפנסיון
        public DateTime FromDateInRoom { get; set; } //תאריך כניסה לחדר 
        public System.DateTime ToDateInRoom { get; set; } //תאריך עזיבת החדר
        public string Comments { get; set; } //הערות על הכלב בחדר

    }
}