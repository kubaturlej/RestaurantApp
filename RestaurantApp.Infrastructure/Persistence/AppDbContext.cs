using Microsoft.EntityFrameworkCore;
using RestaurantApp.Domain.Common;
using RestaurantApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        break;
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>(eb =>
            {
                eb.HasOne(r => r.Address)
                .WithOne(a => a.Restaurant)
                .HasForeignKey<Address>(a => a.RestaurantId);

                eb.HasMany(r => r.Dishes)
                .WithOne(d => d.Restaurant)
                .HasForeignKey(x => x.RestaurantId);

                eb.HasOne(r => r.CreatedBy)
                 .WithMany()
                 .HasForeignKey(x => x.CreatedById);

            });

            modelBuilder.Entity<User>(eb =>
            {
                eb.HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleId);
            });


            modelBuilder.Entity<Role>()
                .HasData(
                    new Role { Id = 1, Name = "User" },
                    new Role { Id = 2, Name = "Manager" },
                    new Role { Id = 3, Name = "Admin" }
                );

            //modelBuilder.Entity<User>(eb =>
            //{
            //    eb
            //    .HasMany(u => u.Role)
            //    .WithMany(u => u.Users)
            //    .UsingEntity<UserRole>(
            //        ur => ur.HasOne(ur => ur.Role)
            //            .WithMany()
            //            .HasForeignKey(ur => ur.RoleId),
            //        ur => ur.HasOne(ur => ur.User)
            //            .WithMany()
            //            .HasForeignKey(ur => ur.UserId),
            //        ur =>
            //        {
            //            ur.HasKey(x => new { x.UserId, x.RoleId });
            //            ur.Property(x => x.CreationDate).HasDefaultValueSql("getutcdate()");
            //        }
            //     );
            //});
             
        }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        //public DbSet<UserRole> UserRole { get; set; }
    }
}
