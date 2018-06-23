//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApiMTModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class RoomSetting
    {
        public int Id { get; set; }
        public int RoomNumber { get; set; }
        public Nullable<int> OrderNumber { get; set; }
        public int DogNumber { get; set; }
        public System.DateTime RoomFromDate { get; set; }
        public System.DateTime RoomToDate { get; set; }
        public string Comments { get; set; }
        public Nullable<int> RoomShiftFrom { get; set; }
        public Nullable<int> RoomShiftTo { get; set; }
    
        public virtual OpenHours OpenHours { get; set; }
        public virtual OpenHours OpenHours1 { get; set; }
        public virtual OrdersTbl OrdersTbl { get; set; }
        public virtual RoomsTbl RoomsTbl { get; set; }
        public virtual UserDogs UserDogs { get; set; }
    }
}