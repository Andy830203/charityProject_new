using System;
using System.Collections.Generic;

namespace charity.Models;

public partial class OrderStatus
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
