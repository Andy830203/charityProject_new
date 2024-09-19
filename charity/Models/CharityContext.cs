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

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<LocationImg> LocationImgs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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

        modelBuilder.Entity<LocationImg>(entity =>
        {
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
