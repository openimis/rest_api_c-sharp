using System;
using System.Collections.Generic;

namespace ImisRestApi.Models.Entities
{
    public partial class TblInsureePolicy
    {
        public int InsureePolicyId { get; set; }
        public int? InsureeId { get; set; }
        public int? PolicyId { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int? AuditUserId { get; set; }
        public bool? IsOffline { get; set; }
        public byte[] RowId { get; set; }

        public TblInsuree Insuree { get; set; }
        public TblPolicy Policy { get; set; }
    }
}
