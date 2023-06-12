﻿using Microsoft.EntityFrameworkCore;
using ModsenOnlineStore.Store.Domain.Entities;

namespace ModsenOnlineStore.Store.Infrastructure.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Log> Logs { get; set; }
    }
}
