﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImisRestApi.Models
{
    public class MatchSms
    {
        public int PaymentId { get; set; }
        public DateTime? DateLastSms { get; set; }
    }
}
