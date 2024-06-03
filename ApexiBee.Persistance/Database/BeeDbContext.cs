using ApexiBee.Domain.Models;
using ApexiBee.Persistance.EntityConfiguration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Persistance.Database
{
    public class BeeDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public BeeDbContext() 
        { }

        public BeeDbContext(DbContextOptions<BeeDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Apiary>(entity =>
            {
                entity.HasOne(a => a.Beekeeper)
                      .WithMany(b => b.Apiaries)
                      .HasForeignKey(a => a.BeekeeperId)
                      .IsRequired();
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(a => a.ManagerId).IsRequired(false);

                entity.HasOne(a => a.Manager)
                      .WithMany()
                      .HasForeignKey(a => a.ManagerId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<HubStation>(entity =>
            {
                entity.Property(a => a.ApiaryId).IsRequired(false);
                entity.Property(a => a.SerialDataId).IsRequired(false);
            });

            modelBuilder.Entity<Hive>(entity =>
            {
                entity.Property(a => a.ApiaryId).IsRequired(false);
                entity.Property(a => a.SerialDataId).IsRequired(false);
            });

            /*modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.ToTable("UserAccounts");
            });*/

            modelBuilder.Entity<IdentityUserRole<Guid>>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasOne<ApplicationUser>()
                    .WithMany()
                    .HasForeignKey(x => x.UserId)
                    .IsRequired(false);

                entity.HasOne<IdentityRole<Guid>>()
                    .WithMany()
                    .HasForeignKey(x => x.RoleId)
                    .IsRequired(false);
            });
        }

        public DbSet<UserAccount> UserAccounts { get; set; } = null!;
        public DbSet<Apiary> Apiaries { get; set; } = null!;
        public DbSet<Hive> Hives { get; set; } = null!;
        public DbSet<HubStation> HubStations { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderStatus> OrderStatuses { get; set; } = null!;
        public DbSet<Sensor> Sensors { get; set; } = null!;
        public DbSet<SensorReading> SensorReadings { get; set; } = null!;
        public DbSet<SensorType> SensorTypes { get; set; } = null!;
        public DbSet<SerialData> SerialDatas { get; set; } = null!;
    }
}
