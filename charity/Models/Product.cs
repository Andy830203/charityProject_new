using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace charity.Models;

public partial class Product
{
    public int Id { get; set; }
    [Display(Name = "商品名稱")]
    public string? Name { get; set; }
    
    public int? Seller { get; set; }
    public int? Category { get; set; }
    [Display(Name = "售價")]
    public int? Price { get; set; }
    [Display(Name = "上架狀態")]
    public bool? OnShelf { get; set; }
    [Display(Name = "上架時間")]
    public DateTime? OnShelfTime { get; set; }
    [Display(Name = "商品描述")]
    public string? Description { get; set; }
    [Display(Name = "庫存量")]
    public int? Instock { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    [Display(Name = "商品類別")]
    public virtual ProductCategory? CategoryNavigation { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<ProductImg> ProductImgs { get; set; } = new List<ProductImg>();
    [Display(Name = "賣家ID")]
    public virtual Member? SellerNavigation { get; set; }
}
