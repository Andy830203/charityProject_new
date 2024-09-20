using System;
using System.Collections.Generic;

namespace charity.Models;

public partial class EventPeriod
{
    public int Id { get; set; }

    public int? EId { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public string? Description { get; set; }

    public virtual Event? EIdNavigation { get; set; }

    public virtual ICollection<SignUp> SignUps { get; set; } = new List<SignUp>();
}
