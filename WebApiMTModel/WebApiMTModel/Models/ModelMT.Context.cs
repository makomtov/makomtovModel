﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DatabaseEntitiesMT : DbContext
    {
        public DatabaseEntitiesMT()
            : base("name=DatabaseEntitiesMT")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<DogsInOrder> DogsInOrder { get; set; }
        public virtual DbSet<OrdersTbl> OrdersTbl { get; set; }
        public virtual DbSet<OrderTypes> OrderTypes { get; set; }
        public virtual DbSet<PricesTbl> PricesTbl { get; set; }
        public virtual DbSet<RoomSetting> RoomSetting { get; set; }
        public virtual DbSet<RoomsTbl> RoomsTbl { get; set; }
        public virtual DbSet<StatusTbl> StatusTbl { get; set; }
        public virtual DbSet<UserDogs> UserDogs { get; set; }
        public virtual DbSet<UsersTbl> UsersTbl { get; set; }
        public virtual DbSet<veterinarTbl> veterinarTbl { get; set; }
        public virtual DbSet<OpenHours> OpenHours { get; set; }
    }
}
