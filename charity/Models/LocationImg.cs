using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace charity.Models;

public partial class LocationImg
{
    [Display(Name = "地點圖片id")]
    public int Id { get; set; }
    [Display(Name = "地點id")]
    public int? LId { get; set; }
    [Display(Name = "地點圖片調用名稱")]
    public string? ImgName { get; set; }
    [Display(Name = "地點圖片歸屬")]
    public virtual Location? LIdNavigation { get; set; }
}
