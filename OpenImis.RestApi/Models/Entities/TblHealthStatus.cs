using System;
using System.Collections.Generic;

namespace OpenImis.RestApi.Models.Entities
{
    public partial class TblHealthStatus
    {
        public int HealthStatusId { get; set; }
        public int InsureeId { get; set; }
        public string Description { get; set; }
        public DateTime? ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? AuditUserId { get; set; }
        public int? LegacyId { get; set; }

        public TblInsuree Insuree { get; set; }
    }
}
