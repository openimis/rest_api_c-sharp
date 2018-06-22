using System;
using System.Collections.Generic;

namespace ImisRestApi.Models.Entities
{
    public partial class TblPolicyRenewalDetails
    {
        public int RenewalDetailId { get; set; }
        public int RenewalId { get; set; }
        public int InsureeId { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int AuditCreateUser { get; set; }

        public TblInsuree Insuree { get; set; }
        public TblPolicyRenewals Renewal { get; set; }
    }
}
