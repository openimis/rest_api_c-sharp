using System;
using System.Collections.Generic;

namespace OpenImis.RestApi.Models.Entities
{
    public partial class TblRelDistr
    {
        public int DistrId { get; set; }
        public byte DistrType { get; set; }
        public string DistrCareType { get; set; }
        public int ProdId { get; set; }
        public byte Period { get; set; }
        public decimal? DistrPerc { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int AuditUserId { get; set; }
        public byte[] RowId { get; set; }

        public TblProduct Prod { get; set; }
    }
}
