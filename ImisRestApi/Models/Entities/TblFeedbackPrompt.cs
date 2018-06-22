using System;
using System.Collections.Generic;

namespace ImisRestApi.Models.Entities
{
    public partial class TblFeedbackPrompt
    {
        public int FeedbackPromptId { get; set; }
        public DateTime FeedbackPromptDate { get; set; }
        public int? ClaimId { get; set; }
        public int? OfficerId { get; set; }
        public string PhoneNumber { get; set; }
        public byte? Smsstatus { get; set; }
        public DateTime? ValidityFrom { get; set; }
        public DateTime? ValidityTo { get; set; }
        public int? LegacyId { get; set; }
        public int? AuditUserId { get; set; }
    }
}
