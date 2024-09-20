using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace charity.Models;

public partial class Location
{
    [Display(Name = "地點id")]
    public int Id { get; set; }
    [Display(Name = "地點名稱")]
    public string? Name { get; set; }
    [Display(Name = "地點經度")]
    public decimal? Longitude { get; set; }
    [Display(Name = "地點緯度")]
    public decimal? Latitude { get; set; }
    [Display(Name = "地點描述")]
    public string? Description { get; set; }
    [Display(Name = "地點地址")]
    public string? Address { get; set; }
    [Display(Name = "地點經緯度簡寫")]
    public string? PlusCode { get; set; }
    [Display(Name = "地點人數上限")]
    public int? Capacity { get; set; }

    public virtual ICollection<EventLocation> EventLocations { get; set; } = new List<EventLocation>();

    public virtual ICollection<LocationImg> LocationImgs { get; set; } = new List<LocationImg>();
}
