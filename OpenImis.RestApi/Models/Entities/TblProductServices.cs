using System;
using System.Collections.Generic;

namespace OpenImis.RestApi.Models.Entities
{
    public partial class TblProductServices
    {
        public int ProdServiceId { get; set; }
        public int ProdId { get; set; }
        public int ServiceId { get; set; }
        public string LimitationType { get; set; }
        public string PriceOrigin { get; set; }
        public decimal? LimitAdult { get; set; }
        public decimal? LimitChild { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int AuditUserId { get; set; }
        public byte[] RowId { get; set; }
        public int? WaitingPeriodAdult { get; set; }
        public int? WaitingPeriodChild { get; set; }
        public int? LimitNoAdult { get; set; }
        public int? LimitNoChild { get; set; }
        public string LimitationTypeR { get; set; }
        public string LimitationTypeE { get; set; }
        public decimal? LimitAdultR { get; set; }
        public decimal? LimitAdultE { get; set; }
        public decimal? LimitChildR { get; set; }
        public decimal? LimitChildE { get; set; }
        public string CeilingExclusionAdult { get; set; }
        public string CeilingExclusionChild { get; set; }

        public TblProduct Prod { get; set; }
        public TblServices Service { get; set; }
    }
}
