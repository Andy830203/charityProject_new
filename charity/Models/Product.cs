using System;
using System.Collections.Generic;

namespace charity.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Seller { get; set; }

    public int Category { get; set; }

    public int Price { get; set; }

    public bool OnShelf { get; set; }

    public DateTime OnShelfTime { get; set; }

    public string? Description { get; set; }

    public int Instock { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<ProductImg> ProductImgs { get; set; } = new List<ProductImg>();

    public virtual Member SellerNavigation { get; set; } = null!;
}
