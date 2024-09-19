using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace charity.Models;

public partial class CharityContext : DbContext
{
    public CharityContext(DbContextOptions<CharityContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventCategory> EventCategories { get; set; }

    public virtual DbSet<EventImg> EventImgs { get; set; }

    public virtual DbSet<EventLocation> EventLocations { get; set; }

    public virtual DbSet<EventPeriod> EventPeriods { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("events_id_pk");

            entity.ToTable("event");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.Fee).HasColumnName("fee");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.OrganizerId).HasColumnName("organizer_id");
            entity.Property(e => e.Priority).HasColumnName("priority");

            entity.HasOne(d => d.Category).WithMany(p => p.Events)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("events_category_id_fk");

            entity.HasOne(d => d.Organizer).WithMany(p => p.Events)
                .HasForeignKey(d => d.OrganizerId)
                .HasConstraintName("events_organizer_id_fk");
        });

        modelBuilder.Entity<EventCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ec_c_id_pk");

            entity.ToTable("event_category");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<EventImg>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ei_id_pk");

            entity.ToTable("event_img");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EId).HasColumnName("e_id");
            entity.Property(e => e.ImgName)
                .HasMaxLength(100)
                .HasColumnName("img_name");

            entity.HasOne(d => d.EIdNavigation).WithMany(p => p.EventImgs)
                .HasForeignKey(d => d.EId)
                .HasConstraintName("ei_e_id_fk");
        });

        modelBuilder.Entity<EventLocation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("el_id_pk");

            entity.ToTable("event_location");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EId).HasColumnName("e_id");
            entity.Property(e => e.LId).HasColumnName("l_id");
            entity.Property(e => e.OrderInEvent).HasColumnName("order_in_event");

            entity.HasOne(d => d.EIdNavigation).WithMany(p => p.EventLocations)
                .HasForeignKey(d => d.EId)
                .HasConstraintName("el_e_id_fk");

            entity.HasOne(d => d.LIdNavigation).WithMany(p => p.EventLocations)
                .HasForeignKey(d => d.LId)
                .HasConstraintName("el_l_id_fk");
        });

        modelBuilder.Entity<EventPeriod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ep_id_pk");

            entity.ToTable("event_period");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.EId).HasColumnName("e_id");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.StartTime).HasColumnName("start_time");

            entity.HasOne(d => d.EIdNavigation).WithMany(p => p.EventPeriods)
                .HasForeignKey(d => d.EId)
                .HasConstraintName("ep_e_id_fk");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("location_id_pk");

            entity.ToTable("location");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.Latitude)
                .HasColumnType("decimal(9, 6)")
                .HasColumnName("latitude");
            entity.Property(e => e.Longitude)
                .HasColumnType("decimal(9, 6)")
                .HasColumnName("longitude");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.PlusCode)
                .HasMaxLength(100)
                .HasColumnName("plus_code");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("member_id_pk");

            entity.ToTable("member");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Access).HasColumnName("access");
            entity.Property(e => e.Account)
                .HasMaxLength(100)
                .HasColumnName("account");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.Checkin).HasColumnName("checkin");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Exp).HasColumnName("exp");
            entity.Property(e => e.FaceRec).HasColumnName("face_rec");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.ImgName)
                .HasMaxLength(100)
                .HasColumnName("img_name");
            entity.Property(e => e.NickName)
                .HasMaxLength(100)
                .HasColumnName("nickName");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(100)
                .HasColumnName("phone");
            entity.Property(e => e.Points).HasColumnName("points");
            entity.Property(e => e.RealName)
                .HasMaxLength(100)
                .HasColumnName("realName");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
