using System;
using System.Collections.Generic;

namespace WebAPI_for_frondEnd.Models
{
    public partial class Collection
    {
        public int Id { get; set; }

        public int? MemberId { get; set; }

        public int? eventId { get; set; }

        public bool? attendance { get; set; }
    }
}
