﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SWD.TicketBooking.Repo.Entities;

#nullable disable

namespace SWD.TicketBooking.Repo.Migrations
{
    [DbContext(typeof(TicketBookingDbContext))]
    [Migration("20240608181112_BaoUpdate")]
    partial class BaoUpdate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.26")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.Booking", b =>
                {
                    b.Property<int>("BookingID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookingID"), 1L, 1);

                    b.Property<DateTime>("BookingTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentMethod")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QRCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QRCodeImage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QRCodeText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("TotalBill")
                        .HasColumnType("float");

                    b.Property<int>("TripID")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("BookingID");

                    b.HasIndex("TripID");

                    b.HasIndex("UserID");

                    b.ToTable("Booking");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.City", b =>
                {
                    b.Property<int>("CityID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CityID"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CityID");

                    b.ToTable("City");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.Company", b =>
                {
                    b.Property<int>("CompanyID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CompanyID"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CompanyID");

                    b.ToTable("Company");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.Feedback", b =>
                {
                    b.Property<int>("FeedbackID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FeedbackID"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TripID")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("FeedbackID");

                    b.HasIndex("TripID");

                    b.HasIndex("UserID");

                    b.ToTable("Feedback");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.Feedback_Image", b =>
                {
                    b.Property<int>("Feedback_Image_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Feedback_Image_ID"), 1L, 1);

                    b.Property<int>("FeedbackID")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UrlGuidID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Feedback_Image_ID");

                    b.HasIndex("FeedbackID");

                    b.ToTable("Feedback_Image");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.Route", b =>
                {
                    b.Property<int>("RouteID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RouteID"), 1L, 1);

                    b.Property<string>("EndLocation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FromCityID")
                        .HasColumnType("int");

                    b.Property<string>("StartLocation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ToCityID")
                        .HasColumnType("int");

                    b.HasKey("RouteID");

                    b.HasIndex("FromCityID");

                    b.HasIndex("ToCityID");

                    b.ToTable("Route");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.Route_Company", b =>
                {
                    b.Property<int>("Route_CompanyID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Route_CompanyID"), 1L, 1);

                    b.Property<int>("CompanyID")
                        .HasColumnType("int");

                    b.Property<int>("RouteID")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Route_CompanyID");

                    b.HasIndex("CompanyID");

                    b.HasIndex("RouteID");

                    b.ToTable("Route_Company");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.Service", b =>
                {
                    b.Property<int>("ServiceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ServiceID"), 1L, 1);

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("ServiceTypeID")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UrlGuidID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ServiceID");

                    b.HasIndex("ServiceTypeID");

                    b.ToTable("Service");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.ServiceType", b =>
                {
                    b.Property<int>("ServiceTypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ServiceTypeID"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ServiceTypeID");

                    b.ToTable("ServiceType");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.Station", b =>
                {
                    b.Property<int>("StationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StationID"), 1L, 1);

                    b.Property<int>("CityID")
                        .HasColumnType("int");

                    b.Property<int>("CompanyID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StationID");

                    b.HasIndex("CityID");

                    b.HasIndex("CompanyID");

                    b.ToTable("Station");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.Station_Route", b =>
                {
                    b.Property<int>("Station_RouteID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Station_RouteID"), 1L, 1);

                    b.Property<int>("OrderInRoute")
                        .HasColumnType("int");

                    b.Property<int>("RouteID")
                        .HasColumnType("int");

                    b.Property<int>("StationID")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Station_RouteID");

                    b.HasIndex("RouteID");

                    b.HasIndex("StationID");

                    b.ToTable("Station_Route");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.Station_Service", b =>
                {
                    b.Property<int>("Station_ServiceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Station_ServiceID"), 1L, 1);

                    b.Property<int>("ServiceID")
                        .HasColumnType("int");

                    b.Property<int>("StationID")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Station_ServiceID");

                    b.HasIndex("ServiceID");

                    b.HasIndex("StationID");

                    b.ToTable("Station_Service");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.TicketDetail", b =>
                {
                    b.Property<int>("TicketDetailID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TicketDetailID"), 1L, 1);

                    b.Property<int>("BookingID")
                        .HasColumnType("int");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("SeatCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TicketType_TripID")
                        .HasColumnType("int");

                    b.HasKey("TicketDetailID");

                    b.HasIndex("BookingID");

                    b.HasIndex("TicketType_TripID");

                    b.ToTable("TicketDetail");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.TicketDetail_Service", b =>
                {
                    b.Property<int>("TicketDetail_ServiceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TicketDetail_ServiceID"), 1L, 1);

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("ServiceID")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TicketDetailID")
                        .HasColumnType("int");

                    b.HasKey("TicketDetail_ServiceID");

                    b.HasIndex("ServiceID");

                    b.HasIndex("TicketDetailID");

                    b.ToTable("TicketDetail_Service");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.TicketType", b =>
                {
                    b.Property<int>("TicketTypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TicketTypeID"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TicketTypeID");

                    b.ToTable("TicketType");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.TicketType_Trip", b =>
                {
                    b.Property<int>("TicketType_TripID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TicketType_TripID"), 1L, 1);

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TicketTypeID")
                        .HasColumnType("int");

                    b.Property<int>("TripID")
                        .HasColumnType("int");

                    b.HasKey("TicketType_TripID");

                    b.HasIndex("TicketTypeID");

                    b.HasIndex("TripID");

                    b.ToTable("TicketType_Trip");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.Trip", b =>
                {
                    b.Property<int>("TripID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TripID"), 1L, 1);

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsTemplate")
                        .HasColumnType("bit");

                    b.Property<int>("RouteID")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TripID");

                    b.HasIndex("RouteID");

                    b.ToTable("Trip");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.Trip_Utility", b =>
                {
                    b.Property<int>("Trip_UtilityID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Trip_UtilityID"), 1L, 1);

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TripID")
                        .HasColumnType("int");

                    b.Property<int>("UtilityID")
                        .HasColumnType("int");

                    b.HasKey("Trip_UtilityID");

                    b.HasIndex("TripID");

                    b.HasIndex("UtilityID");

                    b.ToTable("Trip_Utility");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.TripPicture", b =>
                {
                    b.Property<int>("TripPictureID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TripPictureID"), 1L, 1);

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TripID")
                        .HasColumnType("int");

                    b.Property<string>("UrlGuidID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TripPictureID");

                    b.HasIndex("TripID");

                    b.ToTable("TripPicture");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"), 1L, 1);

                    b.Property<string>("Address")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Balance")
                        .HasColumnType("float");

                    b.Property<DateTimeOffset?>("CreateDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FullName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool?>("IsVerified")
                        .HasColumnType("bit");

                    b.Property<string>("OTPCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<int>("RoleID")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UrlGuidID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("UserID");

                    b.HasIndex("RoleID");

                    b.ToTable("User");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.UserRole", b =>
                {
                    b.Property<int>("RoleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleID"), 1L, 1);

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleID");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.Utility", b =>
                {
                    b.Property<int>("UtilityID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UtilityID"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UtilityID");

                    b.ToTable("Utility");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.Booking", b =>
                {
                    b.HasOne("SWD.TicketBooking.Repo.Entities.Trip", "Trip")
                        .WithMany()
                        .HasForeignKey("TripID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SWD.TicketBooking.Repo.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Trip");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.Feedback", b =>
                {
                    b.HasOne("SWD.TicketBooking.Repo.Entities.Trip", "Trip")
                        .WithMany()
                        .HasForeignKey("TripID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SWD.TicketBooking.Repo.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Trip");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.Feedback_Image", b =>
                {
                    b.HasOne("SWD.TicketBooking.Repo.Entities.Feedback", "Feedback")
                        .WithMany()
                        .HasForeignKey("FeedbackID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Feedback");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.Route", b =>
                {
                    b.HasOne("SWD.TicketBooking.Repo.Entities.City", "FromCity")
                        .WithMany()
                        .HasForeignKey("FromCityID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SWD.TicketBooking.Repo.Entities.City", "ToCity")
                        .WithMany()
                        .HasForeignKey("ToCityID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("FromCity");

                    b.Navigation("ToCity");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.Route_Company", b =>
                {
                    b.HasOne("SWD.TicketBooking.Repo.Entities.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SWD.TicketBooking.Repo.Entities.Route", "Route")
                        .WithMany()
                        .HasForeignKey("RouteID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("Route");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.Service", b =>
                {
                    b.HasOne("SWD.TicketBooking.Repo.Entities.ServiceType", "ServiceType")
                        .WithMany()
                        .HasForeignKey("ServiceTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ServiceType");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.Station", b =>
                {
                    b.HasOne("SWD.TicketBooking.Repo.Entities.City", "City")
                        .WithMany()
                        .HasForeignKey("CityID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SWD.TicketBooking.Repo.Entities.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");

                    b.Navigation("Company");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.Station_Route", b =>
                {
                    b.HasOne("SWD.TicketBooking.Repo.Entities.Route", "Route")
                        .WithMany()
                        .HasForeignKey("RouteID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SWD.TicketBooking.Repo.Entities.Station", "Station")
                        .WithMany()
                        .HasForeignKey("StationID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Route");

                    b.Navigation("Station");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.Station_Service", b =>
                {
                    b.HasOne("SWD.TicketBooking.Repo.Entities.Service", "Service")
                        .WithMany()
                        .HasForeignKey("ServiceID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SWD.TicketBooking.Repo.Entities.Station", "Station")
                        .WithMany()
                        .HasForeignKey("StationID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Service");

                    b.Navigation("Station");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.TicketDetail", b =>
                {
                    b.HasOne("SWD.TicketBooking.Repo.Entities.Booking", "Booking")
                        .WithMany()
                        .HasForeignKey("BookingID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SWD.TicketBooking.Repo.Entities.TicketType_Trip", "TicketType_Trip")
                        .WithMany()
                        .HasForeignKey("TicketType_TripID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Booking");

                    b.Navigation("TicketType_Trip");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.TicketDetail_Service", b =>
                {
                    b.HasOne("SWD.TicketBooking.Repo.Entities.Service", "Service")
                        .WithMany()
                        .HasForeignKey("ServiceID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SWD.TicketBooking.Repo.Entities.TicketDetail", "TicketDetail")
                        .WithMany()
                        .HasForeignKey("TicketDetailID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Service");

                    b.Navigation("TicketDetail");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.TicketType_Trip", b =>
                {
                    b.HasOne("SWD.TicketBooking.Repo.Entities.TicketType", "TicketType")
                        .WithMany()
                        .HasForeignKey("TicketTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SWD.TicketBooking.Repo.Entities.Trip", "Trip")
                        .WithMany()
                        .HasForeignKey("TripID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("TicketType");

                    b.Navigation("Trip");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.Trip", b =>
                {
                    b.HasOne("SWD.TicketBooking.Repo.Entities.Route", "Route")
                        .WithMany()
                        .HasForeignKey("RouteID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Route");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.Trip_Utility", b =>
                {
                    b.HasOne("SWD.TicketBooking.Repo.Entities.Trip", "Trip")
                        .WithMany()
                        .HasForeignKey("TripID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SWD.TicketBooking.Repo.Entities.Utility", "Utility")
                        .WithMany()
                        .HasForeignKey("UtilityID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Trip");

                    b.Navigation("Utility");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.TripPicture", b =>
                {
                    b.HasOne("SWD.TicketBooking.Repo.Entities.Trip", "Trip")
                        .WithMany()
                        .HasForeignKey("TripID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Trip");
                });

            modelBuilder.Entity("SWD.TicketBooking.Repo.Entities.User", b =>
                {
                    b.HasOne("SWD.TicketBooking.Repo.Entities.UserRole", "UserRole")
                        .WithMany()
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserRole");
                });
#pragma warning restore 612, 618
        }
    }
}