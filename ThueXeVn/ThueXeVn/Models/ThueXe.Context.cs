﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ThueXeVn.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class thuexevnEntities : DbContext
    {
        public thuexevnEntities()
            : base("name=thuexevnEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<list_car> list_car { get; set; }
        public virtual DbSet<list_car_model> list_car_model { get; set; }
        public virtual DbSet<list_car_type> list_car_type { get; set; }
        public virtual DbSet<list_online> list_online { get; set; }
        public virtual DbSet<news> news { get; set; }
        public virtual DbSet<notice> notices { get; set; }
        public virtual DbSet<notify> notifies { get; set; }
        public virtual DbSet<user> users { get; set; }
        public virtual DbSet<TinhThanh> TinhThanhs { get; set; }
        public virtual DbSet<activecode> activecodes { get; set; }
        public virtual DbSet<booking> bookings { get; set; }
        public virtual DbSet<call_driver_log> call_driver_log { get; set; }
        public virtual DbSet<car_hire_type> car_hire_type { get; set; }
        public virtual DbSet<car_size> car_size { get; set; }
        public virtual DbSet<call_log> call_log { get; set; }
        public virtual DbSet<car_made_model> car_made_model { get; set; }
        public virtual DbSet<invoice> invoices { get; set; }
        public virtual DbSet<driver> drivers { get; set; }
    }
}
