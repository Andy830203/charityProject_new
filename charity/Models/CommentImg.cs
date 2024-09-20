using System;
using System.Collections.Generic;

namespace charity.Models;

public partial class CommentImg
{
    public int Id { get; set; }

    public int CId { get; set; }

    public string ImgName { get; set; } = null!;

    public virtual Comment CIdNavigation { get; set; } = null!;
}
