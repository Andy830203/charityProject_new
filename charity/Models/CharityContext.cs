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

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImg> ProductImgs { get; set; }

    /*
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source = (localdb)\\ProjectModels;Initial Catalog=charity;Integrated Security=true;Encrypt=true;TrustServerCertificate=true");
    */

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("o_id_pk");

            entity.ToTable("orders");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Buyer).HasColumnName("buyer");
            entity.Property(e => e.DiscountCode).HasColumnName("discount_code");
            entity.Property(e => e.OrderTime).HasColumnName("order_time");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TotalPrice).HasColumnName("total_price");

            entity.HasOne(d => d.BuyerNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Buyer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("o_buyer_fk");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("oi_id_pk");

            entity.ToTable("order_item");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.OId).HasColumnName("o_id");
            entity.Property(e => e.PId).HasColumnName("p_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.ShippedTime).HasColumnName("shipped_time");

            entity.HasOne(d => d.OIdNavigation).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("oi_o_id_fk");

            entity.HasOne(d => d.PIdNavigation).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.PId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("oi_p_id_fk");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("p_id_pk");

            entity.ToTable("product");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.Instock).HasColumnName("instock");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.OnShelf).HasColumnName("on_shelf");
            entity.Property(e => e.OnShelfTime).HasColumnName("on_shelf_time");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Seller).HasColumnName("seller");

            entity.HasOne(d => d.SellerNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Seller)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("p_seller_fk");
        });

        modelBuilder.Entity<ProductImg>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pi_id_pk");

            entity.ToTable("product_img");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ImgName)
                .HasMaxLength(100)
                .HasColumnName("img_name");
            entity.Property(e => e.PId).HasColumnName("p_id");

            entity.HasOne(d => d.PIdNavigation).WithMany(p => p.ProductImgs)
                .HasForeignKey(d => d.PId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pi_p_id_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
