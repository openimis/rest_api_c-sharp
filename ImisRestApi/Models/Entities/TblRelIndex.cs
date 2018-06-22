using System;
using System.Collections.Generic;

namespace ImisRestApi.Models.Entities
{
    public partial class TblRelIndex
    {
        public int RelIndexId { get; set; }
        public int ProdId { get; set; }
        public byte RelType { get; set; }
        public string RelCareType { get; set; }
        public int RelYear { get; set; }
        public byte RelPeriod { get; set; }
        public DateTime CalcDate { get; set; }
        public decimal? RelIndex { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int AuditUserId { get; set; }
        public int? LocationId { get; set; }

        public TblProduct Prod { get; set; }
    }
}
