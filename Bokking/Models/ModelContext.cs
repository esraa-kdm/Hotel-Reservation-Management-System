using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Bokking.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bank> Banks { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Hotel> Hotels { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<ReservationService> ReservationServices { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Testimonial> Testimonials { get; set; }

    public virtual DbSet<UserLogin> UserLogins { get; set; }

    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("USER ID=C##ESRAA;PASSWORD=Test321;DATA SOURCE=DESKTOP-01DSDTD/xe");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("C##ESRAA")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Bank>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008508");

            entity.ToTable("BANK");

            entity.HasIndex(e => e.Cardid, "SYS_C008509").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.Balance)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("BALANCE");
            entity.Property(e => e.Cardid)
                .IsRequired()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CARDID");
            entity.Property(e => e.Cardname)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("CARDNAME");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008476");

            entity.ToTable("CUSTOMER");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.Fname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FNAME");
            entity.Property(e => e.ImagePath)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("IMAGE_PATH");
            entity.Property(e => e.Lname)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("LNAME");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008474");

            entity.ToTable("FEEDBACK");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.Guestemail)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("GUESTEMAIL");
            entity.Property(e => e.Guestmessage)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("GUESTMESSAGE");
            entity.Property(e => e.Guestname)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("GUESTNAME");
            entity.Property(e => e.Isapproved)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("ISAPPROVED");
        });

        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008506");

            entity.ToTable("HOTELS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.City)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("CITY");
            entity.Property(e => e.Country)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("COUNTRY");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Hotelname)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("HOTELNAME");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Phone)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("PHONE");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008497");

            entity.ToTable("RESERVATION");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.CheckIn)
                .HasColumnType("DATE")
                .HasColumnName("Check-in");
            entity.Property(e => e.CheckOut)
                .HasColumnType("DATE")
                .HasColumnName("Check-out");
            entity.Property(e => e.Hotelid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("HOTELID");
            entity.Property(e => e.PayementStatus)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("PAYEMENT_STATUS");
            entity.Property(e => e.Roomid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ROOMID");
            entity.Property(e => e.Serviceid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("SERVICEID");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("TOTAL_PRICE");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Service).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.Serviceid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("SERVV_ID");

            entity.HasOne(d => d.User).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("USERR_ID");
        });

        modelBuilder.Entity<ReservationService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008511");

            entity.ToTable("RESERVATION_SERVICE");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.Cardid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CARDID");
            entity.Property(e => e.InvoiceSent)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("INVOICE_SENT");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("PAYMENT_METHOD");
            entity.Property(e => e.Reservationid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("RESERVATIONID");

            entity.HasOne(d => d.Card).WithMany(p => p.ReservationServices)
                .HasPrincipalKey(p => p.Cardid)
                .HasForeignKey(d => d.Cardid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("CARD_ID");

            entity.HasOne(d => d.Reservation).WithMany(p => p.ReservationServices)
                .HasForeignKey(d => d.Reservationid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("RESERVATION_ID");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008470");

            entity.ToTable("ROLES");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.Rolename)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("ROLENAME");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008503");

            entity.ToTable("ROOMS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.Availability)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.HotelId)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("HOTEL_ID");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Pricepernight)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("PRICEPERNIGHT");
            entity.Property(e => e.Rommnumber)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ROMMNUMBER");
            entity.Property(e => e.Roomtype)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("ROOMTYPE");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008494");

            entity.ToTable("SERVICE");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.HotelId)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("HOTEL_ID");
            entity.Property(e => e.Nameservice)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("NAMESERVICE");
            entity.Property(e => e.Price)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("PRICE");
        });

        modelBuilder.Entity<Testimonial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008488");

            entity.ToTable("TESTIMONIALS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.Hotelid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("HOTELID");
            entity.Property(e => e.Isapproved)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("ISAPPROVED");
            entity.Property(e => e.Message)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("MESSAGE");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USERID");

            entity.HasOne(d => d.User).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("USE_ID");
        });

        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008483");

            entity.ToTable("USER_LOGIN");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.Customerid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CUSTOMERID");
            entity.Property(e => e.Email)
                .HasMaxLength(220)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Fname)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("FNAME");
            entity.Property(e => e.Lname)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("LNAME");
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ROLE");
            entity.Property(e => e.Username)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("USERNAME");

            entity.HasOne(d => d.Customer).WithMany(p => p.UserLogins)
                .HasForeignKey(d => d.Customerid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("CUS_ID");
        });
        modelBuilder.HasSequence("SQ1");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
