using System;
using System.Collections.Generic;

namespace charity.Models;

public partial class EventImg
{
    public int Id { get; set; }

    public int? EId { get; set; }

    public string? ImgName { get; set; }

    public virtual Event? EIdNavigation { get; set; }
}
