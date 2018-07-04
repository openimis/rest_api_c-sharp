using System;
using System.Collections.Generic;

namespace OpenImis.RestApi.Models.Entities
{
    public partial class TblClaimDedRem
    {
        public int ExpenditureId { get; set; }
        public int PolicyId { get; set; }
        public int InsureeId { get; set; }
        public int ClaimId { get; set; }
        public decimal? DedG { get; set; }
        public decimal? DedOp { get; set; }
        public decimal? DedIp { get; set; }
        public decimal? RemG { get; set; }
        public decimal? RemIp { get; set; }
        public decimal? RemOp { get; set; }
        public decimal? RemConsult { get; set; }
        public decimal? RemSurgery { get; set; }
        public decimal? RemDelivery { get; set; }
        public decimal? RemHospitalization { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int AuditUserId { get; set; }
        public decimal? RemAntenatal { get; set; }

        public TblInsuree Insuree { get; set; }
        public TblPolicy Policy { get; set; }
    }
}
