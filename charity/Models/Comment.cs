using System;
using System.Collections.Generic;

namespace charity.Models;

public partial class Comment
{
    public int Id { get; set; }

    public int? MId { get; set; }

    public int? EId { get; set; }

    public string? Content { get; set; }

    public virtual ICollection<CommentImg> CommentImgs { get; set; } = new List<CommentImg>();

    public virtual Event? EIdNavigation { get; set; }

    public virtual Member? MIdNavigation { get; set; }
}
