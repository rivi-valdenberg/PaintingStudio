using Microsoft.EntityFrameworkCore;
using PaintingStudio.API.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace PaintingStudio.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<Instruction> Instructions { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Item
            modelBuilder.Entity<Item>()
                .HasMany(i => i.Instructions)
                .WithOne(i => i.Item)
                .HasForeignKey(i => i.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed admin user
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    // This is a hashed password for "admin123" - in production, use a proper password hashing mechanism
                    PasswordHash = "AQAAAAEAACcQAAAAEKGIieH/kHFBh84ab+iALYDFvz3Sq4XzlB/ohXB/Q3KKKKzXpUXUXXXXXXXXXXXXXX==",
                    IsAdmin = true
                }
            );
        }
    }
}