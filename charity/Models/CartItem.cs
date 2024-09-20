using System;
using System.Collections.Generic;

namespace charity.Models;

public partial class CartItem
{
    public int Id { get; set; }

    public int Buyer { get; set; }

    public int PId { get; set; }

    public int Quantity { get; set; }

    public virtual Member BuyerNavigation { get; set; } = null!;

    public virtual Product PIdNavigation { get; set; } = null!;
}
