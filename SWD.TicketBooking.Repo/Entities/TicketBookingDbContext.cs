using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Repo.Entities
{
    public class TicketBookingDbContext : DbContext
    {
        public TicketBookingDbContext(DbContextOptions<TicketBookingDbContext> options) : base(options)
        {
        }

        #region Dbset

        public DbSet<User> Users { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Route> Route { get; set; }
        public DbSet<TicketType> TicketType { get; set; }

        public DbSet<Station> Station { get; set; }
        public DbSet<Station_Company> Station_Company { get; set; }
        public DbSet<StationCompany_Route> StationCompany_Route { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<Feedback_Image> Feedback_Image { get; set; }

        public DbSet<Booking> Booking { get; set; }
        public DbSet<TicketDetail> TicketDetail { get; set; }
        public DbSet<Station_Service> Station_Service { get; set; }
        public DbSet<Transaction> Transaction { get; set; }

        public DbSet<TicketDetail_Service> TicketDetail_Service { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<ServiceType> ServiceType { get; set; }
        public DbSet<TicketType_Trip> TicketType_Trip { get; set; }

        public DbSet<Trip> Trip { get; set; }

        public DbSet<Route_Company> Route_Company { get; set; }
        //public DbSet<Service_Trip> Service_Trip { get; set; }

        public DbSet<Utility> Utility { get; set; }

        public DbSet<Trip_Utility> UtilityInTrips { get; set; }

        public DbSet<TripPicture> TripPictures { get; set; }

        #endregion Dbset

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=localhost;uid=sa;pwd=12345;Database=TicketBookingSWD;Trusted_Connection=True;TrustServerCertificate=True;");
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TicketType_Trip>()
                .HasOne(vr => vr.Trip)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<TicketDetail>()
               .HasOne(vr => vr.TicketType_Trip)
               .WithMany()
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<TicketDetail_Service>()
               .HasOne(vr => vr.Service)
               .WithMany()
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Route>()
               .HasOne(vr => vr.FromCity)
               .WithMany()
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Route>()
               .HasOne(vr => vr.ToCity)
               .WithMany()
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Station_Company>()
               .HasOne(vr => vr.Station)
               .WithMany()
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<StationCompany_Route>()
               .HasOne(vr => vr.Route)
               .WithMany()
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Station_Service>()
               .HasOne(vr => vr.Station)
               .WithMany()
               .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Route_Company>()
               .HasOne(vr => vr.Route)
               .WithMany()
               .OnDelete(DeleteBehavior.NoAction);
            //modelBuilder.Entity<Service_Trip>()
            //   .HasOne(vr => vr.Trip)
            //   .WithMany()
            //   .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Trip_Utility>()
                .HasOne(vr => vr.Trip)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<TicketDetail_Service>()
       .HasOne(td => td.TicketDetail)
       .WithMany() // Adjust as necessary
       .HasForeignKey(td => td.TicketDetailID)
       .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Feedback>()
              .HasOne(vr => vr.Trip)
              .WithMany()
              .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Trip>()
           .HasOne(t => t.User)
           .WithMany()
           .OnDelete(DeleteBehavior.Restrict);
        }
    }
}