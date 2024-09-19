using System;
using System.Collections.Generic;

namespace charity.Models;

public partial class StaffAccess
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
