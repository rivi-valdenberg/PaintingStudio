using DL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace DL
{
    public class DataContext : DbContext, IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Image> Images { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        //    protected override void OnModelCreating(ModelBuilder modelBuilder)
        //    {
        //        base.OnModelCreating(modelBuilder);

        //        // User configuration
        //        modelBuilder.Entity<User>(entity =>
        //        {
        //            entity.HasKey(e => e.Id);
        //            entity.HasIndex(e => e.Email).IsUnique();
        //            entity.HasIndex(e => e.AccessCode).IsUnique();

        //            entity.Property(e => e.CreatedAt)
        //                .HasDefaultValueSql("GETUTCDATE()");
        //        });

        //        // Image configuration
        //        modelBuilder.Entity<Image>(entity =>
        //        {
        //            entity.HasKey(e => e.Id);

        //            entity.HasOne(e => e.User)
        //                .WithMany(u => u.Images)
        //                .HasForeignKey(e => e.UserId)
        //                .OnDelete(DeleteBehavior.Cascade);

        //            entity.Property(e => e.CreatedAt)
        //                .HasDefaultValueSql("GETUTCDATE()");

        //            entity.Property(e => e.UpdatedAt)
        //                .HasDefaultValueSql("GETUTCDATE()");
        //        });

        //        // Seed data
        //        SeedData(modelBuilder);
        //    }

        //    private static void SeedData(ModelBuilder modelBuilder)
        //    {
        //        // Seed admin user
        //        modelBuilder.Entity<User>().HasData(
        //            new User
        //            {
        //                Id = 1,
        //                Name = "מנהלת הסטודיו",
        //                Email = "admin@paintingstudio.com",
        //                PasswordHash = "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=", // "admin123"
        //                Role = "Admin",
        //                AccessCode = "ADMIN1",
        //                CreatedAt = DateTime.UtcNow,
        //                IsActive = true
        //            }
        //        );
        //    }
        //}
    }
    }