using System;
using System.Collections.Generic;

namespace ImisRestApi.Models.Entities
{
    public partial class TblClaim
    {
        public TblClaim()
        {
            TblClaimItems = new HashSet<TblClaimItems>();
            TblClaimServices = new HashSet<TblClaimServices>();
            TblFeedback = new HashSet<TblFeedback>();
        }

        public int ClaimId { get; set; }
        public int InsureeId { get; set; }
        public string ClaimCode { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int Icdid { get; set; }
        public byte ClaimStatus { get; set; }
        public int? Adjuster { get; set; }
        public string Adjustment { get; set; }
        public decimal? Claimed { get; set; }
        public decimal? Approved { get; set; }
        public decimal? Reinsured { get; set; }
        public decimal? Valuated { get; set; }
        public DateTime DateClaimed { get; set; }
        public DateTime? DateProcessed { get; set; }
        public bool Feedback { get; set; }
        public int? FeedbackId { get; set; }
        public string Explanation { get; set; }
        public byte? FeedbackStatus { get; set; }
        public byte? ReviewStatus { get; set; }
        public byte? ApprovalStatus { get; set; }
        public byte? RejectionReason { get; set; }
        public DateTime ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int AuditUserId { get; set; }
        public DateTime? ValidityFromReview { get; set; }
        public DateTime? ValidityToReview { get; set; }
        public int? AuditUserIdreview { get; set; }
        public byte[] RowId { get; set; }
        public int Hfid { get; set; }
        public int? RunId { get; set; }
        public int? AuditUserIdsubmit { get; set; }
        public int? AuditUserIdprocess { get; set; }
        public DateTime? SubmitStamp { get; set; }
        public DateTime? ProcessStamp { get; set; }
        public decimal? Remunerated { get; set; }
        public string GuaranteeId { get; set; }
        public int? ClaimAdminId { get; set; }
        public int? Icdid1 { get; set; }
        public int? Icdid2 { get; set; }
        public int? Icdid3 { get; set; }
        public int? Icdid4 { get; set; }
        public string VisitType { get; set; }
        public string ClaimCategory { get; set; }

        public TblUsers AdjusterNavigation { get; set; }
        public TblClaimAdmin ClaimAdmin { get; set; }
        public TblFeedback FeedbackNavigation { get; set; }
        public TblHf Hf { get; set; }
        public TblIcdcodes Icd { get; set; }
        public TblInsuree Insuree { get; set; }
        public TblBatchRun Run { get; set; }
        public ICollection<TblClaimItems> TblClaimItems { get; set; }
        public ICollection<TblClaimServices> TblClaimServices { get; set; }
        public ICollection<TblFeedback> TblFeedback { get; set; }
    }
}
