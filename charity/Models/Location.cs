using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace charity.Models;

public partial class Location
{
    [Display(Name = "編號")]
    public int Id { get; set; }
    [Display(Name = "地名")]
    public string Name { get; set; } = null!;
    [Display(Name = "經度")]
    public decimal Longitude { get; set; }
    [Display(Name = "緯度")]
    public decimal Latitude { get; set; }
    [Display(Name = "描述")]
    public string? Description { get; set; }
    [Display(Name = "地址")]
    public string? Address { get; set; }
    [Display(Name = "經緯度簡碼")]
    public string? PlusCode { get; set; }
    [Display(Name = "最大人數")]
    public int? Capacity { get; set; }

    public virtual ICollection<LocationImg> LocationImgs { get; set; } = new List<LocationImg>();
}
