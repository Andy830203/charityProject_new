using System;
using System.Collections.Generic;

namespace charity.Models;

public partial class Staff
{
    public int Id { get; set; }

    public string? Account { get; set; }

    public string? Password { get; set; }

    public string? Name { get; set; }

    public string? RealName { get; set; }

    public bool? Gender { get; set; }

    public DateTime? Birthday { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public DateTime? ArrivalDate { get; set; }

    public DateTime? ResignDate { get; set; }

    public int? Status { get; set; }

    public int? Access { get; set; }

    public virtual StaffAccess? AccessNavigation { get; set; }

    public virtual StaffStatus? StatusNavigation { get; set; }
}
