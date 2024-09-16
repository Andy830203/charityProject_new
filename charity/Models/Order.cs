using System;
using System.Collections.Generic;

namespace charity.Models;

public partial class Order
{
    public int Id { get; set; }

    public int Buyer { get; set; }

    public int TotalPrice { get; set; }

    public int Status { get; set; }

    public DateTime OrderTime { get; set; }

    public int? DiscountCode { get; set; }

    public virtual Member BuyerNavigation { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
