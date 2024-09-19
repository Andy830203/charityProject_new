using System;
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

    public virtual DbSet<LevelExp> LevelExps { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<MemberAccess> MemberAccesses { get; set; }

    public virtual DbSet<MemberStatus> MemberStatuses { get; set; }

    public virtual DbSet<SignUp> SignUps { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source = (localdb)\\ProjectModels;Initial Catalog=charity;Integrated Security=true;Encrypt=true;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LevelExp>(entity =>
        {
            entity.HasKey(e => e.Level).HasName("le_level_pk");

            entity.ToTable("level_exp");

            entity.Property(e => e.Level)
                .ValueGeneratedNever()
                .HasColumnName("level");
            entity.Property(e => e.Exp).HasColumnName("exp");
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

            entity.HasOne(d => d.AccessNavigation).WithMany(p => p.Members)
                .HasForeignKey(d => d.Access)
                .HasConstraintName("member_access_fk");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.Members)
                .HasForeignKey(d => d.Status)
                .HasConstraintName("member_status_fk");
        });

        modelBuilder.Entity<MemberAccess>(entity =>
        {
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

        modelBuilder.Entity<MemberStatus>(entity =>
        {
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

        modelBuilder.Entity<SignUp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("su_id_pk");

            entity.ToTable("sign_up");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Applicant).HasColumnName("applicant");
            entity.Property(e => e.EpId).HasColumnName("ep_id");

            entity.HasOne(d => d.ApplicantNavigation).WithMany(p => p.SignUps)
                .HasForeignKey(d => d.Applicant)
                .HasConstraintName("su_applicant");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("staff_id_pk");

            entity.ToTable("staff");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Account)
                .HasMaxLength(100)
                .HasColumnName("account");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
