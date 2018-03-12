using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

/// <summary>
/// Summary description for DogDetails
/// </summary>
namespace WebApiMTModel.Models.Models.View
{
    public class DogDetailsView
    {

        public DogDetailsView()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public int DogNumber { get; set; } //מספר כלב

        public int DogUserID { get; set; } //קוד בעלים

        public string DogName { get; set; } //שם

        public string DogComments { get; set; } //הערות

        public string DogShvav { get; set; } //שבב

        public string DogType { get; set; } //גזע

        public int DogStatus { get; set; } //סטטוס - 21 או 22

        public string DogImage { get; set; } //תמונה

        public string DogGender { get; set; } //מין

        public bool DogNeuter { get; set; } //מסורס? כן/לא

        public DateTime DogRabiesVaccine { get; set; } //תאריך חיסון כלבת

        public string DogFood { get; set; } //מזון מועדף

        public DateTime DogBirthDate { get; set; } //תאריך לידה

        public int DogFriendlyWith { get; set; } //מסתדר עם: 0- זכרים, 1-נקבות 2- שניהם

        public bool DogDig { get; set; } //חופר?

        public bool DogJump { get; set; } // קופץ?
    }










}