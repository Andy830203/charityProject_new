﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace WebAPI_for_frondEnd.Models;

public partial class StaffAccess
{
    public int Id { get; set; }

    public string Name { get; set; }

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}