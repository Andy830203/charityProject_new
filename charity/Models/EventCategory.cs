﻿using System;
using System.Collections.Generic;

namespace charity.Models;

public partial class EventCategory
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
