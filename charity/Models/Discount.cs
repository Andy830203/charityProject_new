using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace charity.Models;

public partial class Discount
{
    public int Id { get; set; }

    [Display(Name = "商品名稱")]
    public string? Name { get; set; }

    [Display(Name = "折扣倍率")]
    public decimal? Rate { get; set; }
}
