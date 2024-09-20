using System;
using System.Collections.Generic;

namespace charity.Models;

public partial class Discount
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public decimal Rate { get; set; }
}
