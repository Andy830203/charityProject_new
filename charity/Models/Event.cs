using System;
using System.Collections.Generic;

namespace charity.Models;

public partial class Event
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int OrganizerId { get; set; }

    public int Fee { get; set; }

    public int? Capacity { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<EventImg> EventImgs { get; set; } = new List<EventImg>();

    public virtual ICollection<EventLocation> EventLocations { get; set; } = new List<EventLocation>();

    public virtual ICollection<EventPeriod> EventPeriods { get; set; } = new List<EventPeriod>();

    public virtual Member Organizer { get; set; } = null!;
}
