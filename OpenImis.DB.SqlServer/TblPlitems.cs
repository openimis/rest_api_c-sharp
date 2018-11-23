using System;
using System.Collections.Generic;

namespace OpenImis.DB.SqlServer
{
    public partial class TblPlitems
    {
        public TblPlitems()
        {
            TblHf = new HashSet<TblHf>();
            TblPlitemsDetail = new HashSet<TblPlitemsDetail>();
        }

        public int PlitemId { get; set; }
        public string PlitemName { get; set; }
        public DateTime DatePl { get; set; }
        public int? LocationId { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int AuditUserId { get; set; }
        public byte[] RowId { get; set; }

        public TblLocations Location { get; set; }
        public ICollection<TblHf> TblHf { get; set; }
        public ICollection<TblPlitemsDetail> TblPlitemsDetail { get; set; }
    }
}
