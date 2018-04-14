//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApiMTModel.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UsersTbl
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UsersTbl()
        {
            this.OrdersTbl = new HashSet<OrdersTbl>();
            this.UserDogs = new HashSet<UserDogs>();
        }
    
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserAddress { get; set; }
        public string UserCity { get; set; }
        public int UserStatus { get; set; }
        public string UserComments { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserPaswrd { get; set; }
        public string UserPhone1 { get; set; }
        public string UserPhone2 { get; set; }
        public int DaysSumForDiscount { get; set; }
        public bool Acceptmessages { get; set; }
        public int UserVeterinarId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrdersTbl> OrdersTbl { internal get; set; }
        public virtual StatusTbl StatusTbl { internal get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserDogs> UserDogs { internal get; set; }
        public virtual veterinarTbl veterinarTbl { get; set; }
    }
}
