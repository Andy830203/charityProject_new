﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace WebAPI_for_frondEnd.Models;

public partial class EventLocation
{
    public int Id { get; set; }

    public int? EId { get; set; }

    public int? LId { get; set; }

    public int? OrderInEvent { get; set; }

    public virtual Event EIdNavigation { get; set; }

    public virtual Location LIdNavigation { get; set; }
}