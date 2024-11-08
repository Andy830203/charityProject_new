using System;
using System.Collections.Generic;

namespace charity.Models;

public partial class Collection
{
    public int Id { get; set; }

    public int? MemberId { get; set; }

    public int? EventId { get; set; }

    public bool? Attendance { get; set; }
}
