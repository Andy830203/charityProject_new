using System;
using System.Collections.Generic;

namespace charity.Models;

public partial class Member
{
    public int Id { get; set; }

    public string? Account { get; set; }

    public string? Password { get; set; }

    public string? NickName { get; set; }

    public string? RealName { get; set; }

    public bool? Gender { get; set; }

    public DateTime? Birthday { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public int? Points { get; set; }

    public int? Checkin { get; set; }

    public int? Exp { get; set; }

    public string? ImgName { get; set; }

    public int? Status { get; set; }

    public int? Access { get; set; }

    public bool? FaceRec { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
