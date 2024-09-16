using System;
using System.Collections.Generic;

namespace charity.Models;

public partial class ProductImg
{
    public int Id { get; set; }

    public int PId { get; set; }

    public string ImgName { get; set; } = null!;

    public virtual Product PIdNavigation { get; set; } = null!;
}
