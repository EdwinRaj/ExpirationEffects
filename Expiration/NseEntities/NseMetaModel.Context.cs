﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NseEntities
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class NseContext : DbContext
    {
        public NseContext()
            : base("name=NseContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<DerivativeType> DerivativeTypes { get; set; }
        public virtual DbSet<Symbol> Symbols { get; set; }
        public virtual DbSet<ExpirationDetail> ExpirationDetails { get; set; }
    }
}