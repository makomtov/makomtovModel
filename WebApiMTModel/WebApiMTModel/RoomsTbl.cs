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
    
    public partial class RoomsTbl
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RoomsTbl()
        {
            this.RoomSetting = new HashSet<RoomSetting>();
        }
    
        public int RoomID { get; set; }
        public string RoomDescription { get; set; }
        public int RoomStatus { get; set; }
        public Nullable<int> RoomCapacity { get; set; }
        public string RoomComments { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoomSetting> RoomSetting { get; set; }
        public virtual StatusTbl StatusTbl { get; set; }
    }
}
