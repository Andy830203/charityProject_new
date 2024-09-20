using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace charity.Models;

public partial class Order
{
    public int Id { get; set; }
    
    public int? Buyer { get; set; }
    [Display(Name = "總價")]
    public int? TotalPrice { get; set; }

    public int? Status { get; set; }
    [Display(Name = "下單時間")]
    public DateTime? OrderTime { get; set; }
    [Display(Name = "折扣碼")]
    public int? DiscountCode { get; set; }
    [Display(Name = "買家ID")]
    public virtual Member? BuyerNavigation { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    [Display(Name = "訂單狀態")]
    public virtual OrderStatus? StatusNavigation { get; set; }
}
