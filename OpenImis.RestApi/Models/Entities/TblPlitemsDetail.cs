using System;
using System.Collections.Generic;

namespace OpenImis.RestApi.Models.Entities
{
    public partial class TblPlitemsDetail
    {
        public int PlitemDetailId { get; set; }
        public int PlitemId { get; set; }
        public int ItemId { get; set; }
        public decimal? PriceOverule { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int AuditUserId { get; set; }
        public byte[] RowId { get; set; }

        public TblItems Item { get; set; }
        public TblPlitems Plitem { get; set; }
    }
}
