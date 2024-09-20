using System;
using System.Collections.Generic;

namespace charity.Models;

public partial class MemberAccess
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}
