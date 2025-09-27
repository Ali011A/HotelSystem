using Hotel.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Infrastructure.Persistence
{
    public class ApplicationDbContext :DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<HotelStaff> HotelStaffs { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<RoomFacility> RoomFacilities { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<OfferRoom> OfferRooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
                optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .LogTo(log => Debug.WriteLine(log)); 
        }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(b =>
            {
                b.HasKey(u => u.Id); // Id من BaseModel (Guid)
                b.HasIndex(u => u.Username).IsUnique();
                b.HasIndex(u => u.Email).IsUnique();

                b.Property(u => u.Username).HasMaxLength(180).IsRequired();
                b.Property(u => u.PasswordHash).HasMaxLength(256).IsRequired();
                b.Property(u => u.Email).HasMaxLength(256).IsRequired();
               
            });

            // -----------------------
            // Customer (PK = UserId)  1:1 with User
            // -----------------------
            modelBuilder.Entity<Customer>(b =>
            {
                b.HasKey(c => c.UserId); // use UserId as PK
                b.Property(c => c.FirstName).HasMaxLength(200);
                b.Property(c => c.LastName).HasMaxLength(200);
                b.Property(c => c.Phone).HasMaxLength(50);

                b.HasOne(c => c.User)
                 .WithOne(u => u.Customer)
                 .HasForeignKey<Customer>(c => c.UserId)
                 .OnDelete(DeleteBehavior.NoAction);
            });

            // -----------------------
            // HotelStaff (PK = UserId)  1:1 with User
            // -----------------------
            modelBuilder.Entity<HotelStaff>(b =>
            {
                b.HasKey(s => s.UserId);
                b.Property(s => s.Position).HasMaxLength(200);
                b.Property(s => s.AccessRights).HasMaxLength(1000);

                b.HasOne(s => s.User)
                 .WithOne(u => u.HotelStaff)
                 .HasForeignKey<HotelStaff>(s => s.UserId)
                 .OnDelete(DeleteBehavior.NoAction );

               
            });

            // -----------------------
            // Room
            // -----------------------
            modelBuilder.Entity<Room>(b =>
            {
                b.HasKey(r => r.Id); // Id from BaseModel
                b.Property(r => r.Description).HasMaxLength(1000);
                b.Property(r => r.RoomNumber).IsRequired();
                b.Property(r => r.Price).HasColumnType("decimal(18,2)");
                b.Property(r => r.Capacity).IsRequired();
                b.Property(r => r.BedCount);

                
                b.Property(r => r.RoomType).HasConversion<string>().HasMaxLength(50);
                b.Property(r => r.AvailabilityStatus).HasConversion<string>().HasMaxLength(50);

               
                b.HasOne(r => r.HotelStaff)
                 .WithMany(s => s.ManagedRooms)
                 .HasForeignKey(r => r.StaffId)
                 .OnDelete(DeleteBehavior.NoAction); 
            });

            // -----------------------
            // RoomImage (dependent -> Room)
            // -----------------------
            modelBuilder.Entity<RoomImage>(b =>
            {
                b.HasKey(ri => ri.Id);
                b.Property(ri => ri.Url).IsRequired().HasMaxLength(1000);
                b.Property(ri => ri.IsPrimary).IsRequired();

                b.HasOne(ri => ri.Room)
                 .WithMany(r => r.RoomImages)
                 .HasForeignKey(ri => ri.RoomId)
                 .OnDelete(DeleteBehavior.NoAction);
            });

            // -----------------------
            // Facility & RoomFacility
            // -----------------------
            modelBuilder.Entity<Facility>(b =>
            {
                b.HasKey(f => f.Id);
                b.Property(f => f.FacilityName).IsRequired().HasMaxLength(200);

                b.HasMany(f => f.RoomFacilities)
                 .WithOne(rf => rf.Facility)
                 .HasForeignKey(rf => rf.FacilityId)
                 .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<RoomFacility>(b =>
            {
                b.HasKey(rf => new { rf.RoomId, rf.FacilityId }); // composite PK
                b.HasOne(rf => rf.Room).WithMany(r => r.RoomFacilities).HasForeignKey(rf => rf.RoomId);
                b.HasOne(rf => rf.Facility).WithMany(f => f.RoomFacilities).HasForeignKey(rf => rf.FacilityId);

                
            });

            // -----------------------
            // Offer & OfferRoom
            // -----------------------
            modelBuilder.Entity<Offer>(b =>
            {
                b.HasKey(o => o.Id);
                b.Property(o => o.OfferName).IsRequired().HasMaxLength(255);
                b.Property(o => o.Discount).HasColumnType("decimal(5,2)");
                b.Property(o => o.IsActive).IsRequired();

                b.HasMany(o => o.OfferRooms)
                 .WithOne(or => or.Offer)
                 .HasForeignKey(or => or.OfferId)
                 .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<OfferRoom>(b =>
            {
                b.HasKey(or => new { or.OfferId, or.RoomId });
                b.HasOne(or => or.Offer).WithMany(o => o.OfferRooms).HasForeignKey(or => or.OfferId);
                b.HasOne(or => or.Room).WithMany(r => r.OfferRooms).HasForeignKey(or => or.RoomId);
            });

            // -----------------------
            // Reservation
            // -----------------------
            modelBuilder.Entity<Reservation>(b =>
            {
                b.HasKey(r => r.Id);
                b.Property(r => r.CheckinDate).IsRequired();
                b.Property(r => r.CheckoutDate).IsRequired();
                b.Property(r => r.BookingDate).IsRequired();
                b.Property(r => r.TotalAmount).HasColumnType("decimal(18,2)");
                b.Property(r => r.Currency).HasMaxLength(10);
                b.Property(r => r.SpecialRequests).HasMaxLength(500);

                // Reservation -> Room (many reservations for one room)
                b.HasOne(r => r.room)
                 .WithMany(room => room.Reservations)
                 .HasForeignKey(r => r.RoomId)
                 .OnDelete(DeleteBehavior.NoAction);

                // Reservation -> Customer (Customer.UserId is PK)
                b.HasOne(r => r.Customer)
                 .WithMany(c => c.Reservations)
                 .HasForeignKey(r => r.CustomerId)
                 .OnDelete(DeleteBehavior.NoAction);

                // Enum mapping for Status
                b.Property(r => r.Status).HasConversion<string>().HasMaxLength(50);
            });

            // -----------------------
            // Feedback  (1:1 with Reservation)
            // -----------------------
            modelBuilder.Entity<Feedback>(b =>
            {
                b.HasKey(f => f.Id);
                b.Property(f => f.Comment).HasColumnType("text").IsRequired();
                b.Property(f => f.Rating).IsRequired();
                b.Property(f => f.ResponseStaff).HasMaxLength(100);

                // feedback -> reservation (1:1)
                b.HasOne(f => f.Reservation)
                 .WithOne(r => r.Feedback)
                 .HasForeignKey<Feedback>(f => f.ReservationId)
                 .OnDelete(DeleteBehavior.NoAction);

            });

            // -----------------------
            // Payment (1:1 with Reservation)
            // -----------------------
            modelBuilder.Entity<Payment>(b =>
            {
                b.HasKey(p => p.Id);
                b.Property(p => p.Amount).HasColumnType("decimal(18,2)");
                b.Property(p => p.PaymentGateway).HasMaxLength(100);
                b.Property(p => p.TransactionId).HasMaxLength(50);
                b.Property(p => p.Currency).HasMaxLength(10);
                b.Property(p => p.RecieveUrl).HasMaxLength(1000); // note spelling from model

                b.HasOne(p => p.Reservation)
                 .WithOne(r => r.Payments)
                 .HasForeignKey<Payment>(p => p.ReservationId)
                 .OnDelete(DeleteBehavior.NoAction);

                // map enums to strings
                b.Property(p => p.PaymentStatus).HasConversion<string>().HasMaxLength(50);
                b.Property(p => p.PaymentMethod).HasConversion<string>().HasMaxLength(50);
            });

            // -----------------------
            // Indices & other constraints
            // -----------------------
            modelBuilder.Entity<Room>().HasIndex(r => r.RoomNumber).IsUnique(false); // make unique if desired
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            // -----------------------
            // Global soft-delete query filter (optional; requires BaseModel.IsDeleted exists)
            // -----------------------
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                     .Where(t => typeof(BaseModel).IsAssignableFrom(t.ClrType)))
            {
                var method = typeof(ModelBuilderExtensions)
                    .GetMethod(nameof(ModelBuilderExtensions.AddIsDeletedQueryFilter), 
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                    ?.MakeGenericMethod(entityType.ClrType);

                method?.Invoke(null, new object[] { modelBuilder });
            }
        }
        

        // Helper extension (put in a static class)
        public static class ModelBuilderExtensions
        {
            public static void AddIsDeletedQueryFilter<TEntity>(ModelBuilder builder) where TEntity : BaseModel
            {
                builder.Entity<TEntity>().HasQueryFilter(e => !EF.Property<bool>(e, nameof(BaseModel.IsDeleted)));
            }
        }



    }
}
