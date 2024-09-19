using System;
using System.Collections.Generic;

namespace charity.Models;

public partial class SignUp
{
    public int Id { get; set; }

    public int? EpId { get; set; }

    public int? Applicant { get; set; }

    public virtual Member? ApplicantNavigation { get; set; }
}
