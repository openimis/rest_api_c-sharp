using System;
using System.Collections.Generic;

namespace OpenImis.DB.SqlServer
{
    public partial class TblClaimServices
    {
        public int ClaimServiceId { get; set; }
        public int ClaimId { get; set; }
        public int ServiceId { get; set; }
        public int? ProdId { get; set; }
        public byte ClaimServiceStatus { get; set; }
        public decimal QtyProvided { get; set; }
        public decimal? QtyApproved { get; set; }
        public decimal PriceAsked { get; set; }
        public decimal? PriceAdjusted { get; set; }
        public decimal? PriceApproved { get; set; }
        public decimal? PriceValuated { get; set; }
        public string Explanation { get; set; }
        public string Justification { get; set; }
        public short? RejectionReason { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int AuditUserId { get; set; }
        public DateTime? ValidityFromReview { get; set; }
        public DateTime? ValidityToReview { get; set; }
        public int? AuditUserIdreview { get; set; }
        public decimal? LimitationValue { get; set; }
        public string Limitation { get; set; }
        public int? PolicyId { get; set; }
        public decimal? RemuneratedAmount { get; set; }
        public decimal? DeductableAmount { get; set; }
        public decimal? ExceedCeilingAmount { get; set; }
        public string PriceOrigin { get; set; }
        public decimal? ExceedCeilingAmountCategory { get; set; }

        public TblClaim Claim { get; set; }
        public TblProduct Prod { get; set; }
        public TblServices Service { get; set; }
    }
}
