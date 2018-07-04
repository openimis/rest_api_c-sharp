using System;
using System.Collections.Generic;

namespace OpenImis.RestApi.Models.Entities
{
    public partial class TblBatchRun
    {
        public TblBatchRun()
        {
            TblClaim = new HashSet<TblClaim>();
        }

        public int RunId { get; set; }
        public int? LocationId { get; set; }
        public DateTime RunDate { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int AuditUserId { get; set; }
        public int RunYear { get; set; }
        public byte RunMonth { get; set; }

        public TblLocations Location { get; set; }
        public ICollection<TblClaim> TblClaim { get; set; }
    }
}
