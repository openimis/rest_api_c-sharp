using System;
using System.Collections.Generic;

namespace OpenImis.DB.SqlServer
{
    public partial class TblPlservicesDetail
    {
        public int PlserviceDetailId { get; set; }
        public int PlserviceId { get; set; }
        public int ServiceId { get; set; }
        public decimal? PriceOverule { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int AuditUserId { get; set; }
        public byte[] RowId { get; set; }

        public TblPlservices Plservice { get; set; }
        public TblServices Service { get; set; }
    }
}
