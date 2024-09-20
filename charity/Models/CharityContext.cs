﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace charity.Models;

public partial class CharityContext : DbContext
{
    public CharityContext()
    {
    }

    public CharityContext(DbContextOptions<CharityContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }
    public virtual DbSet<LevelExp> LevelExps { get; set; }

    public virtual DbSet<EventCategory> EventCategories { get; set; }

    public virtual DbSet<EventImg> EventImgs { get; set; }
    public virtual DbSet<MemberAccess> MemberAccesses { get; set; }

    public virtual DbSet<EventLocation> EventLocations { get; set; }
    public virtual DbSet<MemberStatus> MemberStatuses { get; set; }

    public virtual DbSet<EventPeriod> EventPeriods { get; set; }
    public virtual DbSet<SignUp> SignUps { get; set; }

    public virtual DbSet<Location> Locations { get; set; }
    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Member> Members { get; set; }
    public virtual DbSet<StaffAccess> StaffAccesses { get; set; }

    public virtual DbSet<LocationImg> LocationImgs { get; set; }
    public virtual DbSet<StaffStatus> StaffStatuses { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source = (localdb)\\ProjectModels;Initial Catalog=charity;Integrated Security=true;Encrypt=true;TrustServerCertificate=true");

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
        modelBuilder.Entity<LocationImg>(entity => {
            entity.HasKey(e => e.Id).HasName("location_imgs_li_id_pk");

            entity.ToTable("location_img");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ImgName)
                .HasMaxLength(100)
                .HasColumnName("img_name");
            entity.Property(e => e.LId).HasColumnName("l_id");

            entity.HasOne(d => d.LIdNavigation).WithMany(p => p.LocationImgs)
                .HasForeignKey(d => d.LId)
                .HasConstraintName("location_l_id_fk");
        });
        modelBuilder.Entity<LevelExp>(entity => {
            entity.HasKey(e => e.Level).HasName("le_level_pk");

            entity.ToTable("level_exp");

            entity.Property(e => e.Level)
                .ValueGeneratedNever()
                .HasColumnName("level");
            entity.Property(e => e.Exp).HasColumnName("exp");
        });

        modelBuilder.Entity<Member>(entity => {
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

            entity.HasOne(d => d.AccessNavigation).WithMany(p => p.Members)
                .HasForeignKey(d => d.Access)
                .HasConstraintName("member_access_fk");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.Members)
                .HasForeignKey(d => d.Status)
                .HasConstraintName("member_status_fk");
        });

        modelBuilder.Entity<MemberAccess>(entity => {
            entity.HasKey(e => e.Id).HasName("ma_id_pk");

            entity.ToTable("member_access");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<MemberStatus>(entity => {
            entity.HasKey(e => e.Id).HasName("ms_id_pk");

            entity.ToTable("member_status");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<SignUp>(entity => {
            entity.HasKey(e => e.Id).HasName("su_id_pk");

            entity.ToTable("sign_up");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Applicant).HasColumnName("applicant");
            entity.Property(e => e.EpId).HasColumnName("ep_id");

            entity.HasOne(d => d.ApplicantNavigation).WithMany(p => p.SignUps)
                .HasForeignKey(d => d.Applicant)
                .HasConstraintName("su_applicant");
        });

        modelBuilder.Entity<Staff>(entity => {
            entity.HasKey(e => e.Id).HasName("staff_id_pk");

            entity.ToTable("staff");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Access).HasColumnName("access");
            entity.Property(e => e.Account)
                .HasMaxLength(100)
                .HasColumnName("account");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.ArrivalDate).HasColumnName("arrivalDate");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(100)
                .HasColumnName("phone");
            entity.Property(e => e.RealName)
                .HasMaxLength(100)
                .HasColumnName("realName");
            entity.Property(e => e.ResignDate).HasColumnName("resignDate");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.AccessNavigation).WithMany(p => p.Staff)
                .HasForeignKey(d => d.Access)
                .HasConstraintName("staff_access_fk");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.Staff)
                .HasForeignKey(d => d.Status)
                .HasConstraintName("staff_status_fk");
        });

        modelBuilder.Entity<StaffAccess>(entity => {
            entity.HasKey(e => e.Id).HasName("sa_id_pk");

            entity.ToTable("staff_access");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<StaffStatus>(entity => {
            entity.HasKey(e => e.Id).HasName("ss_id_pk");

            entity.ToTable("staff_status");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
