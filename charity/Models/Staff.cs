using System;
using System.Collections.Generic;

namespace charity.Models;

public partial class Staff
{
    public int Id { get; set; }

    public string Account { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Name { get; set; }
}
