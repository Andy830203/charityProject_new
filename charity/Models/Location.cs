using System;
using System.Collections.Generic;

namespace charity.Models;

public partial class Location
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Longitude { get; set; }

    public decimal Latitude { get; set; }

    public string? Description { get; set; }

    public string? Address { get; set; }

    public string? PlusCode { get; set; }

    public int? Capacity { get; set; }

    public virtual ICollection<EventLocation> EventLocations { get; set; } = new List<EventLocation>();
}
