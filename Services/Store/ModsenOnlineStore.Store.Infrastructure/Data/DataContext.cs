﻿using Microsoft.EntityFrameworkCore;
using ModsenOnlineStore.Store.Domain.Entities;

namespace ModsenOnlineStore.Store.Infrastructure.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        
        public DbSet<Comment> Comments { get; set; }
        
        public DbSet<Product> Products { get; set; }
        
        public DbSet<Order> Orders { get; set; }
        
        public DbSet<Coupon> Coupons { get; set; }
        
        public DbSet<OrderProduct> OrderProducts { get; set; }
        
        public DbSet<ProductType> ProductTypes { get; set; }
                
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(p => p.Price).HasPrecision(18, 4);
            modelBuilder.Entity<Product>().Property(p => p.Discount).HasPrecision(18, 4);
            modelBuilder.Entity<Order>().Property(p => p.TotalPrice).HasPrecision(18, 4);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
