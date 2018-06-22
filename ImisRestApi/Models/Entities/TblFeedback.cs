using System;
using System.Collections.Generic;

namespace ImisRestApi.Models.Entities
{
    public partial class TblFeedback
    {
        public TblFeedback()
        {
            TblClaim = new HashSet<TblClaim>();
        }

        public int FeedbackId { get; set; }
        public int ClaimId { get; set; }
        public bool? CareRendered { get; set; }
        public bool? PaymentAsked { get; set; }
        public bool? DrugPrescribed { get; set; }
        public bool? DrugReceived { get; set; }
        public byte? Asessment { get; set; }
        public int? ChfofficerCode { get; set; }
        public DateTime? FeedbackDate { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int AuditUserId { get; set; }

        public TblClaim Claim { get; set; }
        public ICollection<TblClaim> TblClaim { get; set; }
    }
}
