﻿using System;
using System.Collections.Generic;

namespace charity.Models;

public partial class OrderItem
{
    public int Id { get; set; }

    public int? OId { get; set; }

    public int? PId { get; set; }

    public int? Quantity { get; set; }

    public DateTime? ShippedTime { get; set; }

    public virtual Order? OIdNavigation { get; set; }

    public virtual Product? PIdNavigation { get; set; }
}
