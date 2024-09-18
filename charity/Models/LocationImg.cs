using System;
using System.Collections.Generic;

namespace charity.Models;

public partial class LocationImg
{
    public int Id { get; set; }

    public int LId { get; set; }

    public string ImgName { get; set; } = null!;

    public virtual Location LIdNavigation { get; set; } = null!;
}
