using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace charity.Models;

public partial class Member
{
    public int Id { get; set; }

    [Display(Name = "帳號")]
    public string? Account { get; set; }

    [Display(Name = "密碼")]
    public string? Password { get; set; }

    [Display(Name = "暱稱")]
    public string? NickName { get; set; }

    [Display(Name = "姓名")]
    public string? RealName { get; set; }

    [Display(Name = "性別")]
    public bool? Gender { get; set; }

    [Display(Name = "生日")]
    public DateTime? Birthday { get; set; }

    [Display(Name = "信箱")]
    public string? Email { get; set; }

    [Display(Name = "地址")]
    public string? Address { get; set; }

    [Display(Name = "手機號碼")]
    public string? Phone { get; set; }

    [Display(Name = "點數")]
    public int? Points { get; set; }

    [Display(Name = "登入天數")]
    public int? Checkin { get; set; }

    [Display(Name = "經驗值")]
    public int? Exp { get; set; }

    [Display(Name = "相片")]
    public string? ImgName { get; set; }

    [Display(Name = "帳號狀態")]
    public int? Status { get; set; }

    [Display(Name = "權限")]
    public int? Access { get; set; }

    public bool? FaceRec { get; set; }

    public virtual MemberAccess? AccessNavigation { get; set; }

    public virtual ICollection<SignUp> SignUps { get; set; } = new List<SignUp>();

    public virtual MemberStatus? StatusNavigation { get; set; }
}
