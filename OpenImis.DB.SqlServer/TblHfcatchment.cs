using System;
using System.Collections.Generic;

namespace OpenImis.DB.SqlServer
{
    public partial class TblHfcatchment
    {
        public int HfcatchmentId { get; set; }
        public int Hfid { get; set; }
        public int LocationId { get; set; }
        public int? Catchment { get; set; }
        public DateTime? ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int? AuditUserId { get; set; }

        public TblHf Hf { get; set; }
        public TblLocations Location { get; set; }
    }
}
