﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenImis.ePayment.Escape.Payment.Models
{
    public class ControlNumber
    {
        public DateTime RequestedDate { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string RequestOrigin { get; set; }
        public string ResponseOrigin { get; set; }
        public string Status { get; set; }
        public int LegacyID { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime ValidityTo { get; set; }
        public int AuditedUserID { get; set; }
        public int RequestID { get; set; }
        [Required]
        public int InternalIdentifier { get; set; }
        public string Value { get; set; }
        public DateTime IssuedDate { get; set; }
    }
}