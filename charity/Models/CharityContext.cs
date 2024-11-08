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

    public virtual DbSet<Backup1> Backup1s { get; set; }

    public virtual DbSet<Backup2> Backup2s { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Collection> Collections { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<CommentImg> CommentImgs { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventCategory> EventCategories { get; set; }

    public virtual DbSet<EventImg> EventImgs { get; set; }

    public virtual DbSet<EventLocation> EventLocations { get; set; }

    public virtual DbSet<EventPeriod> EventPeriods { get; set; }

    public virtual DbSet<LevelExp> LevelExps { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<LocationImg> LocationImgs { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<MemberAccess> MemberAccesses { get; set; }

    public virtual DbSet<MemberStatus> MemberStatuses { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ProductImg> ProductImgs { get; set; }

    public virtual DbSet<SignUp> SignUps { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<StaffAccess> StaffAccesses { get; set; }

    public virtual DbSet<StaffStatus> StaffStatuses { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=charity;Integrated Security=true;Encrypt=true;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Backup1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("backup1_id_pk");

            entity.ToTable("backup1");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Col1)
                .HasMaxLength(100)
                .HasColumnName("col1");
            entity.Property(e => e.Col2)
                .HasMaxLength(100)
                .HasColumnName("col2");
            entity.Property(e => e.Col3)
                .HasMaxLength(100)
                .HasColumnName("col3");
            entity.Property(e => e.Col4)
                .HasMaxLength(100)
                .HasColumnName("col4");
            entity.Property(e => e.Col5)
                .HasMaxLength(100)
                .HasColumnName("col5");
        });

        modelBuilder.Entity<Backup2>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("backup2_id_pk");

            entity.ToTable("backup2");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Col1)
                .HasMaxLength(100)
                .HasColumnName("col1");
            entity.Property(e => e.Col2)
                .HasMaxLength(100)
                .HasColumnName("col2");
            entity.Property(e => e.Col3)
                .HasMaxLength(100)
                .HasColumnName("col3");
            entity.Property(e => e.Col4)
                .HasMaxLength(100)
                .HasColumnName("col4");
            entity.Property(e => e.Col5)
                .HasMaxLength(100)
                .HasColumnName("col5");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cart_item_id_pk");

            entity.ToTable("cart_item");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Buyer).HasColumnName("buyer");
            entity.Property(e => e.PId).HasColumnName("p_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.BuyerNavigation).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.Buyer)
                .HasConstraintName("cart_item_buyer_fk");

            entity.HasOne(d => d.PIdNavigation).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.PId)
                .HasConstraintName("cart_item_p_id_fk");
        });

        modelBuilder.Entity<Collection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("collection_id_pk");

            entity.ToTable("collections");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Attendance).HasColumnName("attendance");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("comment_id_pk");

            entity.ToTable("comment");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content)
                .HasMaxLength(500)
                .HasColumnName("content");
            entity.Property(e => e.EId).HasColumnName("e_id");
            entity.Property(e => e.MId).HasColumnName("m_id");
            entity.Property(e => e.Score).HasColumnName("score");

            entity.HasOne(d => d.EIdNavigation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.EId)
                .HasConstraintName("comment_e_id_fk");

            entity.HasOne(d => d.MIdNavigation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.MId)
                .HasConstraintName("comment_m_id_fk");
        });

        modelBuilder.Entity<CommentImg>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ci_id_pk");

            entity.ToTable("comment_img");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CId).HasColumnName("c_id");
            entity.Property(e => e.ImgName)
                .HasMaxLength(100)
                .HasColumnName("img_name");

            entity.HasOne(d => d.CIdNavigation).WithMany(p => p.CommentImgs)
                .HasForeignKey(d => d.CId)
                .HasConstraintName("ci_c_id_fk");
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("discount_id_pk");

            entity.ToTable("discount");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Rate)
                .HasColumnType("decimal(2, 2)")
                .HasColumnName("rate");
        });

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
                .OnDelete(DeleteBehavior.Cascade)
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

        modelBuilder.Entity<LevelExp>(entity =>
        {
            entity.HasKey(e => e.Level).HasName("le_level_pk");

            entity.ToTable("level_exp");

            entity.Property(e => e.Level)
                .ValueGeneratedNever()
                .HasColumnName("level");
            entity.Property(e => e.Exp).HasColumnName("exp");
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
                .HasConstraintName("o_buyer_fk");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Status)
                .HasConstraintName("o_status_fk");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("oi_id_pk");

            entity.ToTable("order_item");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.OId).HasColumnName("o_id");
            entity.Property(e => e.PId).HasColumnName("p_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.ShippedTime).HasColumnName("shipped_time");

            entity.HasOne(d => d.OIdNavigation).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("oi_o_id_fk");

            entity.HasOne(d => d.PIdNavigation).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.PId)
                .HasConstraintName("oi_p_id_fk");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("os_id_pk");

            entity.ToTable("order_status");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
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

            entity.HasOne(d => d.CategoryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Category)
                .HasConstraintName("p_category_fk");

            entity.HasOne(d => d.SellerNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Seller)
                .HasConstraintName("p_seller_fk");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pc_c_id_pk");

            entity.ToTable("product_category");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
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
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("pi_p_id_fk");
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

            entity.HasOne(d => d.Ep).WithMany(p => p.SignUps)
                .HasForeignKey(d => d.EpId)
                .HasConstraintName("su_ep_id_fk");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
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
            entity.Property(e => e.ImgName)
                .HasMaxLength(100)
                .HasColumnName("imgName");
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

        modelBuilder.Entity<StaffAccess>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sa_id_pk");

            entity.ToTable("staff_access");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<StaffStatus>(entity =>
        {
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
