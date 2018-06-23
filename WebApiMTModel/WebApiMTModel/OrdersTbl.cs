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
    
    public partial class OrdersTbl
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrdersTbl()
        {
            this.DogsInOrder = new HashSet<DogsInOrder>();
            this.RoomSetting = new HashSet<RoomSetting>();
        }
    
        public int OrderNumber { get; set; }
        public int OrderUserId { get; set; }
        public int OrderStatus { get; set; }
        public string OrderconfirmationNumber { get; set; }
        public int OrderType { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<int> ShiftNumberFrom { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public Nullable<int> ShiftNumberTo { get; set; }
        public Nullable<decimal> Price { get; set; }
        public System.DateTime OrderCreateDate { get; set; }
        public string ManagerComments { get; set; }
        public Nullable<decimal> discount { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DogsInOrder> DogsInOrder { get; set; }
        public virtual OpenHours OpenHours { get; set; }
        public virtual OpenHours OpenHours1 { get; set; }
        public virtual OrderTypes OrderTypes { get; set; }
        public virtual StatusTbl StatusTbl { get; set; }
        public virtual UsersTbl UsersTbl { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoomSetting> RoomSetting { get; set; }
    }
}