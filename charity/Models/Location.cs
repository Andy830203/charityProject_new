using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace charity.Models;

public partial class Location
{
    public int Id { get; set; }

    public string? Name { get; set; }
    [DisplayFormat(DataFormatString = "{0}", ApplyFormatInEditMode = true)]
    public decimal? Longitude { get; set; }
    [DisplayFormat(DataFormatString = "{0}", ApplyFormatInEditMode = true)]
    public decimal? Latitude { get; set; }

    public string? Description { get; set; }

    public string? Address { get; set; }

    public string? PlusCode { get; set; }

    public int? Capacity { get; set; }

    public virtual ICollection<LocationImg> LocationImgs { get; set; } = new List<LocationImg>();
}
