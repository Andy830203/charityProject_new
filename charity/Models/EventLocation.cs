using System;
using System.Collections.Generic;

namespace charity.Models;

public partial class EventLocation
{
    public int Id { get; set; }

    public int EId { get; set; }

    public int LId { get; set; }

    public int? OrderInEvent { get; set; }

    public virtual Event EIdNavigation { get; set; } = null!;

    public virtual Location LIdNavigation { get; set; } = null!;
}
