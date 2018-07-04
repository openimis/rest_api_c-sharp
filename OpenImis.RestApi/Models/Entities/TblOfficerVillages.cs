using System;
using System.Collections.Generic;

namespace OpenImis.RestApi.Models.Entities
{
    public partial class TblOfficerVillages
    {
        public int OfficerVillageId { get; set; }
        public int? OfficerId { get; set; }
        public int? LocationId { get; set; }
        public DateTime? ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int? AuditUserId { get; set; }
        public byte[] RowId { get; set; }

        public TblLocations Location { get; set; }
        public TblOfficer Officer { get; set; }
    }
}
