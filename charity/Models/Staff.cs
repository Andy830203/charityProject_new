using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace charity.Models;

public partial class Staff
{
    public int Id { get; set; }

    [Display(Name = "帳號")]
    public string? Account { get; set; }

    [Display(Name = "密碼")]
    public string? Password { get; set; }

    [Display(Name = "姓名")]
    public string? Name { get; set; }

    public string? RealName { get; set; }

    [Display(Name = "性別")]
    public bool? Gender { get; set; }

    [Display(Name = "生日")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)] //更改顯示方式，年月日
    public DateTime? Birthday { get; set; }

    [Display(Name = "信箱")]
    public string? Email { get; set; }

    [Display(Name = "地址")]
    public string? Address { get; set; }

    [Display(Name = "手機號碼")]
    public string? Phone { get; set; }

    [Display(Name = "到職日")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)] //更改顯示方式，年月日
    public DateTime? ArrivalDate { get; set; }

    [Display(Name = "離職日")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)] //更改顯示方式，年月日
    public DateTime? ResignDate { get; set; }

    [Display(Name = "帳號狀態")]
    public int? Status { get; set; }

    [Display(Name = "權限")]
    public int? Access { get; set; }

    public virtual StaffAccess? AccessNavigation { get; set; }

    [Display(Name = "帳號狀態")]
    public virtual StaffStatus? StatusNavigation { get; set; }
}
