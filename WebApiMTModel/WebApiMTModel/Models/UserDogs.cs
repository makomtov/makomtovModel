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
    
    public partial class UserDogs
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserDogs()
        {
            this.DogsInOrder = new HashSet<DogsInOrder>();
            this.RoomSetting = new HashSet<RoomSetting>();
        }
    
        public int DogNumber { get; set; }
        public string DogName { get; set; }
        public string DogShvav { get; set; }
        public string DogType { get; set; }
        public int DogStatus { get; set; }
        public string DogComments { get; set; }
        public int DogUserID { get; set; }
        public string DogImage { get; set; }
        public string DogGender { get; set; }
        public bool DogNeuter { get; set; }
        public System.DateTime DogRabiesVaccine { get; set; }
        public Nullable<bool> DogJump { get; set; }
        public System.DateTime DogBirthDate { get; set; }
        public Nullable<int> DogFriendlyWith { get; set; }
        public Nullable<bool> DogDig { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DogsInOrder> DogsInOrder { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoomSetting> RoomSetting { get; set; }
        public virtual StatusTbl StatusTbl { get; set; }
        public virtual UsersTbl UsersTbl { get; set; }
    }
}
