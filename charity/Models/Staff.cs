using System;
using System.Collections.Generic;

namespace charity.Models;

public partial class Staff
{
    public int Id { get; set; }

    public string? Account { get; set; }

    public string? Password { get; set; }

    public string? Name { get; set; }
}
