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
    
    public partial class DogsInOrder
    {
        public int OrderNumber { get; set; }
        public int DogNumber { get; set; }
        public System.DateTime FromDate { get; set; }
        public System.DateTime ToDate { get; set; }
        public int Status { get; set; }
        public Nullable<bool> DogTraining { get; set; }
        public bool HomeFood { get; set; }
        public int ShiftNumberFrom { get; set; }
        public int ShiftNumberTo { get; set; }
    
        public virtual OrdersTbl OrdersTbl { get; set; }
        public virtual StatusTbl StatusTbl { internal get; set; }
        public virtual UserDogs UserDogs { get; set; }
    }
}
