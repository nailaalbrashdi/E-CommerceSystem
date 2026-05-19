using E_CommerceSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_CommerceSystem
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Models.User> Users { get; set; }
        public DbSet<Models.Product> Products { get; set; }
        public DbSet<Models.Order> Orders { get; set; }

        public DbSet<Models.Review> Reviews { get; set; }

        public DbSet<Models.OrderProducts> OrderProducts { get; set; }
        public object Models { get; internal set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ECommerceDB;Trusted_Connection=True");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderProducts>()
                .HasKey(op => new
                {
                    op.OrderId,
                    op.ProductId
                });
        }


    }
}
